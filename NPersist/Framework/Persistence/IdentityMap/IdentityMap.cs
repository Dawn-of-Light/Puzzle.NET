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
using Puzzle.NPersist.Framework.EventArguments;
using Puzzle.NPersist.Framework.Exceptions;
using Puzzle.NPersist.Framework.Interfaces;
using Puzzle.NPersist.Framework.Mapping;
using Puzzle.NPersist.Framework.Proxy;
using Puzzle.NPersist.Framework.Querying;

namespace Puzzle.NPersist.Framework.Persistence
{
	public class IdentityMap : ContextChild, IIdentityMap
	{
//		private Hashtable m_hashLoadedObjects = new Hashtable(1000);
//		private Hashtable m_hashUnloadedObjects = new Hashtable(1000);
//		private ArrayList m_ListAllObjects = new ArrayList(1000);

		//private Hashtable m_objectStatusLookup = new Hashtable(1000);

//		public virtual ObjectStatus GetObjectStatus(object obj)
//		{
//			if (obj == null)
//				throw new NullReferenceException("Can't return status for null object!"); // do not localize
//
//			object result = m_objectStatusLookup[obj];
//			if (result != null)
//				return (ObjectStatus) result;
//
//			return ObjectStatus.Deleted;
//		}

		public virtual IObjectCache GetObjectCache()
		{
			return this.Context.GetObjectCache(); 
		}

		public virtual void UnRegisterCreatedObject(object obj)
		{
			if (obj == null)
				throw new NullReferenceException("Can't unregister null object!"); // do not localize

			IContext ctx = this.Context;
			
			IObjectCache cache = GetObjectCache();

			cache.AllObjects.Remove(obj);

			string key = GetKey(obj);
			cache.LoadedObjects.Remove(key);
			//m_objectStatusLookup.Remove(key);
			ctx.ObjectManager.SetObjectStatus(obj, ObjectStatus.NotRegistered);

			ctx.LogManager.Info(this, "Unregistered created object", "Type: " + obj.GetType().ToString() + ", Key: " + key ); // do not localize
		}

		public virtual void RegisterCreatedObject(object obj)
		{
			if (obj == null)
				throw new NullReferenceException("Can't register null object as created!"); // do not localize

			IContext ctx = this.Context;

			string key = GetKey(obj);

			IObjectCache cache = GetObjectCache();

			object result = cache.LoadedObjects[key];
			if (result != null)
				throw new IdentityMapException("An object with the key " + key + " is already registered in the identity map!");

			result = cache.UnloadedObjects[key];
			if (result != null)
				throw new IdentityMapException("An object with the key " + key + " is already registered in the identity map!");

			//ctx.PersistenceManager.InitializeObject(obj);				
			
			if (cache.AllObjects != null)
				cache.AllObjects.Add(obj);

			cache.LoadedObjects[key] = obj;
			//m_objectStatusLookup[obj] = ObjectStatus.Clean;
			//ctx.ObjectManager.SetObjectStatus(obj, ObjectStatus.Clean);

			ctx.LogManager.Info(this, "Registered created object", "Type: " + obj.GetType().ToString() + ", Key: " + key ); // do not localize
		}

		public virtual void RegisterLoadedObject(object obj)
		{
			if (obj == null)
				throw new NullReferenceException("Can't register null object as loaded!"); // do not localize

			IContext ctx = this.Context;

			string key = GetKey(obj);

			IObjectCache cache = GetObjectCache();

			object result = cache.UnloadedObjects[key];
			if (result != null)
				cache.UnloadedObjects[key] = null;
			else
			{
				result = cache.LoadedObjects[key];
				if (result == null)
				{
					//ctx.PersistenceManager.InitializeObject(obj);				

					if (cache.AllObjects != null)
						cache.AllObjects.Add(obj);
				}
			}
			cache.LoadedObjects[key] = obj;
			this.Context.ReadOnlyObjectCacheManager.SaveObject(obj);
			
			if (this.Context.GetObjectStatus(obj) != ObjectStatus.Dirty)
			{
				//m_objectStatusLookup[obj] = ObjectStatus.Clean;
				ctx.ObjectManager.SetObjectStatus(obj, ObjectStatus.Clean);						
			}
			ctx.LogManager.Info(this, "Registered loaded object", "Type: " + obj.GetType().ToString() + ", Key: " + key ); // do not localize
		}

