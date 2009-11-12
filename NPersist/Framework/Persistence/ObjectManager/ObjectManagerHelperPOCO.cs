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
using System.Globalization;
using System.Reflection;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Mapping;
using Puzzle.NPersist.Framework.Utility;
using Puzzle.NCore.Framework.Collections;
using Puzzle.NPersist.Framework.Interfaces;

namespace Puzzle.NPersist.Framework.Persistence
{
    public class ObjectManagerHelperPOCO
    {
        private IObjectManager m_ObjectManager;
        private Hashtable m_hashTempIds = new Hashtable();

        public virtual IObjectManager ObjectManager
        {
            get { return m_ObjectManager; }
            set { m_ObjectManager = value; }
        }

        public virtual string GetObjectIdentity(object obj)
        {
            return GetObjectIdentity(obj, null, null);
        }

        private string BuildObjectIdentity(object obj, IPropertyMap newPropertyMap, object newValue)
        {
            string id = "";
            IClassMap classMap = m_ObjectManager.Context.DomainMap.MustGetClassMap(obj.GetType());
            string sep = classMap.IdentitySeparator;
            //			bool gotObjectStatus = false;
            //			ObjectStatus objStatus = ObjectStatus.Clean;
            if (sep == "")
            {
                sep = "|";
            }
            object value;
            foreach (IPropertyMap propertyMap in classMap.GetIdentityPropertyMaps())
            {
                if (propertyMap == newPropertyMap)
                {
                    value = newValue;
                    if (propertyMap.ReferenceType != ReferenceType.None)
                    {
                        value = m_ObjectManager.GetObjectIdentity(value);
                    }
                }
                else
                {
                    value = m_ObjectManager.GetPropertyValue(obj, propertyMap.Name);
                    if (value == null || m_ObjectManager.GetNullValueStatus(obj, propertyMap.Name) == true)
                    {
                        if (!(m_hashTempIds.ContainsKey(obj)))
                        {
                            m_hashTempIds[obj] = Guid.NewGuid().ToString();
                        }
                        return (string)m_hashTempIds[obj];
                    }
                    else if (propertyMap.ReferenceType != ReferenceType.None)
                    {
                        //this ensures that a complete id can be created ahead in case of auto-in
                        //m_ObjectManager.GetPropertyValue(obj, propertyMap.Name);
                        value = m_ObjectManager.GetObjectIdentity(value);
                    }
                }

                id += Convert.ToString(value) + sep;
            }
            if (id.Length > sep.Length)
            {
                id = id.Substring(0, id.Length - sep.Length);
            }
            return id;
        }

        public virtual string GetObjectIdentity(object obj, IPropertyMap newPropertyMap, object newValue)
        {
            //to be implemented: idhelper
            
            //IIdentityHelper idObj = (IIdentityHelper)obj;
            //if (idObj.IsInvalid)
            //{
            //    string id = BuildObjectIdentity(obj, newPropertyMap, newValue);
            //    idObj.SetIdentity(id);
            //    idObj.IsInvalid = false;
            //}

            //return idObj.GetIdentity();           
            string id = BuildObjectIdentity(obj, newPropertyMap, newValue);
            return id;
        }

        public virtual string GetObjectKeyOrIdentity(object obj)
        {
            IClassMap classMap = m_ObjectManager.Context.DomainMap.MustGetClassMap(obj.GetType());
            string key = "";
            if (classMap.GetKeyPropertyMaps().Count > 0)
            {
                key = GetObjectKey(obj);
            }
            if (key.Length < 1)
            {
                key = GetObjectIdentity(obj);
            }
            return key;
        }


        public virtual string GetObjectKey(object obj)
        {
            string key = "";
            IClassMap classMap = m_ObjectManager.Context.DomainMap.MustGetClassMap(obj.GetType());
            string sep = classMap.KeySeparator;
            if (sep == "")
            {
                sep = " ";
            }
            object value;
            foreach (IPropertyMap propertyMap in classMap.GetKeyPropertyMaps())
            {
                value = m_ObjectManager.GetPropertyValue(obj, propertyMap.Name);
                if (value == null || m_ObjectManager.GetNullValueStatus(obj, propertyMap.Name))
                {
                    return "";
                }
                if (!(propertyMap.ReferenceType == ReferenceType.None))
                {
                    value = m_ObjectManager.GetObjectKey(value);
                    if (((string)value).Length < 1)
                        return "";
                }
                key += Convert.ToString(value) + sep;
            }
            if (key.Length > sep.Length)
            {
                key = key.Substring(0, key.Length - sep.Length);
            }
            return key;
        }


