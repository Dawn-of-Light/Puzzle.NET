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
using System.Diagnostics;
using Puzzle.NPersist.Framework.BaseClasses;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Exceptions;
using Puzzle.NPersist.Framework.Interfaces;
using Puzzle.NPersist.Framework.Mapping;

namespace Puzzle.NPersist.Framework.Persistence
{
	public class DefaultObjectManager : ContextChild, IObjectManager
	{
		private ObjectManagerHelperPOCO m_helperPoco;

		public DefaultObjectManager() : base()
		{
			InitHelpers();
		}

		protected void InitHelpers()
		{
			m_helperPoco = new ObjectManagerHelperPOCO();
			m_helperPoco.ObjectManager = this;
		}

		public virtual string GetObjectIdentity(object obj)
		{
			return m_helperPoco.GetObjectIdentity(obj);
		}

		public virtual string GetObjectKey(object obj)
		{
			return m_helperPoco.GetObjectKey(obj);
		}

		public virtual string GetObjectKeyOrIdentity(object obj)
		{
			return m_helperPoco.GetObjectKeyOrIdentity(obj);
		}

		public virtual string GetObjectIdentity(object obj, IPropertyMap propertyMap, object value)
		{
			return m_helperPoco.GetObjectIdentity(obj, propertyMap, value);
		}

		public virtual void SetObjectIdentity(object obj, string identity)
		{
            try
            {
			    m_helperPoco.SetObjectIdentity(obj, identity);
            }
            catch (Exception ex)
            {
                throw new NPersistException(string.Format ("Could not set Identity '{0}' on object '{1}'",identity,obj), ex);
            }
		}


		public virtual string GetPropertyDisplayName(object obj, string propertyName)
		{
//			IClassMap classMap = this.Context.DomainMap.GetClassMap(obj.GetType() );
//			
//			string displayName = 
			return propertyName;
		}

		public virtual string GetPropertyDescription(object obj, string propertyName)
		{
			return "";
		}


		//[DebuggerStepThrough()]
		public virtual object GetPropertyValue(object obj, string propertyName)
		{
			return m_helperPoco.GetPropertyValue(obj, propertyName);
		}

		public virtual void SetPropertyValue(object obj, string propertyName, object value)
		{
			this.Context.ObjectCloner.EnsureIsClonedIfEditing(obj);
			m_helperPoco.SetPropertyValue(obj, propertyName, value);
		}


		//[DebuggerStepThrough()]
		public virtual ObjectStatus GetObjectStatus(object obj)
		{
			return ((IObjectStatusHelper) obj).GetObjectStatus() ;
		}

		public virtual void SetObjectStatus(object obj, ObjectStatus value)
		{
			this.Context.ObjectCloner.EnsureIsClonedIfEditing(obj);
			((IObjectStatusHelper) obj).SetObjectStatus(value);
		}

		
		public virtual PropertyStatus GetPropertyStatus(object obj, string propertyName)
		{
			return m_helperPoco.GetPropertyStatus(obj, propertyName);
		}

		public virtual object GetOriginalPropertyValue(object obj, string propertyName)
		{
			return ((IOriginalValueHelper) obj).GetOriginalPropertyValue(propertyName);
		}

		public virtual void SetOriginalPropertyValue(object obj, string propertyName, object value)
		{
			this.Context.ObjectCloner.EnsureIsClonedIfEditing(obj);
			((IOriginalValueHelper) obj).SetOriginalPropertyValue(propertyName, value);
		}

		public virtual void RemoveOriginalValues(object obj, string propertyName)
		{
			((IOriginalValueHelper) obj).RemoveOriginalValues(propertyName);
		}

		public virtual bool HasOriginalValues(object obj)
		{
			return ((IOriginalValueHelper) obj).HasOriginalValues();
		}

		public virtual bool HasOriginalValues(object obj, string propertyName)
		{
			return ((IOriginalValueHelper) obj).HasOriginalValues(propertyName);
		}

		public virtual bool IsDirtyProperty(object obj, string propertyName)
		{
			return m_helperPoco.IsDirtyProperty(obj, propertyName);
		}

		public virtual bool ComparePropertyValues(object obj, string propertyName, object value1, object value2)
		{
			return m_helperPoco.ComparePropertyValues(obj, propertyName, value1, value2);
		}

		//[DebuggerStepThrough()]
		public virtual bool GetNullValueStatus(object obj, string propertyName)
		{
			INullValueHelper nullValueHelper = obj as INullValueHelper;
			if (nullValueHelper != null)
				return nullValueHelper.GetNullValueStatus(propertyName);
			return false;
		}

		//[DebuggerStepThrough()]
		public virtual void SetNullValueStatus(object obj, string propertyName, bool value)
		{
			this.Context.ObjectCloner.EnsureIsClonedIfEditing(obj);
			((INullValueHelper) obj).SetNullValueStatus(propertyName, value);
		}

		
		public virtual bool GetUpdatedStatus(object obj, string propertyName)
		{
			return ((IUpdatedPropertyTracker) obj).GetUpdatedStatus(propertyName);
		}

		//[DebuggerStepThrough()]
		public virtual void SetUpdatedStatus(object obj, string propertyName, bool value)
		{
			this.Context.ObjectCloner.EnsureIsClonedIfEditing(obj);
			((IUpdatedPropertyTracker) obj).SetUpdatedStatus(propertyName, value);
		}

