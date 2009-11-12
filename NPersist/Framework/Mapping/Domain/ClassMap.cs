// *
// * Copyright (C) 2005 Mats Helander : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Mapping.Visitor;

namespace Puzzle.NPersist.Framework.Mapping
{
	public class ClassMap : MapBase, IClassMap
	{

		public override void Accept(IMapVisitor visitor)
		{
			visitor.Visit(this);
		}

		
		public override IMap GetParent()
		{
			return m_DomainMap;
		}

		#region Private Member Variables

		private ArrayList m_PropertyMaps = new ArrayList();
		private ArrayList m_CodeMaps = new ArrayList();
		private ArrayList m_EnumValueMaps = new ArrayList();
		private DomainMap m_DomainMap;
		private string m_name = "";
		private string m_Source = "";
		private string m_Table = "";
		private string m_IdentitySeparator = "";
		private string m_KeySeparator = "";
		private string m_TypeColumn = "";
		private string m_TypeValue = "";
		private string m_InheritsClass = "";
		private ClassType m_ClassType = ClassType.Default;
		private InheritanceType m_InheritanceType = InheritanceType.None;
		private MergeBehaviorType m_MergeBehavior = MergeBehaviorType.DefaultBehavior;
		private RefreshBehaviorType m_RefreshBehavior = RefreshBehaviorType.DefaultBehavior;
		private bool m_IsAbstract = false;
		private bool m_IsReadOnly = false;
		private string m_InheritsTransientClass = "";
		private ArrayList m_ImplementsInterfaces = new ArrayList();
		private ArrayList m_ImportsNamespaces = new ArrayList();
		private OptimisticConcurrencyBehaviorType m_UpdateOptimisticConcurrencyBehavior = OptimisticConcurrencyBehaviorType.DefaultBehavior;
		private OptimisticConcurrencyBehaviorType m_DeleteOptimisticConcurrencyBehavior = OptimisticConcurrencyBehaviorType.DefaultBehavior;
		private string m_AssemblyName = "";
		private string m_ValidateMethod = "";
		private ValidationMode m_ValidationMode = ValidationMode.Default ;
		private string m_LoadSpan = "";
		private long m_TimeToLive = -1;
		private TimeToLiveBehavior m_TimeToLiveBehavior = TimeToLiveBehavior.Default;
		private LoadBehavior m_LoadBehavior = LoadBehavior.Default;

		//O/O Mapping
		private string m_SourceClass = "";

		//O/D Mapping
		private string m_DocSource = "";
		private string m_DocElement = "";
		private string m_DocRoot = "";
		private DocClassMapMode m_DocClassMapMode = DocClassMapMode.Default;
		private string m_DocParentProperty = "";

		#endregion

		#region Constructors

		public ClassMap() : base()
		{
		}

		public ClassMap(string name) : base()
		{
			m_name = name;
		}

		#endregion

		#region Object/Relational Mapping

