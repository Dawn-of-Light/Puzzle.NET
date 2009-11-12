// *
// * Copyright (C) 2005 Mats Helander : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.Collections;
using Puzzle.NPersist.Framework.BaseClasses;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Exceptions;
using Puzzle.NPersist.Framework.Interfaces;
using Puzzle.NPersist.Framework.Mapping;

namespace Puzzle.NPersist.Framework.Persistence
{
	public class UnitOfWork : ContextChild, IUnitOfWork
	{
		private ArrayList m_listCreated = new ArrayList();
		private ArrayList m_listDirty = new ArrayList();
		private Hashtable m_hashStillDirty = new Hashtable();
		private ArrayList m_listDeleted = new ArrayList();
//		private ArrayList m_listPOCO = new ArrayList();
		private Hashtable m_objectStatusLookup = new Hashtable() ;

		private ArrayList m_listInserted = new ArrayList();
		private ArrayList m_listUpdated = new ArrayList();
		private ArrayList m_listRemoved = new ArrayList();

		private Hashtable m_hashSpeciallyUpdated = new Hashtable();

		private TopologicalGraph m_topologicalDelete = new TopologicalGraph();

        public virtual void RegisterCreated(object obj)
		{	
			this.Context.LogManager.Info(this, "Registering object as up for creation", "Type: " + obj.GetType().ToString()); // do not localize
			object result = m_objectStatusLookup[obj];
			if (result != null)
			{
				ObjectStatus objStatus = (ObjectStatus) result;
				if (objStatus == ObjectStatus.Dirty)
				{
					throw new UnitOfWorkException("Can't register object as 'Created' when it is already registered as 'Dirty'!"); // do not localize					
				}
				if (objStatus == ObjectStatus.UpForDeletion)
				{
					throw new UnitOfWorkException("Can't register object as 'Created' when it is already registered as 'Deleted'!"); // do not localize
				}
				if (objStatus == ObjectStatus.UpForCreation)
				{
					throw new UnitOfWorkException("Object is already registered as 'Created'!"); // do not localize
				}
			}
			this.Context.ObjectCloner.EnsureIsClonedIfEditing(obj);
			m_listCreated.Add(obj);
			m_objectStatusLookup[obj] = ObjectStatus.UpForCreation;
			this.Context.ObjectManager.SetObjectStatus(obj, ObjectStatus.UpForCreation);
			this.Context.IdentityMap.RegisterCreatedObject(obj);
		}

		public virtual void RegisterDirty(object obj)
		{
			this.Context.LogManager.Info(this, "Registering object as dirty", "Type: " + obj.GetType().ToString()); // do not localize
			object result = m_objectStatusLookup[obj];
			if (result != null)
			{
				ObjectStatus objStatus = (ObjectStatus) result;
				if (objStatus == ObjectStatus.UpForDeletion)
				{
					throw new UnitOfWorkException("Can't register object as 'Dirty' when it is already registered as 'Deleted'!"); // do not localize
				}
				if (objStatus == ObjectStatus.UpForCreation)
				{
					return;
				}
			}
			this.Context.ObjectCloner.EnsureIsClonedIfEditing(obj);

			//Following bug fix (adding the if) submitted by Vlad Ivanov
			if(m_listDirty.IndexOf(obj)==-1) 
			{ 
				m_listDirty.Add(obj); 
			} 

			m_objectStatusLookup[obj] = ObjectStatus.Dirty;
			this.Context.ObjectManager.SetObjectStatus(obj, ObjectStatus.Dirty);
		}