		public virtual void ClearUpdatedStatuses(object obj)
		{
			((IUpdatedPropertyTracker) obj).ClearUpdatedStatuses();
		}


		public virtual void EnsurePropertyIsLoaded(object obj, string propertyName)
		{
			IPropertyMap propertyMap = this.Context.DomainMap.MustGetClassMap(obj.GetType()).MustGetPropertyMap(propertyName);
			EnsurePropertyIsLoaded(obj, propertyMap);
		}

		public virtual void EnsurePropertyIsLoaded(object obj, IPropertyMap propertyMap)
		{
			IObjectManager om = this.Context.ObjectManager;
			IPersistenceEngine pe = this.Context.PersistenceEngine ;
			ObjectStatus objStatus;
			PropertyStatus propStatus;
			objStatus = om.GetObjectStatus(obj) ;
			if (objStatus != ObjectStatus.Deleted )
			{
				if (objStatus == ObjectStatus.NotLoaded )
				{
					pe.LoadObject(ref obj);
					if (pe == null)
					{
						throw new ObjectNotFoundException("Object not found!"); // do not localize
					}
				}
				propStatus = om.GetPropertyStatus(obj, propertyMap.Name) ;
				if (propStatus == PropertyStatus.NotLoaded )
				{
					pe.LoadProperty(obj, propertyMap.Name);
				}
			}
			else
			{
				//We really ought to throw an exception here...
			}
		}			


		public virtual void InvalidateObject(object obj)
		{
		}


		public virtual void InvalidateProperty(object obj, string propertyName)
		{
			IPropertyMap propertyMap = this.Context.DomainMap.MustGetClassMap(obj.GetType()).MustGetPropertyMap(propertyName);
			InvalidateProperty(obj, propertyMap);
		}

		public virtual void InvalidateProperty(object obj, IPropertyMap propertyMap)
		{
			IObjectManager om = this.Context.ObjectManager;
			IPersistenceEngine pe = this.Context.PersistenceEngine ;
			ObjectStatus objStatus;
			PropertyStatus propStatus;
			objStatus = om.GetObjectStatus(obj) ;
			propStatus = om.GetPropertyStatus(obj, propertyMap.Name) ;
			if (propStatus == PropertyStatus.NotLoaded )
			{
				RemoveOriginalValues(obj, propertyMap.Name);

				//pe.LoadProperty(obj, propertyMap.Name);
			}
		}			

		
		public virtual long GetTimeToLive(object obj)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType());
			return GetTimeToLive(classMap);
		}

		public virtual long GetTimeToLive(IClassMap classMap)
		{
			long ttl = classMap.GetTimeToLive();
			if (ttl < 0)
				ttl = this.Context.TimeToLive ;
			return ttl;
		}

		public virtual long GetTimeToLive(object obj, string propertyName)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType());
			IPropertyMap propertyMap = classMap.MustGetPropertyMap(propertyName);
			return GetTimeToLive(propertyMap);
		}

		public virtual long GetTimeToLive(IPropertyMap propertyMap)
		{
			long ttl = propertyMap.GetTimeToLive();
			if (ttl < 0)
				ttl = this.Context.TimeToLive ;
			return ttl;			
		}

		public virtual TimeToLiveBehavior GetTimeToLiveBehavior(object obj)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType());
			return GetTimeToLiveBehavior(classMap);			
		}

		public virtual TimeToLiveBehavior GetTimeToLiveBehavior(IClassMap classMap)
		{
			TimeToLiveBehavior ttlBehavior = classMap.GetTimeToLiveBehavior();
			if (ttlBehavior == TimeToLiveBehavior.Default)
				ttlBehavior = this.Context.TimeToLiveBehavior ;
			return ttlBehavior;			
		}

		public virtual TimeToLiveBehavior GetTimeToLiveBehavior(object obj, string propertyName)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType());
			IPropertyMap propertyMap = classMap.MustGetPropertyMap(propertyName);
			return GetTimeToLiveBehavior(propertyMap);
		}

		public virtual TimeToLiveBehavior GetTimeToLiveBehavior(IPropertyMap propertyMap)
		{
			TimeToLiveBehavior ttlBehavior = propertyMap.GetTimeToLiveBehavior();
			if (ttlBehavior == TimeToLiveBehavior.Default)
				ttlBehavior = this.Context.TimeToLiveBehavior ;
			return ttlBehavior;									
		}


		public virtual LoadBehavior GetLoadBehavior(object obj)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(obj.GetType());
			return GetLoadBehavior(classMap);			
		}

		public virtual LoadBehavior GetLoadBehavior(Type type)
		{
			IClassMap classMap = this.Context.DomainMap.MustGetClassMap(type);
			return GetLoadBehavior(classMap);			
		}

		public virtual LoadBehavior GetLoadBehavior(IClassMap classMap)
		{
			LoadBehavior loadBehavior = classMap.GetLoadBehavior();
			if (loadBehavior == LoadBehavior.Default)
				loadBehavior = this.Context.LoadBehavior;
			return loadBehavior;			
		}

		public ObjectManagerHelperPOCO ObjectManagerHelperPOCO
		{
			get { return m_helperPoco; }
			set { m_helperPoco = value; }
		}
	}
}