		[XmlIgnore()]
		public virtual IDomainMap DomainMap
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get { return m_DomainMap; }
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				if (m_DomainMap != null)
				{
					m_DomainMap.ClassMaps.Remove(this);
				}
				m_DomainMap = (DomainMap) value;
				if (m_DomainMap != null)
				{
					m_DomainMap.ClassMaps.Add(this);
				}
			}
		}

		public virtual void SetDomainMap(IDomainMap value)
		{
			m_DomainMap = (DomainMap) value;
			foreach (IPropertyMap propertyMap in m_PropertyMaps)
			{
				propertyMap.SetClassMap(this);
			}
		}

		[XmlArrayItem(typeof (PropertyMap))]
		public virtual ArrayList PropertyMaps
		{
			get { return m_PropertyMaps; }
			set { m_PropertyMaps = value; }
		}

				
		public virtual IPropertyMap MustGetPropertyMap(string findName)
		{
			IPropertyMap propertyMap = GetPropertyMap(findName);

			if (propertyMap == null)
				throw new MappingException("Could not find property " + findName + " for the type " + this.GetFullName() + " in map file!");

			return propertyMap;
		}


		//Mats : Added case insensitivity
		//[DebuggerStepThrough()]
		public virtual IPropertyMap GetPropertyMap(string findName)
		{
			if (findName == null) { return null; }
			if (findName == "") { return null; }
			findName = findName.ToLower(CultureInfo.InvariantCulture);
			if (IsFixed("GetPropertyMap_" + findName))
			{
				return (IPropertyMap) GetFixedValue("GetPropertyMap_" + findName);
			}
			foreach (IPropertyMap propertyMap in this.GetAllPropertyMaps())
			{
				if (propertyMap.Name.ToLower(CultureInfo.InvariantCulture) == findName)
				{
					if (IsFixed())
					{
						SetFixedValue("GetPropertyMap_" + findName, propertyMap);
					}
					return propertyMap;
				}
			}
			return null;
		}

		[XmlArrayItem(typeof (CodeMap))]
		public virtual ArrayList CodeMaps
		{
			get { return m_CodeMaps; }
			set { m_CodeMaps = value; }
		}

		public virtual ICodeMap GetCodeMap(CodeLanguage codeLanguage)
		{
			if (IsFixed("GetCodeMap_" + codeLanguage.ToString() ))
			{
				return (ICodeMap) GetFixedValue("GetCodeMap_" + codeLanguage.ToString());
			}
			foreach (ICodeMap codeMap in this.m_CodeMaps)
			{
				if (codeMap.CodeLanguage.Equals(codeLanguage))
				{
					if (IsFixed())
					{
						SetFixedValue("GetCodeMap_" + codeLanguage.ToString(), codeMap);
					}
					return codeMap;
				}
			}
			return null;
		}

		public virtual ICodeMap EnsuredGetCodeMap(CodeLanguage codeLanguage)
		{
			ICodeMap codeMap = GetCodeMap(codeLanguage);
			if (codeMap == null)
			{
				codeMap = new CodeMap() ;
				codeMap.CodeLanguage = codeLanguage;
				this.m_CodeMaps.Add(codeMap);
			}				
			return codeMap;	
		}

				
		[XmlArrayItem(typeof (EnumValueMap))]
		public virtual ArrayList EnumValueMaps
		{
			get { return m_EnumValueMaps; }
			set { m_EnumValueMaps = value; }
		}

		//[DebuggerStepThrough()]
		public virtual IEnumValueMap GetEnumValueMap(string findName)
		{
			if (findName == null) { return null; }
			if (findName == "") { return null; }
			findName = findName.ToLower(CultureInfo.InvariantCulture);
			if (IsFixed("GetEnumValueMap_" + findName))
			{
				return (IEnumValueMap) GetFixedValue("GetEnumValueMap_" + findName);
			}
			foreach (IEnumValueMap enumValueMap in this.EnumValueMaps)
			{
				if (enumValueMap.Name.ToLower(CultureInfo.InvariantCulture) == findName)
				{
					if (IsFixed())
					{
						SetFixedValue("GetEnumValueMap_" + findName, enumValueMap);
					}
					return enumValueMap;
				}
			}
			return null;
		}

		public virtual IList GetEnumValueMaps()
		{
			bool sortingFailed = false;
			return GetSortedEnumValueMaps(ref sortingFailed);
		}

		public virtual IList GetSortedEnumValueMaps(ref bool failedSorting)
		{
			IList arrVals = new ArrayList();
			foreach (IEnumValueMap enumValueMap in m_EnumValueMaps)
			{
				arrVals.Add(enumValueMap);
			}
			object[] arr = new object[arrVals.Count];
			foreach (IEnumValueMap enumValueMap in arrVals)
			{
				if (enumValueMap.Index > arr.GetUpperBound(0))
				{
					failedSorting = true;
					return arrVals;
				}
				if (!(arr[enumValueMap.Index] == null))
				{
					failedSorting = true;
					return arrVals;
				}
				arr[enumValueMap.Index] = enumValueMap;
			}
			arrVals.Clear();
			foreach (IEnumValueMap enumValueMap in arr)
			{
				arrVals.Add(enumValueMap);
			}
			return arrVals;
		}



		public override string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		public virtual string Source
		{
			get
			{
				IClassMap superClassMap;
				if (m_InheritsClass.Length > 0)
				{
					superClassMap = GetInheritedClassMap();
					if (superClassMap != null)
					{
						return superClassMap.Source;
					}
				}
				return m_Source;
			}
			set { m_Source = value; }
		}

		public virtual ISourceMap GetSourceMap()
		{
			if (Source == "")
			{
				return m_DomainMap.GetSourceMap();
			}
			else
			{
				return m_DomainMap.GetSourceMap(Source);
			}
		}

		public virtual void SetSourceMap(ISourceMap value)
		{
			m_Source = value.Name;
		}

		public virtual string Table
		{
			get
			{
				IClassMap superClassMap;
				if (m_InheritsClass.Length > 0)
				{
					superClassMap = GetInheritedClassMap();
					if (superClassMap != null)
					{
						return superClassMap.Table;
					}
				}
				return m_Table;
			}
			set { m_Table = value; }
		}

						
		public virtual ITableMap MustGetTableMap()
		{
			ITableMap tableMap = GetTableMap();

			if (tableMap == null)
				throw new MappingException("Could not find table " + m_Table + " mapped to by type " + this.GetFullName() + " in map file!");

			return tableMap;
		}

		public virtual ITableMap GetTableMap()
		{
			ISourceMap sourceMap = GetSourceMap();
			if (sourceMap == null)
			{
				return null;
			}
			return sourceMap.GetTableMap(Table);
		}

		public virtual void SetTableMap(ITableMap value)
		{
			m_Table = value.Name;
		}

		public virtual ArrayList GetPrimaryPropertyMaps()
		{
			ArrayList arrProps = new ArrayList();
			ITableMap tableMap = null;
			foreach (IPropertyMap propertyMap in GetIdentityPropertyMaps())
			{
				arrProps.Add(propertyMap);
			}
			foreach (IPropertyMap propertyMap in m_PropertyMaps)
			{
				if (!(propertyMap.IsIdentity))
				{
					tableMap = propertyMap.GetTableMap();
					if (tableMap != null)
					{
						if (tableMap == GetTableMap())
						{
							arrProps.Add(propertyMap);
						}
					}
				}
			}
			return arrProps;
		}

		public virtual ArrayList GetIdentityPropertyMaps()
		{
			bool sortingFailed = false;
			return GetSortedIdentityPropertyMaps(ref sortingFailed, true);
		}

		public virtual ArrayList GetSortedIdentityPropertyMaps(ref bool failedSorting, bool getInherited)
		{
			ArrayList arrProps = new ArrayList();
			IClassMap superClassMap = GetInheritedClassMap();
			if (getInherited)
			{
				if (superClassMap != null)
				{
					foreach (IPropertyMap propertyMap in superClassMap.GetIdentityPropertyMaps())
					{
						if (propertyMap.IsIdentity)
						{
							arrProps.Add(propertyMap);
						}
					}
				}
			}
			foreach (IPropertyMap propertyMap in m_PropertyMaps)
			{
				if (propertyMap.IsIdentity)
				{
					arrProps.Add(propertyMap);
				}
			}
			object[] arr = new object[arrProps.Count];
			foreach (IPropertyMap propertyMap in arrProps)
			{
				if (propertyMap.IdentityIndex > arr.GetUpperBound(0))
				{
					failedSorting = true;
					return arrProps;
				}
				if (!(arr[propertyMap.IdentityIndex] == null))
				{
					failedSorting = true;
					return arrProps;
				}
				arr[propertyMap.IdentityIndex] = propertyMap;
			}
			arrProps.Clear();
			foreach (IPropertyMap propertyMap in arr)
			{
				arrProps.Add(propertyMap);
			}
			return arrProps;
		}

		public virtual ArrayList GetKeyPropertyMaps()
		{
			bool sortingFailed = false;
			return GetSortedKeyPropertyMaps(ref sortingFailed, true);
		}

		public virtual ArrayList GetSortedKeyPropertyMaps(ref bool failedSorting, bool getInherited)
		{
			ArrayList arrProps = new ArrayList();
			IClassMap superClassMap = GetInheritedClassMap();
			if (getInherited)
			{
				if (superClassMap != null)
				{
					foreach (IPropertyMap propertyMap in superClassMap.GetKeyPropertyMaps())
					{
						if (propertyMap.IsKey)
						{
							arrProps.Add(propertyMap);
						}
					}
				}
			}
			foreach (IPropertyMap propertyMap in m_PropertyMaps)
			{
				if (propertyMap.IsKey)
				{
					arrProps.Add(propertyMap);
				}
			}
			object[] arr = new object[arrProps.Count];
			foreach (IPropertyMap propertyMap in arrProps)
			{
				if (propertyMap.KeyIndex > arr.GetUpperBound(0))
				{
					failedSorting = true;
					return arrProps;
				}
				if (!(arr[propertyMap.KeyIndex] == null))
				{
					failedSorting = true;
					return arrProps;
				}
				arr[propertyMap.KeyIndex] = propertyMap;
			}
			arrProps.Clear();
			foreach (IPropertyMap propertyMap in arr)
			{
				arrProps.Add(propertyMap);
			}
			return arrProps;
		}


		public virtual bool HasAssignedBySource()
		{
			foreach (IPropertyMap propertyMap in GetAllPropertyMaps())
			{
				if (propertyMap.GetIsAssignedBySource())
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool HasGuid()
		{
			IColumnMap columnMap;
			foreach (IPropertyMap propertyMap in GetAllPropertyMaps())
			{
				columnMap = propertyMap.GetColumnMap();
				if (columnMap != null)
				{
					if (columnMap.DataType == DbType.Guid)
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual bool HasIdAssignedBySource()
		{
			foreach (IPropertyMap propertyMap in GetIdentityPropertyMaps())
			{
				if (propertyMap.GetIsAssignedBySource())
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool HasIdGuid()
		{
			IColumnMap columnMap;
			foreach (IPropertyMap propertyMap in GetIdentityPropertyMaps())
			{
				columnMap = propertyMap.GetColumnMap();
				if (columnMap != null)
				{
					if (columnMap.DataType == DbType.Guid)
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual bool HasSingleIdAutoIncreaser()
		{
			IColumnMap columnMap;
			foreach (IPropertyMap propertyMap in GetIdentityPropertyMaps())
			{
				columnMap = propertyMap.GetColumnMap();
				if (columnMap != null)
				{
					if (columnMap.IsAutoIncrease)
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual IPropertyMap GetAutoIncreasingIdentityPropertyMap()
		{
			IColumnMap columnMap;
			foreach (IPropertyMap propertyMap in GetIdentityPropertyMaps())
			{
				columnMap = propertyMap.GetColumnMap();
				if (columnMap != null)
				{
					if (columnMap.IsAutoIncrease)
					{
						return propertyMap;
					}
				}
			}
			return null;
		}

		public virtual IPropertyMap GetAssignedBySourceIdentityPropertyMap()
		{
			foreach (IPropertyMap propertyMap in GetIdentityPropertyMaps())
			{
				if (propertyMap.GetIsAssignedBySource())
				{
					return propertyMap;
				}
			}
			return null;
		}

		public virtual string IdentitySeparator
		{
			get { return m_IdentitySeparator; }
			set { m_IdentitySeparator = value; }
		}

		public virtual string GetIdentitySeparator()
		{
			return InternalGetIdentitySeparator(new Hashtable());
		}

		private string InternalGetIdentitySeparator(Hashtable visited)
		{
			if (visited[this] != null)
				return "";
			visited[this] = this;

			IClassMap superClassMap = this.GetInheritedClassMap() ;
			
			if (superClassMap != null)
				return superClassMap.GetIdentitySeparator();
			return m_IdentitySeparator;			
		}

		public virtual string KeySeparator
		{
			get { return m_KeySeparator; }
			set { m_KeySeparator = value; }
		}

		public virtual string GetKeySeparator()
		{
			return InternalGetKeySeparator(new Hashtable());
		}

		private string InternalGetKeySeparator(Hashtable visited)
		{
			if (visited[this] != null)
				return "";
			visited[this] = this;

			IClassMap superClassMap = this.GetInheritedClassMap() ;
			
			if (superClassMap != null)
				return superClassMap.GetKeySeparator();
			return m_KeySeparator;			
		}

		public virtual string TypeValue
		{
			get { return m_TypeValue; }
			set { m_TypeValue = value; }
		}

		public virtual string TypeColumn
		{
			get { return m_TypeColumn; }
			set { m_TypeColumn = value; }
		}

		public virtual IColumnMap GetTypeColumnMap()
		{
			ITableMap tableMap = GetTableMap();
			if (tableMap != null)
			{
				return GetTableMap().GetColumnMap(m_TypeColumn);
			}
			return null;
		}

		public virtual void SetTypeColumnMap(IColumnMap value)
		{
			m_TypeColumn = value.Name;
		}

		public virtual MergeBehaviorType MergeBehavior
		{
			get { return m_MergeBehavior; }
			set { m_MergeBehavior = value; }
		}

		public virtual RefreshBehaviorType RefreshBehavior
		{
			get { return m_RefreshBehavior; }
			set { m_RefreshBehavior = value; }
		}

		public virtual bool IsAbstract
		{
			get { return m_IsAbstract; }
			set { m_IsAbstract = value; }
		}
		
		//[DebuggerStepThrough()]
		public virtual ArrayList GetAllPropertyMaps()
		{
			if (IsFixed("GetAllPropertyMaps"))
			{
				return (ArrayList) GetFixedValue("GetAllPropertyMaps");
			}
			ArrayList arrProps = new ArrayList();
			Hashtable hashAdded;
			IClassMap superClass;
			superClass = GetInheritedClassMap();
			if (superClass == null)
			{
				foreach (IPropertyMap propertyMap in GetIdentityPropertyMaps())
				{
					arrProps.Add(propertyMap);
				}
				foreach (IPropertyMap propertyMap in m_PropertyMaps)
				{
					if (propertyMap.Name.Length > 0)
					{
						if (!(propertyMap.IsIdentity))
						{
							arrProps.Add(propertyMap);
						}
					}
				}
			}
			else
			{
				hashAdded = new Hashtable();
				foreach (IPropertyMap propertyMap in GetIdentityPropertyMaps())
				{
					arrProps.Add(propertyMap);
					hashAdded[propertyMap.Name.ToLower(CultureInfo.InvariantCulture)] = true;
				}
				foreach (IPropertyMap propertyMap in m_PropertyMaps)
				{
					if (propertyMap.Name.Length > 0)
					{
						if (!(propertyMap.IsIdentity))
						{
							arrProps.Add(propertyMap);
							hashAdded[propertyMap.Name.ToLower(CultureInfo.InvariantCulture)] = true;
						}
					}
				}
				if (superClass != null)
				{
					foreach (IPropertyMap propertyMap in superClass.GetAllPropertyMaps())
					{
						if (!(hashAdded.ContainsKey(propertyMap.Name.ToLower(CultureInfo.InvariantCulture))))
						{
							arrProps.Add(propertyMap);
							hashAdded[propertyMap.Name.ToLower(CultureInfo.InvariantCulture)] = true;
						}
					}
				}
			}
			if (IsFixed())
			{
				SetFixedValue("GetAllPropertyMaps", arrProps);
			}
			return arrProps;
		}

		public virtual ArrayList GetInheritedPropertyMaps()
		{
			IClassMap superClass = GetInheritedClassMap();
			ArrayList propertyMaps = new ArrayList();
			if (superClass != null)
			{
				Hashtable hashProps = new Hashtable();
				foreach (IPropertyMap propertyMap in m_PropertyMaps)
				{
					hashProps[propertyMap.Name.ToLower(CultureInfo.InvariantCulture)] = true;
				}
				foreach (IPropertyMap propertyMap in superClass.GetAllPropertyMaps())
				{
					if (!(hashProps.ContainsKey(propertyMap.Name.ToLower(CultureInfo.InvariantCulture))))
					{
						propertyMaps.Add(propertyMap);
					}
				}
			}
			return propertyMaps;
		}

		public virtual ArrayList GetNonInheritedPropertyMaps()
		{
			ArrayList arrProps = new ArrayList();
			foreach (IPropertyMap propertyMap in GetNonInheritedIdentityPropertyMaps())
			{
				arrProps.Add(propertyMap);
			}
			foreach (IPropertyMap propertyMap in m_PropertyMaps)
			{
				if (!(propertyMap.IsIdentity))
				{
					arrProps.Add(propertyMap);
				}
			}
			return arrProps;
		}

		public virtual ArrayList GetNonInheritedIdentityPropertyMaps()
		{
			bool sortingFailed = false;
			return GetSortedIdentityPropertyMaps(ref sortingFailed, false);
		}

		public virtual ArrayList GetInheritedIdentityPropertyMaps()
		{
			IClassMap superClass = GetInheritedClassMap();
			if (superClass == null)
			{
				return new ArrayList();
			}
			return superClass.GetIdentityPropertyMaps();
		}

		public virtual IClassMap GetInheritedClassMap()
		{
			if (m_InheritsClass.Length < 1)
			{
				return null;
			}
			string ns;
			IClassMap classMap = DomainMap.GetClassMap(m_InheritsClass);
			if (classMap == null)
			{
				ns = GetNamespace();
				if (ns.Length > 0)
				{
					classMap = DomainMap.GetClassMap(ns + "." + m_InheritsClass);
				}
			}
			return classMap;
		}

		public virtual string InheritsClass
		{
			get { return m_InheritsClass; }
			set
			{
				if (DomainMap != null)
				{
					if (value.Length > 0)
					{
						string ns;
						IClassMap classMap = DomainMap.GetClassMap(value);
						if (classMap == null)
						{
							ns = GetNamespace();
							if (ns.Length > 0)
							{
								classMap = DomainMap.GetClassMap(ns + "." + value);
							}
						}
						if (classMap != null)
						{
							if (!(IsLegalAsSuperClass(classMap)))
							{
								throw new MappingException("Class '" + classMap.Name + "' is illegal as superclass for class '" + this.Name + "'! Would create cyclic inheritance hierarchy!"); // do not localize
							}
						}
					}
				}
				m_InheritsClass = value;
			}
		}

		public ClassType ClassType
		{
			get { return this.m_ClassType; }
			set { this.m_ClassType = value; }
		}

		public void UpdateName(string newName)
		{
			foreach (IClassMap classMap in DomainMap.ClassMaps)
			{
				if (classMap.GetInheritedClassMap() == this)
				{
					classMap.InheritsClass = newName;
				}
				foreach (IPropertyMap propertyMap in classMap.GetNonInheritedPropertyMaps())
				{
					if (!(propertyMap.ReferenceType == ReferenceType.None))
					{
						if (propertyMap.GetReferencedClassMap() == this)
						{
							if (propertyMap.IsCollection)
							{
								propertyMap.ItemType = newName;
							}
							else
							{
								propertyMap.DataType = newName;
							}
						}
					}
				}
			}
			m_name = newName;
		}

		public virtual string GetFullName()
		{
			if (m_name == "")
			{
				return "";
			}
			if (m_DomainMap.RootNamespace.Length > 0)
			{
				return m_DomainMap.RootNamespace + "." + m_name;
			}
			else
			{
				return m_name;
			}
		}

		public virtual string GetName()
		{
			if (m_name == "")
			{
				return "";
			}
			string[] arrName = m_name.Split('.');
			return arrName[arrName.GetUpperBound(0)];
		}

		public virtual string GetNamespace()
		{
			if (m_name == "")
			{
				return "";
			}
			string[] arrName = m_name.Split('.');
			if (arrName.GetUpperBound(0) < 1)
			{
				return "";
			}
			return m_name.Substring(0, m_name.Length - 1 - arrName[arrName.GetUpperBound(0)].Length);
		}

		public virtual string GetFullNamespace()
		{
			string rootNs = this.DomainMap.RootNamespace;
			string ns = this.GetNamespace();
			if (rootNs.Length > 0)
			{
				if (ns.Length > 0)
				{
					ns = rootNs + "." + ns;					
				}
				else
				{
					ns = rootNs;
				}
			}
			return ns;
		}

		public virtual InheritanceType InheritanceType
		{
			get { return m_InheritanceType; }
			set { m_InheritanceType = value; }
		}

		public virtual IPropertyMap MustGetPropertyMapForColumnMap(IColumnMap columnMap)
		{
			IPropertyMap propertyMap = GetPropertyMapForColumnMap(columnMap);

			if (propertyMap == null)
				throw new MappingException("Could not find any property mapping to column " + columnMap.TableMap.Name + "." + columnMap.Name + " in the type " + this.GetFullName() + " in map file!");

			return propertyMap;
		}

		public virtual IPropertyMap GetPropertyMapForColumnMap(IColumnMap columnMap)
		{
			IColumnMap testColumnMap;
			foreach (IPropertyMap propertyMap in this.GetAllPropertyMaps())
			{
				testColumnMap = propertyMap.GetColumnMap();
				if (testColumnMap != null)
				{
					if (testColumnMap == columnMap)
					{
						return propertyMap;
					}
				}
				testColumnMap = propertyMap.GetIdColumnMap();
				if (testColumnMap != null)
				{
					if (testColumnMap == columnMap)
					{
						return propertyMap;
					}
				}
				foreach (IColumnMap checkColumnMap in propertyMap.GetAdditionalColumnMaps())
				{
					if (checkColumnMap == columnMap)
					{
						return propertyMap;
					}
				}
				foreach (IColumnMap checkColumnMap in propertyMap.GetAdditionalIdColumnMaps())
				{
					if (checkColumnMap == columnMap)
					{
						return propertyMap;
					}
				}
			}
			return null;
		}

		public virtual ArrayList GetDirectSubClassMaps()
		{
			ArrayList classMaps = new ArrayList();
			IClassMap superClassMap;
			foreach (IClassMap classMap in m_DomainMap.ClassMaps)
			{
				if (!(classMap == this))
				{
					superClassMap = classMap.GetInheritedClassMap();
					if (superClassMap == null)
					{
						if (superClassMap == this)
						{
							classMaps.Add(classMap);
						}
					}
				}
			}
			return classMaps;
		}

		public virtual ArrayList GetSubClassMaps()
		{
			ArrayList classMaps = new ArrayList();
			foreach (IClassMap classMap in m_DomainMap.ClassMaps)
			{
				if (!(classMap == this))
				{
					if (IsSubClass(classMap))
					{
						classMaps.Add(classMap);
					}
				}
			}
			return classMaps;
		}

		//retrns true if this class equals any of the passed 
		//in class' superclasses. ("Is the passed in class a subclass
		//of this class)
		public virtual bool IsSubClass(IClassMap classMap)
		{
			IClassMap superClassMap = null;
			superClassMap = classMap.GetInheritedClassMap();
			while (superClassMap != null)
			{
				if (superClassMap == this)
				{
					return true;
				}
				superClassMap = superClassMap.GetInheritedClassMap();
			}
			return false;
		}

		public virtual bool IsSubClassOrThisClass(IClassMap classMap)
		{
			if (classMap == this)
			{
				return true;
			}
			return IsSubClass(classMap);
		}

		public virtual bool HasSubClasses()
		{
			foreach (IClassMap classMap in m_DomainMap.ClassMaps)
			{
				if (!(classMap == this))
				{
					if (IsSubClass(classMap))
					{
						return true;
					}
				}
			}
			return false;
		}

		public virtual IClassMap GetSubClassWithTypeValue(string value)
		{
			if (TypeValue == value)
			{
				return this;
			}
			foreach (IClassMap classMap in GetSubClassMaps())
			{
				if (classMap.TypeValue == value)
				{
					return classMap;
				}
			}
			return null;
		}

		public virtual IClassMap GetBaseClassMap()
		{
			IClassMap classMap = this;
			IClassMap baseClassMap = classMap.GetInheritedClassMap();
			while (baseClassMap != null)
			{
				classMap = baseClassMap;
				baseClassMap = classMap.GetInheritedClassMap();
			}
			return classMap;
		}

		public virtual bool IsInHierarchy()
		{
			if (!(this.GetInheritedClassMap() == null))
			{
				return true;
			}
			if (this.HasSubClasses())
			{
				return true;
			}
			return false;
		}

		public bool IsInheritedProperty(IPropertyMap propertyMap)
		{
			IClassMap superClass = GetInheritedClassMap();
			if (superClass != null)
			{
				foreach (IPropertyMap testPropertyMap in superClass.GetAllPropertyMaps())
				{
					if (testPropertyMap.Name == propertyMap.Name)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsShadowingProperty(IPropertyMap propertyMap)
		{
			IClassMap superClass = GetInheritedClassMap();
			if (superClass != null)
			{
				foreach (IPropertyMap testPropertyMap in superClass.GetAllPropertyMaps())
				{
					if (testPropertyMap.Name == propertyMap.Name)
					{
						foreach (IPropertyMap testPropertyMap2 in m_PropertyMaps)
						{
							if (testPropertyMap2.Name == propertyMap.Name)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}


		public virtual bool IsReadOnly
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				if (DomainMap.IsReadOnly)
				{
					return true;
				}
				return m_IsReadOnly;
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set { m_IsReadOnly = value; }
		}

		public virtual string InheritsTransientClass
		{
			get { return m_InheritsTransientClass; }
			set { m_InheritsTransientClass = value; }
		}

		[XmlArrayItem(typeof (string))]
		public virtual ArrayList ImplementsInterfaces
		{
			get { return m_ImplementsInterfaces; }
			set { m_ImplementsInterfaces = value; }
		}

		[XmlArrayItem(typeof (string))]
		public virtual ArrayList ImportsNamespaces
		{
			get { return m_ImportsNamespaces; }
			set { m_ImportsNamespaces = value; }
		}

		public virtual OptimisticConcurrencyBehaviorType UpdateOptimisticConcurrencyBehavior
		{
			get { return m_UpdateOptimisticConcurrencyBehavior; }
			set { m_UpdateOptimisticConcurrencyBehavior = value; }
		}

		public virtual OptimisticConcurrencyBehaviorType DeleteOptimisticConcurrencyBehavior
		{
			get { return m_DeleteOptimisticConcurrencyBehavior; }
			set { m_DeleteOptimisticConcurrencyBehavior = value; }
		}

		public virtual string AssemblyName
		{
			get { return m_AssemblyName; }
			set { m_AssemblyName = value; }
		}

				
		public ValidationMode ValidationMode
		{
			get { return this.m_ValidationMode; }
			set { this.m_ValidationMode = value; }
		}

		public virtual string ValidateMethod
		{
			get { return m_ValidateMethod; }
			set { m_ValidateMethod = value; }
		}

		public virtual string LoadSpan
		{
			get { return m_LoadSpan; }
			set { m_LoadSpan = value; }
		}

		public long TimeToLive
		{
			get { return this.m_TimeToLive; }
			set { this.m_TimeToLive = value; }
		}
		
		public TimeToLiveBehavior TimeToLiveBehavior
		{
			get { return this.m_TimeToLiveBehavior; }
			set { this.m_TimeToLiveBehavior = value; }
		}
		
		public long GetTimeToLive()
		{
			if (this.m_TimeToLive < 0)
				return this.DomainMap.TimeToLive;
			return this.m_TimeToLive;
		}
		
		public TimeToLiveBehavior GetTimeToLiveBehavior()
		{
			if (this.m_TimeToLiveBehavior == TimeToLiveBehavior.Default)
				return this.DomainMap.TimeToLiveBehavior;
			return this.m_TimeToLiveBehavior;
		}

		public LoadBehavior LoadBehavior
		{
			get { return this.m_LoadBehavior; }
			set { this.m_LoadBehavior = value; }
		}

		public LoadBehavior GetLoadBehavior()
		{
			if (this.m_LoadBehavior == LoadBehavior.Default)
				return this.DomainMap.LoadBehavior;
			return this.m_LoadBehavior;
		}


		public virtual string GetAssemblyName()
		{
			if (m_AssemblyName.Length > 0)
			{
				return m_AssemblyName;
			}
			else
			{
				return this.DomainMap.GetAssemblyName();
			}
		}

		
		
		public virtual string GetInheritsTransientClass()
		{
			if (m_InheritsTransientClass.Length > 0)
			{
				return m_InheritsTransientClass;
			}
			else
			{
				if (m_DomainMap != null)
				{
					return m_DomainMap.InheritsTransientClass;
				}
			}
			return "";
		}

		public virtual ArrayList GetImplementsInterfaces()
		{
			ArrayList returnList = (ArrayList) m_DomainMap.ImplementsInterfaces.Clone();
			Hashtable added = new Hashtable();
			foreach (string interfaceName in returnList)
			{
				if (!(added.ContainsKey(interfaceName.ToLower(CultureInfo.InvariantCulture))))
				{
					added[interfaceName.ToLower(CultureInfo.InvariantCulture)] = true;
				}
			}
			foreach (string interfaceName in m_ImplementsInterfaces)
			{
				if (!(added.ContainsKey(interfaceName.ToLower(CultureInfo.InvariantCulture))))
				{
					returnList.Add(interfaceName);
				}
			}
			return returnList;
		}

		public virtual ArrayList GetImportsNamespaces()
		{
			ArrayList returnList = (ArrayList) m_DomainMap.ImportsNamespaces.Clone();
			Hashtable added = new Hashtable();
			foreach (string namespaceName in returnList)
			{
				if (!(added.ContainsKey(namespaceName.ToLower(CultureInfo.InvariantCulture))))
				{
					added[namespaceName.ToLower(CultureInfo.InvariantCulture)] = true;
				}
			}
			foreach (string namespaceName in m_ImportsNamespaces)
			{
				if (!(added.ContainsKey(namespaceName.ToLower(CultureInfo.InvariantCulture))))
				{
					returnList.Add(namespaceName);
				}
			}
			return returnList;
		}

		#endregion

		#region Object/Object Mapping

		public virtual string SourceClass
		{
			get { return m_SourceClass; }
			set { m_SourceClass = value; }
		}

		public virtual IClassMap GetSourceClassMap()
		{
			if (m_SourceClass == "")
				return null;

			return this.DomainMap.GetClassMap(m_SourceClass);
		}

		public virtual IClassMap GetSourceClassMapOrSelf()
		{
			if (m_SourceClass == "")
				return this;

			IClassMap sourceClassMap = this.GetSourceClassMap();

			if (sourceClassMap == null)
				sourceClassMap = this;

			return sourceClassMap;
		}

		#endregion

		#region Object/Document Mapping

		public virtual string DocSource
		{
			get { return m_DocSource; }
			set { m_DocSource = value; }
		}

		public virtual string DocElement
		{
			get { return m_DocElement; }
			set { m_DocElement = value; }
		}
		
		public virtual string  GetDocElement()
		{
			if (m_DocElement.Length > 0)
				return m_DocElement;

			return this.GetName();
		}
		
		public virtual string DocRoot
		{
			get { return m_DocRoot; }
			set { m_DocRoot = value; }
		}
				
		public virtual string GetDocRoot()
		{
			if (m_DocRoot.Length > 0)
				return m_DocRoot;

			return this.GetName();
		}

		public virtual ISourceMap GetDocSourceMap()
		{
			if (DocSource == "")
			{
				return m_DomainMap.GetDocSourceMap();
			}
			else
			{
				return m_DomainMap.GetSourceMap(DocSource);
			}
		}

		public virtual void SetDocSourceMap(ISourceMap value)
		{
			m_DocSource = value.Name;
		}
		
		public virtual DocClassMapMode DocClassMapMode
		{
			get { return m_DocClassMapMode; }
			set { m_DocClassMapMode = value; }
		}

		
		public virtual string DocParentProperty
		{
			get { return m_DocParentProperty; }
			set { m_DocParentProperty = value; }
		}

				
		public virtual IPropertyMap GetDocParentPropertyMap()
		{
			if (m_DocParentProperty.Length < 1)
				return null;
			
			return MustGetPropertyMap(m_DocParentProperty);
		}

		#endregion

		#region Cloning

		public override IMap Clone()
		{
			IClassMap classMap = new ClassMap();
			Copy(classMap);
			return classMap;
		}

		public override IMap DeepClone()
		{
			IClassMap classMap = new ClassMap();
			DeepCopy(classMap);
			return classMap;
		}

		protected virtual void DoDeepCopy(IClassMap classMap)
		{
			IPropertyMap clonePropertyMap;
			ICodeMap cloneCodeMap;
			IEnumValueMap cloneEnumValueMap;
			foreach (IPropertyMap propertyMap in this.PropertyMaps)
			{
				clonePropertyMap = (IPropertyMap) propertyMap.DeepClone();
				clonePropertyMap.ClassMap = classMap;
			}
			foreach (ICodeMap codeMap in this.CodeMaps)
			{
				cloneCodeMap = (ICodeMap) codeMap.DeepClone();
				classMap.CodeMaps.Add(cloneCodeMap);
			}
			foreach (IEnumValueMap enumValueMap in this.EnumValueMaps)
			{
				cloneEnumValueMap = (IEnumValueMap) enumValueMap.DeepClone();
				classMap.EnumValueMaps.Add(cloneEnumValueMap);
			}
		}

		public override void DeepCopy(IMap mapObject)
		{
			IClassMap classMap = (IClassMap) mapObject;
			classMap.PropertyMaps.Clear();
			classMap.CodeMaps.Clear();
			classMap.EnumValueMaps.Clear();
			Copy(classMap);
			DoDeepCopy(classMap);
		}

		public override bool DeepCompare(IMap compareTo)
		{
			if (!(Compare(compareTo)))
			{
				return false;
			}
			IClassMap classMap = (IClassMap) compareTo;
			IPropertyMap checkPropertyMap;
			ICodeMap checkCodeMap;
			IEnumValueMap checkEnumValueMap;
			if (!(this.PropertyMaps.Count == classMap.PropertyMaps.Count))
			{
				return false;
			}
			if (!(this.CodeMaps.Count == classMap.CodeMaps.Count))
			{
				return false;
			}
			if (!(this.EnumValueMaps.Count == classMap.EnumValueMaps.Count))
			{
				return false;
			}
			foreach (IPropertyMap propertyMap in this.PropertyMaps)
			{
				checkPropertyMap = classMap.GetPropertyMap(propertyMap.Name);
				if (checkPropertyMap == null)
				{
					return false;
				}
				else
				{
					if (!(propertyMap.DeepCompare(checkPropertyMap)))
					{
						return false;
					}
				}
			}
			foreach (ICodeMap codeMap in this.CodeMaps)
			{
				checkCodeMap = classMap.GetCodeMap(codeMap.CodeLanguage);
				if (checkCodeMap == null)
				{
					return false;
				}
				else
				{
					if (!(codeMap.DeepCompare(checkCodeMap)))
					{
						return false;
					}
				}
			}
			foreach (IEnumValueMap enumValueMap in this.EnumValueMaps)
			{
				checkEnumValueMap = classMap.GetEnumValueMap(enumValueMap.Name);
				if (checkEnumValueMap == null)
				{
					return false;
				}
				else
				{
					if (!(enumValueMap.DeepCompare(checkEnumValueMap)))
					{
						return false;
					}
				}
			}
			return true;
		}

		public override void DeepMerge(IMap mapObject)
		{
			Copy(mapObject);
			IClassMap classMap = (IClassMap) mapObject;
			IPropertyMap propertyMap;
			IPropertyMap checkPropertyMap;
			ICodeMap codeMap;
			ICodeMap checkCodeMap;
			IEnumValueMap enumValueMap;
			IEnumValueMap checkEnumValueMap;
			ArrayList remove = new ArrayList();
			foreach (IPropertyMap iPropertyMap in this.PropertyMaps)
			{
				checkPropertyMap = classMap.GetPropertyMap(iPropertyMap.Name);
				if (checkPropertyMap == null)
				{
					checkPropertyMap = (IPropertyMap) iPropertyMap.DeepClone();
					checkPropertyMap.ClassMap = classMap;
				}
				else
				{
					iPropertyMap.DeepMerge(checkPropertyMap);
				}
			}
			foreach (IPropertyMap iPropertyMap in classMap.PropertyMaps)
			{
				propertyMap = this.GetPropertyMap(iPropertyMap.Name);
				if (propertyMap == null)
				{
					remove.Add(iPropertyMap);
				}
			}
			foreach (IPropertyMap iPropertyMap in remove)
			{
				classMap.PropertyMaps.Remove(iPropertyMap);
			}

			remove.Clear() ;
			foreach (ICodeMap iCodeMap in this.CodeMaps)
			{
				checkCodeMap = classMap.GetCodeMap(iCodeMap.CodeLanguage);
				if (checkCodeMap == null)
				{
					checkCodeMap = (ICodeMap) iCodeMap.DeepClone();
					classMap.CodeMaps.Add(checkCodeMap);
				}
				else
				{
					iCodeMap.DeepMerge(checkCodeMap);
				}
			}
			foreach (ICodeMap iCodeMap in classMap.CodeMaps)
			{
				codeMap = this.GetCodeMap(iCodeMap.CodeLanguage );
				if (codeMap == null)
				{
					remove.Add(iCodeMap);
				}
			}
			foreach (ICodeMap iCodeMap in remove)
			{
				classMap.CodeMaps.Remove(iCodeMap);
			}

			remove.Clear() ;
			foreach (IEnumValueMap iEnumValueMap in this.EnumValueMaps)
			{
				checkEnumValueMap = classMap.GetEnumValueMap(iEnumValueMap.Name);
				if (checkEnumValueMap == null)
				{
					checkEnumValueMap = (IEnumValueMap) iEnumValueMap.DeepClone();
					classMap.EnumValueMaps.Add(checkEnumValueMap);
				}
				else
				{
					iEnumValueMap.DeepMerge(checkEnumValueMap);
				}
			}
			foreach (IEnumValueMap iEnumValueMap in classMap.EnumValueMaps)
			{
				enumValueMap = this.GetEnumValueMap(iEnumValueMap.Name);
				if (enumValueMap == null)
				{
					remove.Add(iEnumValueMap);
				}
			}
			foreach (IEnumValueMap iEnumValueMap in remove)
			{
				classMap.EnumValueMaps.Remove(iEnumValueMap);
			}

		}

		public override void Copy(IMap mapObject)
		{
			IClassMap classMap = (IClassMap) mapObject;
			classMap.IdentitySeparator = this.IdentitySeparator;
			classMap.KeySeparator = this.KeySeparator;
			classMap.InheritanceType = this.InheritanceType;
			classMap.InheritsClass = this.InheritsClass;
			classMap.MergeBehavior = this.MergeBehavior;
			classMap.RefreshBehavior = this.RefreshBehavior;
			classMap.Name = this.Name;
			classMap.ClassType = this.ClassType;
			classMap.Source = this.Source;
			classMap.Table = this.Table;
			classMap.TypeColumn = this.TypeColumn;
			classMap.TypeValue = this.TypeValue;
			classMap.IsAbstract = this.IsAbstract;
			classMap.IsReadOnly = this.IsReadOnly;
			classMap.InheritsTransientClass = this.InheritsTransientClass;
			classMap.ImplementsInterfaces = (ArrayList) this.ImplementsInterfaces.Clone();
			classMap.ImportsNamespaces = (ArrayList) this.ImportsNamespaces.Clone();
			classMap.DeleteOptimisticConcurrencyBehavior = this.DeleteOptimisticConcurrencyBehavior;
			classMap.UpdateOptimisticConcurrencyBehavior = this.UpdateOptimisticConcurrencyBehavior;
			classMap.AssemblyName = this.AssemblyName;
			classMap.ValidateMethod = this.ValidateMethod;
			classMap.LoadSpan = this.LoadSpan;
			classMap.ValidationMode = this.ValidationMode;
			classMap.TimeToLive = this.TimeToLive;
			classMap.TimeToLiveBehavior = this.TimeToLiveBehavior;
			classMap.LoadBehavior = this.LoadBehavior;
			classMap.SourceClass = this.SourceClass;
			classMap.DocSource = this.DocSource;
			classMap.DocElement = this.DocElement;
			classMap.DocClassMapMode = this.DocClassMapMode;
			classMap.DocParentProperty = this.DocParentProperty;
			classMap.DocRoot = this.DocRoot;
		}

		public override bool Compare(IMap compareTo)
		{
			if (compareTo == null)
			{
				return false;
			}
			IClassMap classMap = (IClassMap) compareTo;
			if (!(classMap.IdentitySeparator == this.IdentitySeparator))
			{
				return false;
			}
			if (!(classMap.ClassType  == this.ClassType))
			{
				return false;
			}
			if (!(classMap.KeySeparator == this.KeySeparator))
			{
				return false;
			}
			if (!(classMap.InheritanceType == this.InheritanceType))
			{
				return false;
			}
			if (!(classMap.InheritsClass == this.InheritsClass))
			{
				return false;
			}
			if (!(classMap.MergeBehavior == this.MergeBehavior))
			{
				return false;
			}
			if (!(classMap.RefreshBehavior == this.RefreshBehavior))
			{
				return false;
			}
			if (!(classMap.Name == this.Name))
			{
				return false;
			}
			if (!(classMap.Source == this.Source))
			{
				return false;
			}
			if (!(classMap.Table == this.Table))
			{
				return false;
			}
			if (!(classMap.TypeColumn == this.TypeColumn))
			{
				return false;
			}
			if (!(classMap.TypeValue == this.TypeValue))
			{
				return false;
			}
			if (!(classMap.IsAbstract == this.IsAbstract))
			{
				return false;
			}
			if (!(classMap.IsReadOnly == this.IsReadOnly))
			{
				return false;
			}
			if (!(classMap.InheritsTransientClass == this.InheritsTransientClass))
			{
				return false;
			}
			if (!(classMap.DeleteOptimisticConcurrencyBehavior == this.DeleteOptimisticConcurrencyBehavior))
			{
				return false;
			}
			if (!(classMap.UpdateOptimisticConcurrencyBehavior == this.UpdateOptimisticConcurrencyBehavior))
			{
				return false;
			}
			if (!(classMap.AssemblyName == this.AssemblyName))
			{
				return false;
			}
			if (!(classMap.LoadSpan == this.LoadSpan))
			{
				return false;
			}
			if (!(classMap.TimeToLive == this.TimeToLive))
			{
				return false;
			}
			if (!(classMap.TimeToLiveBehavior == this.TimeToLiveBehavior))
			{
				return false;
			}
			if (!(classMap.LoadBehavior == this.LoadBehavior))
			{
				return false;
			}
			if (!(classMap.ValidateMethod == this.ValidateMethod))
			{
				return false;
			}
			if (!(classMap.ValidationMode == this.ValidationMode))
			{
				return false;
			}
			if (!(classMap.SourceClass == this.SourceClass))
			{
				return false;
			}
			if (!(classMap.DocSource == this.DocSource))
			{
				return false;
			}
			if (!(classMap.DocElement == this.DocElement))
			{
				return false;
			}
			if (!(classMap.DocRoot == this.DocRoot))
			{
				return false;
			}
			if (!(classMap.DocClassMapMode == this.DocClassMapMode))
			{
				return false;
			}
			if (!(classMap.DocParentProperty == this.DocParentProperty))
			{
				return false;
			}
			if (!(CompareArrayLists(classMap.ImplementsInterfaces, this.ImplementsInterfaces)))
			{
				return false;
			}
			if (!(CompareArrayLists(classMap.ImportsNamespaces, this.ImportsNamespaces)))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region IMap

		public override string GetKey()
		{
			return m_DomainMap.Name + "." + this.Name;
		}

		#endregion

		#region IFixate

		public override void Fixate()
		{
			base.Fixate();
			foreach (IPropertyMap propertyMap in m_PropertyMaps)
			{
				propertyMap.Fixate();
			}
		}

		public override void UnFixate()
		{
			base.UnFixate();
			foreach (IPropertyMap propertyMap in m_PropertyMaps)
			{
				propertyMap.UnFixate();
			}
		}

		#endregion
		
		public virtual bool IsLegalAsSuperClass(IClassMap classMap)
		{
			IClassMap superClassMap = null;
			if (classMap == null)
			{
				return true;
			}
			if (classMap == this)
			{
				return false;
			}
			superClassMap = classMap.GetInheritedClassMap();
			while (superClassMap != null)
			{
				if (superClassMap == this)
				{
					return false;
				}
				superClassMap = superClassMap.GetInheritedClassMap();
			}
			return true;
		}

		//Determines if this classMap has one or more uni-directional (lacking inverse property) 
		//reference properties to the specified class or any of its superclasses
		public virtual bool HasUniDirectionalReferenceTo(IClassMap classMap, bool nullableOnly)
		{
			foreach (IPropertyMap propertyMap in this.GetAllPropertyMaps())
			{
				if (propertyMap.ReferenceType != ReferenceType.None)
				{
					IClassMap refClassMap = propertyMap.GetReferencedClassMap();
					if (refClassMap != null)
					{
						if (refClassMap.IsSubClassOrThisClass(classMap))
						{
							if (propertyMap.GetInversePropertyMap() == null)
							{
								if (nullableOnly == false || propertyMap.GetIsNullable())
								{
									return true;									
								}
							}
						}						
					}
				}
			}
			return false;
		}

		public virtual IList GetUniDirectionalReferencesTo(IClassMap classMap, bool nullableOnly)
		{
			IList result = new ArrayList() ; 
			foreach (IPropertyMap propertyMap in this.GetAllPropertyMaps())
			{
				if (propertyMap.ReferenceType != ReferenceType.None)
				{
					IClassMap refClassMap = propertyMap.GetReferencedClassMap();
					if (refClassMap != null)
					{
						if (refClassMap.IsSubClassOrThisClass(classMap))
						{
							if (propertyMap.GetInversePropertyMap() == null)
							{
								if (nullableOnly == false || propertyMap.GetIsNullable())
								{
									result.Add(propertyMap);
								}
							}
						}						
					}
				}
			}
			return result;
		}

		public virtual bool HasIdentityGenerators()
		{
			foreach (IPropertyMap propertyMap in this.GetIdentityPropertyMaps())
				if (propertyMap.IdentityGenerator.Length > 0)
					return true;
			return false;
		}

		#region DOL additions

		/// <summary>
		/// Gets the all 'find by' groups.
		/// </summary>
		/// <returns></returns>
		public IDictionary<int, IList<IPropertyMap>> DOLGetFindByGroups()
		{
			Dictionary<int, Dictionary<int, IPropertyMap>> dic = new Dictionary<int, Dictionary<int, IPropertyMap>>();
			Dictionary<int, IList<IPropertyMap>> result = new Dictionary<int, IList<IPropertyMap>>();
			
			// group all
			foreach (IPropertyMap prop in PropertyMaps)
			{
				if (prop.DOLFindByGroup >= 0)
				{
					Dictionary<int, IPropertyMap> group;
					bool oldList = dic.TryGetValue(prop.DOLFindByGroup, out group);
					if (!oldList)
					{
						group = new Dictionary<int, IPropertyMap>();
					}
					
					group.Add(prop.DOLFindByGroupIndex, prop);
					
					if (!oldList)
					{
						dic.Add(prop.DOLFindByGroup, group);
					}
				}
			}
			
			// sort all
			foreach (KeyValuePair<int, Dictionary<int, IPropertyMap>> pair in dic)
			{
				Dictionary<int, IPropertyMap> group = pair.Value;
				List<IPropertyMap> sortedValues = new List<IPropertyMap>(pair.Value.Count);
				foreach (IPropertyMap propSorted in new SortedDictionary<int, IPropertyMap>(group).Values)
				{
					sortedValues.Add(propSorted);
				}
				result.Add(pair.Key, sortedValues);
			}
			
			return result;
		}

		#endregion

	}
}