		public virtual void RegisterDeleted(object obj)
		{
			this.Context.LogManager.Info(this, "Registering object as up for deletion", "Type: " + obj.GetType().ToString()); // do not localize

			object result = m_objectStatusLookup[obj];
            bool addToDeleted = true;
			if (result != null)
			{
				ObjectStatus objStatus = (ObjectStatus) result;
				if (objStatus == ObjectStatus.UpForCreation)
				{
                    //If the object has been created during the same Unit of Work, we don't have to
                    //wait for a commit operation to let the object enter a state of Deleted (that is,
                    //the object does not have to enter the state of UpForDeletion until commit makes it Deleted)
					m_listCreated.Remove(obj);
					m_objectStatusLookup.Remove(obj);
                    this.Context.ObjectManager.ClearUpdatedStatuses(obj);
                    this.Context.ObjectManager.SetObjectStatus(obj, ObjectStatus.Deleted);
                    addToDeleted = false;
				}
			}
			this.Context.ObjectCloner.EnsureIsClonedIfEditing(obj);
            if (addToDeleted)
            {
                m_listDirty.Remove(obj);
                m_listDeleted.Add(obj);
                m_objectStatusLookup[obj] = ObjectStatus.UpForDeletion;
                this.Context.ObjectManager.SetObjectStatus(obj, ObjectStatus.UpForDeletion);
            }
			this.Context.IdentityMap.RemoveObject(obj);
		}

		public virtual void RegisterClean(object obj)
		{
			this.Context.LogManager.Info(this, "Registering object as clean", "Type: " + obj.GetType().ToString()); // do not localize
			m_objectStatusLookup.Remove(obj);
			m_listCreated.Remove(obj);
			m_listDirty.Remove(obj);
			m_listDeleted.Remove(obj);
			this.Context.ObjectManager.SetObjectStatus(obj, ObjectStatus.Clean);
		}

//		public virtual ObjectStatus GetObjectStatus(object obj)
//		{
//			object result = m_objectStatusLookup[obj];
//			if (result != null)
//			{
//				return (ObjectStatus) result;
//			}
//			return this.Context.IdentityMap.GetObjectStatus(obj);
//		}

		public virtual void Complete()
		{
            NotifyCommitted();
			CommitInserted();
			CommitUpdated();
			CommitRemoved();
			CommitSpeciallyUpdated();
			if (this.Context.IsEditing)
				this.Context.EndEdit() ;
		}

		public virtual void Abort()
		{
			AbortInserted();
			AbortUpdated();
			AbortRemoved();
			AbortSpeciallyUpdated();
			if (this.Context.IsEditing)
				this.Context.CancelEdit() ;
		}