		public virtual void RegisterLazyLoadedObject(object obj)
		{
			if (obj == null)
				throw new NullReferenceException("Can't register null object as lazy loaded!"); // do not localize

			IContext ctx = this.Context;

			//ctx.PersistenceManager.InitializeObject(obj);

			IObjectCache cache = GetObjectCache();

			if (cache.AllObjects != null)
				cache.AllObjects.Add(obj);
			string key = GetKey(obj);
			
			cache.UnloadedObjects[key] = obj;
			//m_objectStatusLookup[obj] = ObjectStatus.NotLoaded;
			ctx.ObjectManager.SetObjectStatus(obj, ObjectStatus.NotLoaded);
			ctx.LogManager.Info(this, "Registered lazy loaded object", "Type: " + obj.GetType().ToString() + ", Key: " + key ); // do not localize
		}

		public virtual void UpdateIdentity(object obj, string previousIdentity)
		{
			if (obj == null)
			{
				throw new NullReferenceException("Can't update temporary identity for null object!"); // do not localize
			}
			string key = GetKey(obj);
			string prevKey = GetKey(obj, previousIdentity);

			IObjectCache cache = GetObjectCache();

			object result = cache.LoadedObjects[prevKey];
			if (result != null)
			{
				cache.LoadedObjects[key] = obj;
				cache.LoadedObjects[prevKey] = null;
			}
			else
			{
				result = cache.UnloadedObjects[prevKey];
				if (result != null)
				{
					cache.UnloadedObjects[key] = obj;
					cache.UnloadedObjects[prevKey] = null;
				}				
			}
			this.Context.LogManager.Debug(this, "Updated identity", "Type: " + obj.GetType().ToString() + ", New Key: " + key  + ", Previous Key: " + prevKey ); // do not localize
		}

		public virtual void UpdateIdentity(object obj, string previousIdentity, string newIdentity)
		{
			if (obj == null)
			{
				throw new NullReferenceException("Can't update temporary identity for null object!"); // do not localize
			}
			string key = GetKey(obj, newIdentity);
			string prevKey = GetKey(obj, previousIdentity);

			IObjectCache cache = GetObjectCache( );

			object result = cache.LoadedObjects[prevKey];
			if (result != null)
			{
				cache.LoadedObjects[key] = obj;
				cache.LoadedObjects[prevKey] = null;
			}
			else
			{
				result = cache.UnloadedObjects[prevKey];
				if (result != null)
				{
					cache.UnloadedObjects[key] = obj;
					cache.UnloadedObjects[prevKey] = null;
				}				
			}
			this.Context.LogManager.Debug(this, "Updated identity", "Type: " + obj.GetType().ToString() + ", New Key: " + key  + ", Previous Key: " + prevKey ); // do not localize
		}

		public virtual object GetObject(string identity, Type type)
		{
			return GetObject(identity, type, false);
		}

		public virtual object GetObject(string identity, Type type, bool lazy)
		{
			return GetObject(identity, type, lazy, false);			
		}

		public virtual object GetObject(string identity, Type type, bool lazy, bool ignoreObjectNotFound)
		{
			type = this.Context.AssemblyManager.GetType(type);
			//string key = type.ToString() + "." + identity;
			string key = GetKey(type, identity);
			object obj;
			ObjectCancelEventArgs e;

			IObjectCache cache = GetObjectCache();

			obj = cache.LoadedObjects[key];
			if (obj != null)
			{
				e = new ObjectCancelEventArgs(obj);
				this.Context.EventManager.OnGettingObject(this, e);
				if (e.Cancel)
				{
					return null;
				}
			}
			else
			{
				obj = cache.UnloadedObjects[key];
				if (obj != null)
				{
					e = new ObjectCancelEventArgs(obj);
					this.Context.EventManager.OnGettingObject(this, e);
					if (e.Cancel)
					{
						return null;
					}
				}
				else
				{
					obj = this.Context.AssemblyManager.CreateInstance(type);
					this.Context.ObjectManager.SetObjectIdentity(obj, identity);
					e = new ObjectCancelEventArgs(obj);
					this.Context.EventManager.OnGettingObject(this, e);
					if (e.Cancel)
					{
						return null;
					}
					if (lazy)
					{
						RegisterLazyLoadedObject(obj);
						cache.UnloadedObjects[key] = obj;
					}
					else
					{
						LoadObject(identity, ref obj, ignoreObjectNotFound, type, key);
					}
				}				
			}
			ObjectEventArgs e2 = new ObjectEventArgs(obj);
			this.Context.EventManager.OnGotObject(this, e2);
			return obj;
		}