        public virtual void SetObjectIdentity(object obj, string identity)
        {
            IClassMap classMap = m_ObjectManager.Context.DomainMap.MustGetClassMap(obj.GetType());
            string sep = classMap.IdentitySeparator;
            if (sep == "")
            {
                sep = "|";
            }
            string[] arrId = identity.Split(sep.ToCharArray());
            long i = 0;
            Type refType;
            object refObj;
            object val;
            foreach (IPropertyMap propertyMap in classMap.GetIdentityPropertyMaps())
            {
                if (propertyMap.ReferenceType != ReferenceType.None)
                {
                    refType = obj.GetType().GetProperty(propertyMap.Name).PropertyType;
                    refObj = m_ObjectManager.Context.GetObjectById(Convert.ToString(arrId[i]), refType, true);
                    m_ObjectManager.SetPropertyValue(obj, propertyMap.Name, refObj);
                    m_ObjectManager.SetOriginalPropertyValue(obj, propertyMap.Name, refObj);
                    m_ObjectManager.SetNullValueStatus(obj, propertyMap.Name, false);
                }
                else
                {
                    val = ConvertValueToType(obj, propertyMap, arrId[i]);
                    m_ObjectManager.SetPropertyValue(obj, propertyMap.Name, val);
                    m_ObjectManager.SetOriginalPropertyValue(obj, propertyMap.Name, val);
                    m_ObjectManager.SetNullValueStatus(obj, propertyMap.Name, false);
                }
                i += 1;
            }
        }

        //[DebuggerStepThrough()]
        public virtual object GetPropertyValue(object obj, string propertyName)
        {
            return GetPropertyValue(obj, obj.GetType(), propertyName);
        }

        //[DebuggerStepThrough()]
        public virtual object GetPropertyValue(object obj, Type type, string propertyName)
        {
       //     IPropertyMap propertyMap = m_ObjectManager.Context.DomainMap.MustGetClassMap(obj.GetType()).MustGetPropertyMap(propertyName);

       //     FieldInfo fieldInfo = ReflectionHelper.GetFieldInfo(propertyMap, type, propertyName);

            FieldInfo fieldInfo = (FieldInfo)propertyLookup[obj.GetType().Name +"."+ propertyName];
            if (fieldInfo == null)
            {
                fieldInfo = GetFieldInfo(obj, propertyName);
            }

            return fieldInfo.GetValue(obj);

        }

        public virtual void SetPropertyValue(object obj, string propertyName, object value)
        {
            SetPropertyValue(obj, obj.GetType(), propertyName, value);
        }

        private Hashtable propertyLookup = new Hashtable();
        private FieldInfo GetFieldInfo(object obj, string propertyName)
        {
            IPropertyMap propertyMap = m_ObjectManager.Context.DomainMap.MustGetClassMap(obj.GetType()).MustGetPropertyMap(propertyName);
            FieldInfo fieldInfo = ReflectionHelper.GetFieldInfo(propertyMap, obj.GetType (), propertyName);
            if (fieldInfo == null)
                throw new MappingException("Could not find a field with the name '" + propertyMap.GetFieldName() + "' in class " + obj.GetType().Name);
            propertyLookup[obj.GetType().Name + "." + propertyName] = fieldInfo;
            return fieldInfo;
        }

        
        public virtual void SetPropertyValue(object obj, Type type, string propertyName, object value)
        {
            FieldInfo fieldInfo = (FieldInfo)propertyLookup[obj.GetType().Name + "." + propertyName];
            if (fieldInfo == null)
            {
                fieldInfo = GetFieldInfo(obj, propertyName);
            }
            if (fieldInfo.FieldType.IsEnum)
            {
                if (value != null)
                {
                    fieldInfo.SetValue(obj, Enum.ToObject(fieldInfo.FieldType, value));
                    return;
                }
            }
            fieldInfo.SetValue(obj, value);           
        }

        public virtual PropertyStatus GetPropertyStatus(object obj, string propertyName)
        {
            ObjectStatus objStatus = m_ObjectManager.GetObjectStatus(obj);
            if (objStatus == ObjectStatus.UpForCreation)
            {
                return PropertyStatus.Dirty;

            }
            if (objStatus == ObjectStatus.Deleted)
            {
                return PropertyStatus.Deleted;
            }
            if (m_ObjectManager.IsDirtyProperty(obj, propertyName))
            {
                return PropertyStatus.Dirty;
            }
            if (m_ObjectManager.HasOriginalValues(obj, propertyName))
            {
                return PropertyStatus.Clean;
            }
            return PropertyStatus.NotLoaded;
        }