		public virtual void AbortInserted()
		{
			IObjectManager om = this.Context.ObjectManager;
			foreach (object obj in m_listInserted)
			{
				IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType() );
				//we must roll back autoincreasers
				if (classMap.HasAssignedBySource() )
				{
					foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps() )
					{
						if (propertyMap.IsAssignedBySource)
						{
							string prevId = om.GetObjectIdentity(obj);
							om.SetPropertyValue(obj, propertyMap.Name, 0);			
							this.Context.IdentityMap.UpdateIdentity(obj, prevId);
						}
					}
				}
				m_listCreated.Add(obj);
				om.SetObjectStatus(obj, ObjectStatus.UpForCreation);
			}			
			m_listInserted.Clear() ;
		}

		public virtual void AbortUpdated()
		{
			foreach (object obj in m_listUpdated)
			{
				m_listDirty.Add(obj);
			}			
			m_listUpdated.Clear() ;
		}

		public virtual void AbortRemoved()
		{
			foreach (object obj in m_listRemoved)
			{
				m_listDeleted.Add(obj);
			}			
			m_listRemoved.Clear() ;
		}

   
		protected void AddSpeciallyUpdated(object obj)
		{
			Hashtable cachedOriginals = (Hashtable) m_hashSpeciallyUpdated[obj];
			if (cachedOriginals != null)
				return;

			cachedOriginals = new Hashtable();
			m_hashSpeciallyUpdated[obj] = cachedOriginals;

			IObjectManager om = this.Context.ObjectManager;
			IListManager lm = this.Context.ListManager;
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType() );

			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps() )
			{
				cachedOriginals[propertyMap.Name] = om.GetOriginalPropertyValue(obj, propertyMap.Name);			
				CopyValuesToOriginals(propertyMap, lm, obj, om);
			}
		}

		protected void AbortSpeciallyUpdated()
		{
			foreach (object obj in m_hashSpeciallyUpdated.Keys)
			{
				AbortSpeciallyUpdated(obj);
			}
		}

		protected void AbortSpeciallyUpdated(object obj)
		{
			Hashtable cachedOriginals = (Hashtable) m_hashSpeciallyUpdated[obj];
			if (cachedOriginals != null)
				return;

			cachedOriginals = new Hashtable();
			m_hashSpeciallyUpdated[obj] = cachedOriginals;

			IObjectManager om = this.Context.ObjectManager;
			IListManager lm = this.Context.ListManager;
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType() );

			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps() )
			{
				om.SetOriginalPropertyValue(obj, propertyMap.Name, cachedOriginals[propertyMap.Name]);
			}
		}


        public virtual void NotifyCommitted()
		{
			foreach (object obj in m_listInserted)
			{
                this.Context.InverseManager.NotifyCommitted(obj);
			}
			foreach (object obj in m_listUpdated)
			{
                this.Context.InverseManager.NotifyCommitted(obj);
			}			
			foreach (object obj in m_listRemoved)
			{
                this.Context.InverseManager.NotifyCommitted(obj);
			}			
		}

		public virtual void CommitInserted()
		{
			foreach (object obj in m_listInserted)
			{
				CommitPersisted(obj);
			}
			m_listInserted.Clear() ;
		}

		public virtual void CommitUpdated()
		{
			foreach (object obj in m_listUpdated)
			{
				CommitPersisted(obj);
			}			
			m_listUpdated.Clear() ;
		}

		public virtual void CommitRemoved()
		{
			foreach (object obj in m_listRemoved)
			{
				m_objectStatusLookup.Remove(obj);
				this.Context.ObjectManager.ClearUpdatedStatuses(obj);
				this.Context.ObjectManager.SetObjectStatus(obj, ObjectStatus.Deleted);
			}			
			m_listRemoved.Clear() ;
		}

		public virtual void CommitSpeciallyUpdated()
		{
			m_hashSpeciallyUpdated.Clear() ;
		}

		protected virtual void CommitPersisted(object obj)
		{
			IObjectManager om = this.Context.ObjectManager;
			IListManager lm = this.Context.ListManager;
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType() );
			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps() )
			{
				CopyValuesToOriginals(propertyMap, lm, obj, om);
			}
			m_objectStatusLookup.Remove(obj);
			this.Context.ObjectManager.ClearUpdatedStatuses(obj);
			this.Context.ObjectManager.SetObjectStatus(obj, ObjectStatus.Clean);
		}

		protected void CopyValuesToOriginals(IPropertyMap propertyMap, IListManager lm, object obj, IObjectManager om)
		{
			if (propertyMap.IsCollection)
			{
				IList list =   lm.CloneList(obj, propertyMap, ((IList) (om.GetPropertyValue(obj, propertyMap.Name))));
				om.SetOriginalPropertyValue(obj, propertyMap.Name, list);						
			}
			else
			{
				if (om.GetNullValueStatus(obj, propertyMap.Name))
				{
					om.SetOriginalPropertyValue(obj, propertyMap.Name, System.DBNull.Value);						
				}
				else
				{						
					om.SetOriginalPropertyValue(obj, propertyMap.Name, om.GetPropertyValue(obj, propertyMap.Name));
				}						
			}
		}


		private IList exceptions = null;

		public virtual void Commit(int exceptionLimit)
		{
			this.Context.LogManager.Info(this, "Committing Unit of Work", ""); // do not localize

			exceptions = new ArrayList(); 
			m_hashSpeciallyUpdated.Clear() ;

			try
			{

				if (this.Context.ValidateBeforeCommit)
				{
					this.Context.ValidateUnitOfWork(exceptions);

					//Bug in following line fixed by Vlad Ivanov
					if (exceptions!=null && exceptions.Count > 0)
					{
						Abort();	

						throw new ExceptionLimitExceededException(exceptions);					
					}

				}

				this.Context.PersistenceEngine.Begin();

				InsertCreated(exceptionLimit); 
				UpdateDirty(exceptionLimit);
				UpdateStillDirty(exceptionLimit);
                ExamineDeletedObjects();
				RemoveDeleted(exceptionLimit);	
				
				this.Context.PersistenceEngine.Commit();

				//Bug in following line fixed by Vlad Ivanov
				if (exceptions!=null && exceptions.Count > 0)
				{
					Abort();	

					throw new ExceptionLimitExceededException(exceptions);					
				}
				else
				{
					Complete();					
				}
			}
			catch (Exception ex)
			{
				Abort();	

				if (exceptionLimit == 1)
					throw ex;

				if (ex != null && ex.GetType() != typeof(ExceptionLimitExceededException))
					exceptions.Add(ex);

				if (exceptions.Count == 1)
					throw (Exception) exceptions[0];

				throw new ExceptionLimitExceededException(exceptions);
			}
		}

		public void CommitObject(object obj, int exceptionLimit)
		{
			this.Context.LogManager.Info(this, "Committing object", "Type: " + obj.GetType().ToString()); // do not localize

			exceptions = new ArrayList(); 
			m_hashSpeciallyUpdated.Clear() ;
			
			try
			{
				this.Context.PersistenceEngine.Begin();

				InsertCreated(obj, exceptionLimit);
				UpdateDirty(obj, exceptionLimit);
				UpdateStillDirty(obj, exceptionLimit);
				RemoveDeleted(obj, exceptionLimit);

				this.Context.PersistenceEngine.Commit();

				//Bug in following line fixed by Vlad Ivanov
				if (exceptions != null && exceptions.Count > 0)
				{
					this.Context.PersistenceEngine.Abort();
					Abort();	

					throw new ExceptionLimitExceededException(exceptions);					
				}
				else
				{
					Complete();					
				}
			}
			catch (Exception ex)
			{
				this.Context.PersistenceEngine.Abort();
				Abort();				

				if (exceptionLimit == 1)
					throw ex;

				if (ex != null && ex.GetType() != typeof(ExceptionLimitExceededException) )
					exceptions.Add(ex);

				if (exceptions.Count == 1)
					throw (Exception) exceptions[0];

				throw new ExceptionLimitExceededException(exceptions);
			}

		}

		protected virtual void InsertCreated(int exceptionLimit)
		{
			InsertCreated(null, exceptionLimit);
		}

		protected virtual void InsertCreated(object forObj, int exceptionLimit)
		{
			this.Context.LogManager.Debug(this, "Inserting objects that are up for creation", "");	 // do not localize			

			try
			{
				long cnt;
				int cntStale = 0;
				bool noCheck = false;
				IList stillDirty = new ArrayList() ;
				ArrayList insertObjects = new ArrayList();
				cnt = m_listCreated.Count;
				IObjectManager om = this.Context.ObjectManager;
				IPersistenceEngine pe = this.Context.PersistenceEngine;
				while (cnt > 0)
				{
					try
					{
						insertObjects.Clear();
						foreach (object obj in m_listCreated)
						{
							try
							{
								if (forObj != null)
								{
									if (obj == forObj)
									{
										insertObjects.Add(obj);
									}
								}
								else
								{
									if (noCheck)
									{							
										if (MayInsert(obj, true, true))
										{							
											insertObjects.Add(obj);
											noCheck = false;
										}
									}
									else
									{
										if (MayInsert(obj, true, false))
										{							
											insertObjects.Add(obj);
										}							
									}
								}							
							}
							catch (Exception ex)
							{
								if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
									throw ex;
								exceptions.Add(ex);
							}
						}
						foreach (object obj in insertObjects)
						{
							//this should be the only necessary try block in this 
							//method (and it ought only to be around the call to the
							//persistence engine, at that) but we add the other tries
							//to ensure that some exception from the framework doesn't
							//ruin the beautiful concept with collecting exceptions :-)
							try 
							{
								m_listCreated.Remove(obj);
								m_listInserted.Add(obj);
								stillDirty.Clear() ;
								pe.InsertObject(obj, stillDirty);
								om.SetObjectStatus(obj, ObjectStatus.Clean);
								this.Context.LogManager.Debug(this, "Inserted object", "Type: " + obj.GetType().ToString() + " Still dirty: " + stillDirty.ToString() ); // do not localize
								if (stillDirty.Count > 0)
								{
									IList cloneList = new ArrayList();
									foreach (object clone in stillDirty)
									{
										cloneList.Add(clone);
									}
									m_hashStillDirty[obj] = cloneList ;
								}
							}
							catch (Exception ex)
							{
								if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
									throw ex;
								exceptions.Add(ex);
							}
						}
						if (m_listCreated.Count == cnt)
						{
							noCheck = true;
							cntStale++;
							if (cntStale > 1)
							{
								throw new NPersistException("Cyclic dependency among objects up for creation could not be resolved!"); // do not localize
							}
						}
						else
						{
							cntStale = 0;
							noCheck = false;
						}
						if (forObj != null)
						{
							cnt = 0;
						}
						else
						{
							cnt = m_listCreated.Count;
						}
					}
					catch (Exception ex)
					{
						if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
							throw ex;
						exceptions.Add(ex);
					}
				}				
			}
			catch (Exception ex)
			{
				if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
					throw ex;
				exceptions.Add(ex);
			}
		}

		protected virtual void UpdateDirty(int exceptionLimit)
		{
			UpdateDirty(null, exceptionLimit);
		}

		protected virtual void UpdateDirty(object forObj, int exceptionLimit)
		{
			this.Context.LogManager.Debug(this, "Updating dirty objects", ""); // do not localize				

			try
			{
				long cnt;
				bool noCheck = false;
				ArrayList updateObjects = new ArrayList();
				IList stillDirty = new ArrayList() ;
				cnt = m_listDirty.Count;
				IObjectManager om = this.Context.ObjectManager;
				IPersistenceEngine pe = this.Context.PersistenceEngine;
				while (cnt > 0)
				{
					try
					{
						updateObjects.Clear();
						foreach (object obj in m_listDirty)
						{
							try
							{
								if (forObj != null)
								{
									if (obj == forObj)
									{
										updateObjects.Add(obj);
									}
								}
								else
								{
									if (noCheck || MayUpdate(obj))
									{
										updateObjects.Add(obj);
										noCheck = false;
									}
								}						
							}
							catch (Exception ex)
							{
								if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
									throw ex;
								exceptions.Add(ex);
							}
						}
						foreach (object obj in updateObjects)
						{
							try 
							{
								m_listDirty.Remove(obj);
								m_listUpdated.Add(obj);
								stillDirty.Clear() ;
								pe.UpdateObject(obj, stillDirty);
								this.Context.LogManager.Debug(this, "Updated object", "Type: " + obj.GetType().ToString() + " Still dirty: " + stillDirty.Count.ToString() ); // do not localize
								if (stillDirty.Count > 0)
								{
									IList cloneList = new ArrayList();
									foreach (object clone in stillDirty)
									{
										cloneList.Add(clone);
									}
									m_hashStillDirty[obj] = cloneList ;						
								}
							}
							catch (Exception ex)
							{
								if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
									throw ex;
								exceptions.Add(ex);
							}
						}
						if (m_listDirty.Count == cnt)
						{
							noCheck = true;
						}
						if (forObj != null)
						{
							cnt = 0;
						}
						else
						{
							cnt = m_listDirty.Count;
						}
					
					}
					catch (Exception ex)
					{
						if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
							throw ex;
						exceptions.Add(ex);
					}
				}				
			}
			catch (Exception ex)
			{
				if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
					throw ex;
				exceptions.Add(ex);
			}
		}

		protected virtual void UpdateStillDirty(int exceptionLimit)
		{
			UpdateStillDirty(null, exceptionLimit);
		}

		protected virtual void UpdateStillDirty(object forObj, int exceptionLimit)
		{
			this.Context.LogManager.Debug(this, "Updating still dirty objects", ""); // do not localize				

			try
			{
				long cnt;
				bool noCheck = false;
				ArrayList updateObjects = new ArrayList();
				IList stillDirty;
				cnt = m_hashStillDirty.Count;
				IObjectManager om = this.Context.ObjectManager;
				IPersistenceEngine pe = this.Context.PersistenceEngine;
				while (cnt > 0)
				{
					try
					{
						updateObjects.Clear();
						foreach (object obj in m_hashStillDirty.Keys)
						{
							try
							{
								if (forObj != null)
								{
									if (obj == forObj)
									{
										updateObjects.Add(obj);
									}
								}
								else
								{
									if (noCheck || MayUpdate(obj))
									{
										updateObjects.Add(obj);
										noCheck = false;
									}
								}						
							}
							catch (Exception ex)
							{
								if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
									throw ex;
								exceptions.Add(ex);
							}
						}
						foreach (object obj in updateObjects)
						{
							try
							{
								stillDirty = (IList) m_hashStillDirty[obj] ;
								m_hashStillDirty.Remove(obj);
								pe.UpdateObject(obj, stillDirty);
								this.Context.LogManager.Debug(this, "Updated still dirty object", "Type: " + obj.GetType().ToString() + " Still dirty: " + stillDirty.Count.ToString() ); // do not localize
								if (stillDirty.Count > 0)
								{
									IList cloneList = new ArrayList();
									foreach (object clone in stillDirty)
									{
										cloneList.Add(clone);
									}
									m_hashStillDirty[obj] = cloneList ;						
								}						
							}
							catch (Exception ex)
							{
								if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
									throw ex;
								exceptions.Add(ex);
							}
						}
						if (m_hashStillDirty.Count == cnt)
						{
							noCheck = true;
						}
						if (forObj != null)
						{
							cnt = 0;
						}
						else
						{
							cnt = m_hashStillDirty.Count;
						}
					
					}
					catch (Exception ex)
					{
						if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
							throw ex;
						exceptions.Add(ex);
					}
				}
				
			}
			catch (Exception ex)
			{
				if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
					throw ex;
				exceptions.Add(ex);
			}
		}

		protected virtual void RemoveDeleted(int exceptionLimit)
		{
			RemoveDeleted(null, exceptionLimit);
		}

		protected virtual void RemoveDeleted(object forObj, int exceptionLimit)
		{
			this.Context.LogManager.Debug(this, "Removing objects that are up for deletion", ""); // do not localize			
	
			try
			{
				long cnt;
				long staleCnt = 0;
				bool tryForce = false;
				ArrayList removeObjects = new ArrayList();
				cnt = m_listDeleted.Count;
				IObjectManager om = this.Context.ObjectManager;
				IPersistenceEngine pe = this.Context.PersistenceEngine;
				while (cnt > 0)
				{
					try
					{
						removeObjects.Clear();
						foreach (object obj in m_listDeleted)
						{
							try
							{
								if (forObj != null)
								{
									if (obj == forObj)
									{
										removeObjects.Add(obj);
									}
								}
								else
								{
                                    if (tryForce)
                                    {
                                        if (MayForceDelete(obj))
                                        {
											//Force an update all the referencing objects, which should have had their
											//references to our object set to null in advance during the delete operation.
											//This way all the references to our object should be set to null in the database.
											TopologicalNode node = (TopologicalNode) m_topologicalDelete.Graph[obj];
											if (node != null)
											{
												foreach (TopologicalNode waitForNode in node.WaitFor)
												{
													IList dummyStillDirty = new ArrayList();
													pe.UpdateObject(waitForNode.Obj, dummyStillDirty);
													AddSpeciallyUpdated(waitForNode.Obj);
												}
											}
											tryForce = false;
											removeObjects.Add(obj);												
										}
                                    }
                                    else
                                    {
									    if (MayRemove(obj))
									    {
										    removeObjects.Add(obj);
									    }
                                    }
								}						
							}
							catch (Exception ex)
							{
								if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
									throw ex;
								exceptions.Add(ex);
							}
						}
						foreach (object obj in removeObjects)
						{
							try
							{
								m_listDeleted.Remove(obj);
								m_listRemoved.Add(obj);
								m_topologicalDelete.RemoveNode(obj);
								pe.RemoveObject(obj);
								this.Context.LogManager.Debug(this, "Removed object", "" ); // do not localize						
							}
							catch (Exception ex)
							{
								if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
									throw ex;
								exceptions.Add(ex);
							}
						}
						if (m_listDeleted.Count == cnt)
						{
                            if (staleCnt > 0)
                            {
                                throw new UnitOfWorkException("The objects that are up for deletion in the unit of work are arranged in an unresolvable graph!");
                            }
                            else 
                            {
							    tryForce = true;
                                staleCnt++;
                            }
						}
                        else
                        {
                            staleCnt = 0;
                        }
						if (forObj != null)
						{
							cnt = 0;
						}
						else
						{
							cnt = m_listDeleted.Count;
						}					
					}
					catch (Exception ex)
					{
						if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
							throw ex;
						exceptions.Add(ex);
					}
				}				
			}
			catch (Exception ex)
			{
				if (exceptionLimit > 0 && exceptions.Count >= exceptionLimit - 1)
					throw ex;
				exceptions.Add(ex);
			}
		}

		protected virtual bool MayInsert(object obj, bool checkAllReferences, bool weakCheck)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType());
			IObjectManager om = this.Context.ObjectManager;
			IList list;
			object refObj;
			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps())
			{
				if (!propertyMap.IsReadOnly && !propertyMap.IsSlave)
				{
					if (!(propertyMap.ReferenceType == ReferenceType.None))
					{
						if (checkAllReferences || propertyMap.MustGetReferencedClassMap().HasIdAssignedBySource())
						{
							if (propertyMap.IsCollection)
							{
								list = (IList) om.GetPropertyValue(obj, propertyMap.Name);
								if (list != null)
								{
									foreach (object itemRefObj in list)
									{
										if (m_listCreated.Contains(itemRefObj))
										{
											if (weakCheck)
											{
												if (propertyMap.IsIdentity)
												{
													return false;													
												}
											}
											else
											{
												return false;												
											}
										}
									}
								}
							}
							else
							{
								refObj = om.GetPropertyValue(obj, propertyMap.Name);
								if (refObj != null)
								{
									if (m_listCreated.Contains(refObj))
									{
										if (weakCheck)
										{
											if (propertyMap.IsIdentity)
											{
												return false;													
											}
										}
										else
										{
											return false;												
										}
									}
								}
							}
						}
					}
				}
			}
			return true;
		}

		protected virtual bool MayUpdate(object obj)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType());
			IObjectManager om = this.Context.ObjectManager;
			object refObj;
			IList list;
			foreach (IPropertyMap propertyMap in classMap.GetAllPropertyMaps())
			{
				if (!propertyMap.IsReadOnly && !propertyMap.IsSlave)
				{
					if (!(propertyMap.ReferenceType == ReferenceType.None))
					{
						if (propertyMap.IsCollection)
						{
							list = (IList) om.GetPropertyValue(obj, propertyMap.Name);
							if (list != null)
							{
								foreach (object itemRefObj in list)
								{
									if (m_listCreated.Contains(itemRefObj))
									{
										return false;
									}
								}
							}
						}
						else
						{
							refObj = om.GetPropertyValue(obj, propertyMap.Name);
							if (refObj != null)
							{
								if (m_listCreated.Contains(refObj))
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		protected virtual bool MayRemove(object obj)
		{
            if (m_topologicalDelete.IsWaiting(obj))
                return false;
            return true;
		}

		public ArrayList GetCreatedObjects()
		{
			return m_listCreated;
		}

		public ArrayList GetDeletedObjects()
		{
			return m_listDeleted;
		}

		public ArrayList GetDirtyObjects()
		{
			return m_listDirty;
		}

        private void ExamineDeletedObjects()
        {
            m_topologicalDelete.Graph.Clear();
            IObjectManager om = this.Context.ObjectManager;
            Hashtable hashDeleted = new Hashtable();
			foreach (object delObj in m_listDeleted)
			{
                hashDeleted[delObj] = delObj;
            }
			foreach (object delObj in m_listDeleted)
			{
                ExamineDeletedObject(hashDeleted, om, delObj);
            }
        }

        private void ExamineDeletedObject(Hashtable hashDeleted, IObjectManager om, object delObj)
        {
			IClassMap delObjClassMap = this.Context.DomainMap.MustGetClassMap(delObj.GetType());
			foreach (IPropertyMap propertyMap in delObjClassMap.GetAllPropertyMaps())
			{
				if (!propertyMap.IsReadOnly && !propertyMap.IsSlave)
				{
					if (propertyMap.ReferenceType != ReferenceType.None)
					{
						if (propertyMap.IsCollection)
						{
                            //It is the value in the database, not the current value, that is of importance
                            //for avoiding violations of the foreign key constraint 
							//IList list = (IList) om.GetPropertyValue(delObj, propertyMap.Name);
							IList list = (IList) om.GetOriginalPropertyValue(delObj, propertyMap.Name);
							if (list != null)
							{
								foreach (object itemRefObj in list)
								{
                                    object isDeleted = hashDeleted[itemRefObj];
									if (isDeleted != null)
									{
										m_topologicalDelete.AddNode(itemRefObj, delObj);
									}
								}
							}
						}
						else
						{
                            //It is the value in the database, not the current value, that is of importance
                            //for avoiding violations of the foreign key constraint 
							//object refObj = om.GetPropertyValue(delObj, propertyMap.Name);
							object refObj = om.GetOriginalPropertyValue(delObj, propertyMap.Name);
							if (refObj != null)
							{
                                object isDeleted = hashDeleted[refObj];
								if (isDeleted != null)
								{
									m_topologicalDelete.AddNode(refObj, delObj);
								}
							}
						}
					}
				}
			}
        }

        private bool MayForceDelete(object delObj)
        {
            TopologicalNode node = (TopologicalNode) m_topologicalDelete.Graph[delObj];
            if (node == null)
                return true;

            IObjectManager om = this.Context.ObjectManager;
            foreach (TopologicalNode waitForNode in node.WaitFor)
            {
                if (!ExamineWaitForNode(om, delObj, waitForNode.Obj))
                    return false;
            }

            return true;
        }
 
        private bool ExamineWaitForNode(IObjectManager om, object delObj, object waitForObj)
        {
			IClassMap waitForObjClassMap = this.Context.DomainMap.MustGetClassMap(waitForObj.GetType());
			foreach (IPropertyMap propertyMap in waitForObjClassMap.GetAllPropertyMaps())
			{
				if (!propertyMap.IsReadOnly && !propertyMap.IsSlave)
				{
					if (propertyMap.ReferenceType != ReferenceType.None)
					{
						if (propertyMap.IsCollection)
						{
							IList list = (IList) om.GetPropertyValue(waitForObj, propertyMap.Name);
							if (list != null)
							{
								foreach (object itemRefObj in list)
								{
									if (itemRefObj == delObj)
									{
										return false;
									}
								}
							}
						}
						else
						{
							object refObj = om.GetPropertyValue(waitForObj, propertyMap.Name);
							if (refObj != null)
							{
								if (refObj == delObj)
								{
									return false;
								}
							}
						}
					}
				}
			}
            return true;
        }

    }
}