		public void LoadObject(ref object obj, bool ignoreObjectNotFound)
		{
			Type type = this.Context.AssemblyManager.GetType(obj.GetType());
			string identity = this.Context.ObjectManager.GetObjectIdentity(obj);
			string key = GetKey(type, identity);
			LoadObject(identity, ref obj, ignoreObjectNotFound, type, key);			
		}

		private void LoadObject(string identity, ref object obj, bool ignoreObjectNotFound, Type type, string key)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType() );
			IObjectCache cache = GetObjectCache();

			if (classMap.IsReadOnly)
			{
				if (this.Context.ReadOnlyObjectCacheManager.LoadObject(obj))
				{
					cache.LoadedObjects[key] = obj;
					return;					
				}
			}

			if (classMap.LoadSpan.Length > 0)
			{
				IList listToFill = new ArrayList();
				NPathQuery query = this.Context.GetLoadObjectNPathQuery(obj, classMap.LoadSpan);
				this.Context.PersistenceEngine.LoadObjects( query, listToFill );
				if (listToFill.Count < 1)
				{
					if (ignoreObjectNotFound == false)
					{
						throw new ObjectNotFoundException("Object of type " + type.ToString() + " and with identity '" + identity + "' not found!"); // do not localize							
					}
					obj = null;
				}
				else if (listToFill.Count > 1)
				{
					//throw new WtfException("I thought you said your primary keys were unique, no??")
					obj = null;
				}
				else
				{
					obj = listToFill[0];
					cache.LoadedObjects[key] = obj;						
				}							
			}
			else
			{
				this.Context.PersistenceEngine.LoadObject(ref obj);
				if (obj == null)
				{
					if (ignoreObjectNotFound == false)
					{
						throw new ObjectNotFoundException("Object of type " + type.ToString() + " and with identity '" + identity + "' not found!"); // do not localize							
					}
				}
				else
				{
					cache.LoadedObjects[key] = obj;						
				}							
			}
		}

		public virtual bool HasObject(object obj)
		{
			return HasObject(this.Context.ObjectManager.GetObjectIdentity(obj), obj.GetType() );
		}

		public virtual bool HasObject(string identity, Type type)
		{
			type = this.Context.AssemblyManager.GetType(type);
			string key = GetKey(type, identity);
			object obj;

			IObjectCache cache = GetObjectCache();

			obj = cache.LoadedObjects[key];
			if (obj != null)
			{
				return true;
			}
			else
			{
				obj = cache.UnloadedObjects[key];
				if (obj != null)
				{
					return true;
				}				
			}
			return false;
		}




		public virtual object GetObjectByKey(string keyPropertyName, object keyValue, Type type)
		{
			return GetObjectByKey(keyPropertyName, keyValue, type, false);			
		}

		public virtual object GetObjectByKey(string keyPropertyName, object keyValue, Type type, bool ignoreObjectNotFound)
		{
			type = this.Context.AssemblyManager.GetType(type);
			object obj;
			string key;
			string identity;
			obj = this.Context.AssemblyManager.CreateInstance(type);
			ObjectCancelEventArgs e = new ObjectCancelEventArgs(obj);
			this.Context.EventManager.OnGettingObject(this, e);
			if (e.Cancel)
			{
				return null;
			}
			this.Context.ObjectManager.SetPropertyValue(obj, keyPropertyName, keyValue);
			//this.Context.SqlEngineManager.LoadObjectByKey(obj, keyPropertyName, keyValue);
			this.Context.PersistenceEngine.LoadObjectByKey(ref obj, keyPropertyName, keyValue);
			if (obj == null)
			{
				if (ignoreObjectNotFound == false)
				{
					throw new ObjectNotFoundException("Object not found!"); // do not localize					
				}
			}
			else
			{
				identity = this.Context.ObjectManager.GetObjectIdentity(obj);
//				key = type.ToString() + "." + identity;
				key = GetKey(type, identity);
				IObjectCache cache = GetObjectCache();
				obj = cache.LoadedObjects[key];
//				if (m_hashLoadedObjects.ContainsKey(key))
//				{
//					obj = m_hashLoadedObjects[key];
//				}
				ObjectEventArgs e2 = new ObjectEventArgs(obj);
				this.Context.EventManager.OnGotObject(this, e2);				
			}
			return obj;
		}

		public virtual void RemoveObject(object obj)
		{
			if (obj == null)
			{
				throw new NullReferenceException("Can't remove null object!"); // do not localize
			}
			string key = GetKey(obj);

			IObjectCache cache = GetObjectCache();

			cache.LoadedObjects[key] = null;
			cache.UnloadedObjects[key] = null;
			if (cache.AllObjects != null)
			{
				cache.AllObjects.Remove(obj);
			}
			//m_objectStatusLookup[obj] = null;
			RemoveAllReferencesToObject(obj);
		}

		//This might work better if we skip CascadeDelete props :-)

		protected virtual void RemoveAllReferencesToObject(object obj)
		{
//			IClassMap classMap = this.Context.DomainMap.GetClassMap(obj.GetType() );
//			object refObj;
//			IObjectManager om = this.Context.ObjectManager ;
//			foreach (object checkObject in GetObjects() )
//			{
//				IClassMap checkClassMap = this.Context.DomainMap.GetClassMap(checkObject.GetType() );
//				foreach (IPropertyMap propertyMap in checkClassMap.GetAllPropertyMaps() )
//				{
//					if (propertyMap.ReferenceType != ReferenceType.None)
//					{
//						if (propertyMap.GetReferencedClassMap() == classMap)
//						{
//							if (propertyMap.GetInversePropertyMap() == null)
//							{
//								if (checkObject != obj)
//								{
//									if (propertyMap.IsCollection)
//									{
//								
//									}
//									else
//									{
//										refObj = om.GetPropertyValue( checkObject, propertyMap.Name );
//										if (refObj != null)
//										{
//											if (refObj == obj)
//											{
//												om.SetOriginalPropertyValue( checkObject, propertyMap.Name, null );
//												om.SetPropertyValue( checkObject, propertyMap.Name, null );
//											}									
//										}
//									}																	
//								}
//							}
//						}
//					}
//				}
//
//			}
		}

		protected virtual string GetKey(object obj)
		{
			if (obj == null)
			{
				throw new NullReferenceException("Can't create key for null object!"); // do not localize
			}
			Type type = AssemblyManager.GetBaseType(obj);
			return type.ToString() + "." + this.Context.ObjectManager.GetObjectIdentity(obj);
//			return obj.GetType().ToString() + "." + this.Context.ObjectManager.GetObjectIdentity(obj);
		}

		protected virtual string GetKey(object obj, string identity)
		{
			if (obj == null)
			{
				throw new NullReferenceException("Can't create key for null object!"); // do not localize
			}
			Type type = AssemblyManager.GetBaseType(obj);
			return type.ToString() + "." + identity;
//			return obj.GetType().ToString() + "." + identity;
		}

		protected virtual string GetKey(Type type, string identity)
		{
			Type fixedType = AssemblyManager.GetBaseType(type);
			return fixedType.ToString() + "." + identity;
			//			return obj.GetType().ToString() + "." + identity;
		}


		public virtual IList GetObjects()
		{
			IObjectCache cache = GetObjectCache() ;
			IList cachedObjects = GetCacheObjects(cache);
			return cachedObjects;
		}

		private static IList GetCacheObjects(IObjectCache cache)
		{
			if (cache.AllObjects != null)
			{
				return cache.AllObjects;
			}
			else
			{
				IList objects = new ArrayList() ;
				foreach (object obj in cache.LoadedObjects.Values)
				{
					objects.Add(obj);
				}
				foreach (object obj in cache.UnloadedObjects.Values)
				{
					objects.Add(obj);
				}
				return objects;
			}
		}


		public virtual object TryGetObject(string identity, Type type)
		{
			type = this.Context.AssemblyManager.GetType(type);
			string key = type.ToString() + "." + identity;
			IObjectCache cache = GetObjectCache();
			return cache.LoadedObjects[key];
		}

	}
}