        public virtual bool IsDirtyProperty(object obj, string propertyName)
        {
            if (!(m_ObjectManager.HasOriginalValues(obj, propertyName)))
            {
                return false;
            }
            object orgValue = m_ObjectManager.GetOriginalPropertyValue(obj, propertyName);
            if (Convert.IsDBNull(orgValue))
            {
                if (!(m_ObjectManager.GetNullValueStatus(obj, propertyName)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (m_ObjectManager.GetNullValueStatus(obj, propertyName))
                {
                    return true;
                }
            }
            //			if (obj is IProxy)
            //			{
            if (this.ObjectManager.GetUpdatedStatus(obj, propertyName))
            {
                object value = m_ObjectManager.GetPropertyValue(obj, propertyName);
                if (!(ComparePropertyValues(obj, propertyName, value, orgValue)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            //			}
            //			else
            //			{
            //				object value = m_ObjectManager.GetPropertyValue(obj, propertyName);
            //				if (!(ComparePropertyValues(obj, propertyName, value, orgValue)))
            //				{
            //					return true;
            //				}
            //				else
            //				{
            //					return false;
            //				}								
            //			}
        }

        public virtual bool ComparePropertyValues(object obj, string propertyName, object value1, object value2)
        {
            IPropertyMap propertyMap = m_ObjectManager.Context.DomainMap.MustGetClassMap(obj.GetType()).MustGetPropertyMap(propertyName);
            Array arr1;
            Array arr2;
            if (propertyMap.IsCollection)
            {
                return this.m_ObjectManager.Context.ListManager.CompareLists((IList)value1, (IList)value2);
            }
            else
            {
                if (!(propertyMap.ReferenceType == ReferenceType.None))
                {
                    if (value1 == value2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (Util.IsArray(value1) || Util.IsArray(value2))
                    {
                        if (!((Util.IsArray(value1) && Util.IsArray(value2))))
                        {
                            return false;
                        }
                    }
                    if (Util.IsArray(value1) && Util.IsArray(value2))
                    {
                        if (((Array)(value1)).Length == ((Array)(value2)).Length)
                        {
                            arr1 = ((Array)(value1));
                            arr2 = ((Array)(value2));
                            if (!(CompareArrays(arr1, arr2)))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        string lowTypeName = propertyMap.DataType.ToLower(CultureInfo.InvariantCulture);
                        if (lowTypeName == "guid" || lowTypeName == "system.guid")
                        {
                            if (((Guid)(value1)).Equals(value2))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (value1 == null || value2 == null)
                            {
                                if (value1 == null && value2 == null)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (value1.Equals(value2))
                                {
                                    return true;
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
            return true;
        }

        protected virtual bool CompareArrays(Array arr1, Array arr2)
        {
            object value1;
            object value2;
            for (int i = 0; i <= arr1.GetUpperBound(0); i++)
            {
                value1 = arr1.GetValue(i);
                value2 = arr2.GetValue(i);
                if (value1 == null || value2 == null)
                {
                    if (!(value1 == null && value2 == null))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!(value1.Equals(value2)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        protected virtual object ConvertValueToType(object obj, IPropertyMap propertyMap, string value)
        {
            Type propType = obj.GetType().GetProperty(propertyMap.Name).PropertyType;
            if (propType == typeof(Boolean) || propType.IsSubclassOf(typeof(Boolean)))
            {
                return Convert.ToBoolean(value);
            }
            else if (propType == typeof(Byte) || propType.IsSubclassOf(typeof(Byte)))
            {
                return Convert.ToByte(value);
            }
            else if (propType == typeof(Char) || propType.IsSubclassOf(typeof(Char)))
            {
                return Convert.ToChar(value);
            }
            else if (propType == typeof(DateTime) || propType.IsSubclassOf(typeof(DateTime)))
            {
                return Convert.ToDateTime(value);
            }
            else if (propType == typeof(Decimal) || propType.IsSubclassOf(typeof(Decimal)))
            {
                return Convert.ToDecimal(value);
            }
            else if (propType == typeof(Double) || propType.IsSubclassOf(typeof(Double)))
            {
                return Convert.ToDouble(value);
            }
            else if (propType == typeof(Guid) || propType.IsSubclassOf(typeof(Guid)))
            {
                return new Guid(value);
            }
            else if (propType == typeof(Int16) || propType.IsSubclassOf(typeof(Int16)))
            {
                return Convert.ToInt16(value);
            }
            else if (propType == typeof(Int32) || propType.IsSubclassOf(typeof(Int32)))
            {
                return Convert.ToInt32(value);
            }
            else if (propType == typeof(Int64) || propType.IsSubclassOf(typeof(Int64)))
            {
                return Convert.ToInt64(value);
            }
            else if (propType == typeof(SByte) || propType.IsSubclassOf(typeof(SByte)))
            {
                return Convert.ToByte(value);
            }
            else if (propType == typeof(Single) || propType.IsSubclassOf(typeof(Single)))
            {
                return Convert.ToSingle(value);
            }
            else if (propType == typeof(String) || propType.IsSubclassOf(typeof(String)))
            {
                return Convert.ToString(value);
            }
            else if (propType == typeof(ushort) || propType.IsSubclassOf(typeof(ushort)))
            {
                return UInt16.Parse(value);
            }
            else if (propType == typeof(uint) || propType.IsSubclassOf(typeof(uint)))
            {
                return UInt32.Parse(value);
            }
            else if (propType == typeof(ulong) || propType.IsSubclassOf(typeof(ulong)))
            {
                return UInt64.Parse(value);
            }
            else if (propType == typeof(object) || propType.IsSubclassOf(typeof(object)))
            {
                return value;
            }
            else
            {
                return value;
            }
        }

    }
}
