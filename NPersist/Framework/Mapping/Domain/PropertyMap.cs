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
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Mapping.Visitor;

namespace Puzzle.NPersist.Framework.Mapping
{
	public class PropertyMap : MapBase, IPropertyMap
	{
				
		public override void Accept(IMapVisitor visitor)
		{
			visitor.Visit(this);
		}
	
		public override IMap GetParent()
		{
			return m_ClassMap; 
		}

		#region Private Member Variables

		//O/R Mapping
		private IClassMap m_ClassMap;
		private string m_Name = "";
		private string m_ValidateMethod = "";
		private string m_FieldName = "";
		private string m_DataType = "";
		private string m_ItemType = "";
		private string m_DefaultValue = "";
		private bool m_IsCollection = false;
		private bool m_IsIdentity = false;
		private int m_IdentityIndex = 0;
		private bool m_IsKey = false;
		private int m_KeyIndex = 0;
		private string m_IdentityGenerator = "";
		private bool m_IsNullable = false;
		private bool m_IsAssignedBySource = false;
		private int m_MaxLength = -1;
		private int m_MinLength = -1;
		private string m_MaxValue = "";
		private string m_MinValue = "";
		private string m_Source = "";
		private string m_Table = "";
		private string m_Column = "";
		private string m_IdColumn = "";
		private ArrayList m_AdditionalColumns = new ArrayList();
		private ArrayList m_AdditionalIdColumns = new ArrayList();
		private string m_Inverse = "";
		private bool m_LazyLoad = false;
		private bool m_IsReadOnly = false;
		private bool m_IsSlave = false;
		private string m_NullSubstitute = "";
		private bool m_NoInverseManagement = false;
		private bool m_InheritInverseMappings = false;
		private ReferenceType m_ReferenceType = ReferenceType.None;
		private ReferenceQualifier m_ReferenceQualifier = ReferenceQualifier.Default;
		private bool m_CascadingCreate = false;
		private bool m_CascadingDelete = false;
		private AccessibilityType m_Accessibility = AccessibilityType.PublicAccess;
		private AccessibilityType m_FieldAccessibility = AccessibilityType.PrivateAccess;
		private OptimisticConcurrencyBehaviorType m_UpdateOptimisticConcurrencyBehavior = OptimisticConcurrencyBehaviorType.DefaultBehavior;
		private OptimisticConcurrencyBehaviorType m_DeleteOptimisticConcurrencyBehavior = OptimisticConcurrencyBehaviorType.DefaultBehavior;
		private PropertySpecialBehaviorType m_OnCreateBehavior = PropertySpecialBehaviorType.None;
		private PropertySpecialBehaviorType m_OnPersistBehavior = PropertySpecialBehaviorType.None;
		private string m_OrderBy = "";
		private PropertyModifier m_PropertyModifier = PropertyModifier.Default;
		private MergeBehaviorType m_MergeBehavior = MergeBehaviorType.DefaultBehavior;
		private RefreshBehaviorType m_RefreshBehavior = RefreshBehaviorType.DefaultBehavior;
		private ValidationMode m_ValidationMode = ValidationMode.Default;
		private long m_TimeToLive = -1;
		private TimeToLiveBehavior m_TimeToLiveBehavior = TimeToLiveBehavior.Default ;

		//O/O Mapping
		private string m_SourceProperty = "";

		//O/D Mapping
		private string m_DocSource = "";
		private string m_DocAttribute = "";
		private string m_DocElement = "";
		private DocPropertyMapMode m_DocPropertyMapMode = DocPropertyMapMode.Default;

		#endregion

		#region Constructors

		public PropertyMap() : base()
		{
		}

		public PropertyMap(string name) : base()
		{
			m_Name = name;
		}

		#endregion

		#region Object/Relational Mapping

		[XmlIgnore()]
		public virtual IClassMap ClassMap
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				return m_ClassMap;
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				if (m_ClassMap != null)
				{
					m_ClassMap.PropertyMaps.Remove(this);
				}
				m_ClassMap = value;
				if (m_ClassMap != null)
				{
					m_ClassMap.PropertyMaps.Add(this);
				}
			}
		}

		public virtual void SetClassMap(IClassMap value)
		{
			m_ClassMap = value;
		}

		public override string Name
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				return m_Name;
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				m_Name = value;
			}
		}

		
		public virtual string ValidateMethod
		{
			get
			{
				return m_ValidateMethod;
			}
			set
			{
				m_ValidateMethod = value;
			}
		}


		public virtual string FieldName
		{
			get
			{
				return m_FieldName;
			}
			set
			{
				m_FieldName = value;
			}
		}

		public virtual bool IsNullable
		{
			get
			{
				return m_IsNullable;
			}
			set
			{
				m_IsNullable = value;
			}
		}

		public virtual bool IsAssignedBySource
		{
			get
			{
				return m_IsAssignedBySource;
			}
			set
			{
				m_IsAssignedBySource = value;
			}
		}
		public virtual int MaxLength
		{
			get
			{
				return m_MaxLength;
			}
			set
			{
				m_MaxLength = value;
			}
		}

		public virtual int MinLength
		{
			get { return m_MinLength; }
			set { m_MinLength = value; }
		}

		public virtual string MaxValue
		{
			get { return m_MaxValue; }
			set { m_MaxValue = value; }
		}

		public virtual string MinValue
		{
			get { return m_MinValue; }
			set { m_MinValue = value; }
		}

        //Because there is no difference in the table model when 
        //the slave table has the same number of rows as the master table 
        //(for example: Users and Profiles)) and when the slave table
        //has more rows than the master, it is not dicernable from the 
        //table model if the slave property in a OneToOne relationship
        //is nullable or not! Thus we have to use the Nullable attribute
        //of the propertyMap for a slave OneToOne property!!
		public virtual bool GetIsNullable()
		{
            if (IsFixed("GetIsNullable"))
            {
                return (bool)GetFixedValue("GetIsNullable");
            }
            bool isNullable = false;
			IColumnMap columnMap = null;
            if (this.ReferenceType == ReferenceType.OneToOne && this.IsSlave)
            {
                //This would have worked if it wasn't for the fact that when 
                //the id column is /not/ nullable, the slave OneToOne property 
                //may still be nullable if the slave table contains more rows
                //than the master table (see comment ^^)
			    //columnMap = this.GetIdColumnMap();
            }
            else
            {
			    columnMap = this.GetColumnMap();
            }
            if (columnMap != null)
            {
                isNullable = columnMap.AllowNulls;
            }
            else
            {
                isNullable = this.m_IsNullable;
            }
            if (IsFixed())
            {
                SetFixedValue("GetIsNullable", isNullable);
            }

            return isNullable;
		}


		public virtual bool GetIsAssignedBySource()
		{
			IColumnMap columnMap = this.GetColumnMap();
			if (columnMap != null)
			{
				if (columnMap.IsAutoIncrease)
				{
					return true;
				}
			}
			return this.m_IsAssignedBySource;
		}

		public virtual int GetMaxLength()
		{
			if (!this.IsCollection)
			{
				IColumnMap columnMap = this.GetColumnMap();
				if (columnMap != null)
				{
					switch (columnMap.DataType)
					{
						case DbType.AnsiString :
							return columnMap.Precision ;
						case DbType.AnsiStringFixedLength :
							return columnMap.Precision ;
						case DbType.String  :
							return columnMap.Precision ;
						case DbType.StringFixedLength :
							return columnMap.Precision ;
					}
				}
			}
			return this.m_MaxLength;
		}

		//[DebuggerStepThrough()]
		public virtual string GetFieldName()
		{
			if (IsFixed("GetFieldName"))
			{
				return (string) GetFixedValue("GetFieldName");
			}
			string fn;
			if (m_FieldName == "")
			{
				IDomainMap dm = ClassMap.DomainMap;
				string pre = dm.FieldPrefix;
				string strategyName = "";
				if (dm.FieldNameStrategy == FieldNameStrategyType.None)
				{
					strategyName = m_Name;
				}
				else if (dm.FieldNameStrategy == FieldNameStrategyType.CamelCase)
				{
					strategyName = m_Name.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + m_Name.Substring(1);
				}
				else if (dm.FieldNameStrategy == FieldNameStrategyType.PascalCase)
				{
					strategyName = m_Name.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + m_Name.Substring(1);
				}
				if (pre.Length > 0)
				{
					fn = pre + strategyName;

				}
				else
				{
					if (!(strategyName == m_Name))
					{
						fn = strategyName;
					}
					else
					{
						if (!(m_Name.Substring(0, 1) == m_Name.Substring(0, 1).ToLower(CultureInfo.InvariantCulture)))
						{
							fn = m_Name.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + m_Name.Substring(1);
						}
						else
						{
							fn = "m_" + m_Name;
						}
					}
				}
			}
			else
			{
				fn = m_FieldName;
			}
            if (IsFixed())
			{
				SetFixedValue("GetFieldName", fn);
			}
			return fn;
		}

		public virtual string DataType
		{
			get
			{
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					if (inv.ReferenceType == ReferenceType.ManyToMany || inv.ReferenceType == ReferenceType.OneToMany)
					{
						if (m_DataType == "")
						{
							if (m_IsCollection)
							{
								return "System.Collections.IList";
							}
						}
						return m_DataType;
					}
					else if (inv.ReferenceType == ReferenceType.ManyToOne || inv.ReferenceType == ReferenceType.OneToOne)
					{
						return inv.ClassMap.Name;
					}
				}
				else
				{
					if (IsSlave == false && IsCollection && ReferenceType == ReferenceType.ManyToMany && Inverse.Length > 0 && NoInverseManagement == false)
					{
						//return "Puzzle.NPersist.Framework.InterceptableList";
						return "System.Collections.IList";
					}
					if (m_DataType == "")
					{
						if (m_IsCollection)
						{
							return "System.Collections.IList";
						}
					}
					return m_DataType;
				}
				return m_DataType;
			}
			set
			{
				m_DataType = value;
			}
		}

		public virtual string ItemType
		{
			get
			{
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					if (inv.ReferenceType == ReferenceType.ManyToMany || inv.ReferenceType == ReferenceType.OneToMany)
					{
						return inv.ClassMap.Name;
					}
					else if (inv.ReferenceType == ReferenceType.ManyToOne || inv.ReferenceType == ReferenceType.OneToOne)
					{
						return m_ItemType;
					}
				}
				else
				{
					return m_ItemType;
				}
				return m_ItemType;
			}
			set
			{
				m_ItemType = value;
			}
		}

		public virtual string DefaultValue
		{
			get
			{
				if (IsCollection)
				{
					//NPersist wants to fill list properties with instances of lists
					return "";
//					if (m_DefaultValue.Length > 0)
//					{
//						return m_DefaultValue;
//					}
//					else
//					{
//						if (IsReadOnly == false && ReferenceType == ReferenceType.ManyToMany && Inverse.Length > 0 && NoInverseManagement == false)
//						{
//							return "new " + DataType + "(this, \"" + Name + "\")"; // do not localize
//						}
//						else
//						{
//							return "new " + DataType + "()"; // do not localize
//						}
//					}
				}
				else
				{
					return m_DefaultValue;
				}
			}
			set
			{
				m_DefaultValue = value;
			}
		}

		public virtual bool IsCollection
		{
			get
			{
                if (IsFixed("IsCollection"))
                {
                    return (bool)GetFixedValue("IsCollection");
                }

                bool isCollection = false;
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					if (inv.ReferenceType == ReferenceType.ManyToMany || inv.ReferenceType == ReferenceType.OneToMany)
					{
						isCollection = true;
					}
                    else if (inv.ReferenceType == ReferenceType.ManyToOne || inv.ReferenceType == ReferenceType.OneToOne)
                    {
                        isCollection = false;
                    }
                    else
                    {
                        isCollection = false;
                    }
				}
				else
				{
                    isCollection = m_IsCollection;
				}

                if (IsFixed())
                {
                    SetFixedValue("IsCollection", isCollection);
                }
                return isCollection;
            }
			set
			{
				m_IsCollection = value;
			}
		}

		public virtual IClassMap MustGetReferencedClassMap()
		{
			IClassMap classMap = GetReferencedClassMap();

			if (classMap == null)
			{
				string className;
				if (m_IsCollection)
				{
					className = m_ItemType;
				}
				else
				{
					className = m_DataType;
				}
				
				throw new MappingException("Could not find type " + className + ", referenced by property " + this.ClassMap.Name + "." + this.Name + ", in map file!");
			}

			return classMap;
		}

		public virtual IClassMap GetReferencedClassMap()
		{
			if (m_ReferenceType == ReferenceType.None)
			{
				return null;
			}
			string className;
			if (m_IsCollection)
			{
				className = m_ItemType;
			}
			else
			{
				className = m_DataType;
			}
			if (className.Length < 1)
			{
				return null;
			}
			string ns;
			IClassMap classMap = m_ClassMap.DomainMap.GetClassMap(className);
			if (classMap == null)
			{
				ns = m_ClassMap.GetNamespace();
				if (ns.Length > 0)
				{
					classMap = m_ClassMap.DomainMap.GetClassMap(ns + "." + className);
				}
			}
			return classMap;
		}

		public virtual string Source
		{
			get
			{
				if (DoesInheritInverseMappings())
				{
					string name;
					IPropertyMap inv = GetInversePropertyMap();
					name = inv.Source;
					return name;
				}
				else
				{
					return m_Source;
				}
			}
			set
			{
				m_Source = value;
			}
		}

		public virtual ISourceMap GetSourceMap()
		{
			if (this.Source == "")
			{
				return m_ClassMap.GetSourceMap();
			}
			else
			{
				return m_ClassMap.DomainMap.GetSourceMap(this.Source);
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
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					if (inv.Table.Length < 1)
					{
						return inv.ClassMap.Table;
					}
					else
					{
						return inv.Table;
					}
				}
				else
				{
					return m_Table;
				}
			}
			set
			{
				m_Table = value;
			}
		}


		public virtual ITableMap MustGetTableMap()
		{
			ITableMap tableMap = GetTableMap();

			if (tableMap == null)
				throw new MappingException("Could not find table " + m_Table + ", mapped to by property " + this.ClassMap.Name + "." + this.Name + ", in map file!");

			return tableMap;
		}


		public virtual ITableMap GetTableMap()
		{
			if (this.Table == "")
			{
				return m_ClassMap.GetTableMap();
			}
			else
			{
				ISourceMap sourceMap = GetSourceMap();
				if (sourceMap != null)
				{
					return GetSourceMap().GetTableMap(this.Table);
				}
			}
			return null;
		}

		public virtual void SetTableMap(ITableMap value)
		{
			m_Table = value.Name;
		}

		public virtual string Column
		{
			get
			{
				if (DoesInheritInverseMappings())
				{
					if (ReferenceType == ReferenceType.ManyToOne)
					{
						return "";
					}
					IPropertyMap inv = GetInversePropertyMap();
					if (inv.IdColumn.Length > 0)
					{
						return inv.IdColumn;
					}
					else
					{
						ArrayList idProps = inv.ClassMap.GetIdentityPropertyMaps();
						if (idProps.Count == 1)
						{
							return ((IPropertyMap) (idProps[0])).Column;
						}
					}
				}
				else
				{
					return m_Column;
				}
				return m_Column;
			}
			set
			{
				m_Column = value;
			}
		}

		public virtual IColumnMap MustGetColumnMap()
		{
			IColumnMap columnMap = GetColumnMap();

			if (columnMap == null)
				throw new MappingException("Could not find column '" + this.Column + "' for the property " + this.Name + " of type " + this.ClassMap.GetFullName() + " in map file!");

			return columnMap;
		}

		public virtual IColumnMap GetColumnMap()
		{
			if (this.Column.Length < 1)
			{
				return null;
			}
			ITableMap tableMap = GetTableMap();
			if (tableMap == null)
			{
				return null;
			}
			return tableMap.GetColumnMap(this.Column);
		}

		public virtual void SetColumnMap(IColumnMap value)
		{
			m_Column = value.Name;
		}

		public virtual string IdColumn
		{
			get
			{
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					return inv.Column;
				}
				else
				{
					return m_IdColumn;
				}
			}
			set
			{
				m_IdColumn = value;
			}
		}

		public virtual IColumnMap GetIdColumnMap()
		{
			if (this.IdColumn.Length < 1)
			{
				return null;
			}
			ITableMap tableMap = GetTableMap();
			if (tableMap == null)
			{
				return null;
			}
			return tableMap.GetColumnMap(this.IdColumn);
		}

		public virtual void SetIdColumnMap(IColumnMap value)
		{
			m_IdColumn = value.Name;
		}

		[XmlArrayItem(typeof (string))]
		public virtual ArrayList AdditionalColumns
		{
			get
			{
				if (DoesInheritInverseMappings())
				{
					if (ReferenceType == ReferenceType.ManyToOne)
					{
						return new ArrayList();
					}
					IPropertyMap inv = GetInversePropertyMap();
					return inv.AdditionalIdColumns;
				}
				else
				{
					return m_AdditionalColumns;
				}
			}
			set
			{
				m_AdditionalColumns = value;
			}
		}

		public virtual ArrayList GetAdditionalColumnMaps()
		{
			ArrayList columnMaps = new ArrayList();
			ITableMap tableMap = GetTableMap();
			if (tableMap != null)
			{
				IColumnMap columnMap;
				foreach (string colName in AdditionalColumns)
				{
					columnMap = tableMap.GetColumnMap(colName);
					if (columnMap != null)
					{
						columnMaps.Add(columnMap);
					}
				}
			}
			return columnMaps;
		}

		[XmlArrayItem(typeof (string))]
		public virtual ArrayList AdditionalIdColumns
		{
			get
			{
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					return inv.AdditionalColumns;
				}
				else
				{
					return m_AdditionalIdColumns;
				}
			}
			set
			{
				m_AdditionalIdColumns = value;
			}
		}

		public virtual ArrayList GetAdditionalIdColumnMaps()
		{
			ArrayList columnMaps = new ArrayList();
			ITableMap tableMap = GetTableMap();
			if (tableMap != null)
			{
				IColumnMap columnMap;
				foreach (string colName in AdditionalIdColumns)
				{
					columnMap = tableMap.GetColumnMap(colName);
					if (columnMap != null)
					{
						columnMaps.Add(columnMap);
					}
				}
			}
			return columnMaps;
		}

		public virtual string Inverse
		{
			get
			{
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					return inv.Name;
				}
				else
				{
					return m_Inverse;
				}
			}
			set
			{
				m_Inverse = value;
			}
		}

		public virtual IPropertyMap MustGetInversePropertyMap()
		{
			IPropertyMap propertyMap = GetInversePropertyMap();

			if (propertyMap == null)
				throw new MappingException("Could not find property " + m_Inverse + ", specified as inverse to property " + this.ClassMap.Name + "." + this.Name + ", in map file!");

			return propertyMap;
		}

		public virtual IPropertyMap GetInversePropertyMap()
		{
			if (m_Inverse.Length < 1)
			{
				return null;
			}
			IClassMap classMap = GetReferencedClassMap();
			if (classMap != null)
			{
				return classMap.GetPropertyMap(m_Inverse);
			}
			return null;
		}

		public virtual void SetInversePropertyMap(IPropertyMap value)
		{
			m_Inverse = value.Name;
		}

		public virtual bool IsIdentity
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				return m_IsIdentity;
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				m_IsIdentity = value;
			}
		}

		public int IdentityIndex
		{
			get
			{
				return m_IdentityIndex;
			}
			set
			{
				m_IdentityIndex = value;
			}
		}

		public virtual bool IsKey
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				return m_IsKey;
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				m_IsKey = value;
			}
		}

		public int KeyIndex
		{
			get
			{
				return m_KeyIndex;
			}
			set
			{
				m_KeyIndex = value;
			}
		}

		public string IdentityGenerator
		{
			get
			{
				return m_IdentityGenerator;
			}
			set
			{
				m_IdentityGenerator = value;
			}
		}

		public virtual bool LazyLoad
		{
			get
			{
				return m_LazyLoad;
			}
			set
			{
				m_LazyLoad = value;
			}
		}

		public virtual bool IsReadOnly
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				if (ClassMap.IsReadOnly)
				{
					return true;
				}
				else
				{
					return m_IsReadOnly;
				}
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				m_IsReadOnly = value;
			}
		}

		public virtual bool IsSlave
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					return !(inv.IsSlave);
				}
				else
				{
					return m_IsSlave;
				}
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				m_IsSlave = value;
			}
		}

		public virtual string NullSubstitute
		{
			get
			{
				return m_NullSubstitute;
			}
			set
			{
				m_NullSubstitute = value;
			}
		}

		public virtual bool NoInverseManagement
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					return inv.NoInverseManagement;
				}
				else
				{
					return m_NoInverseManagement;
				}
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				m_NoInverseManagement = value;
			}
		}

		public virtual bool InheritInverseMappings
		{
			get
			{
				return m_InheritInverseMappings;
			}
			set
			{
				m_InheritInverseMappings = value;
			}
		}

		public virtual bool CascadingCreate
		{
			get
			{
				return m_CascadingCreate;
			}
			set
			{
				m_CascadingCreate = value;
			}
		}

		public virtual bool CascadingDelete
		{
			get
			{
				return m_CascadingDelete;
			}
			set
			{
				m_CascadingDelete = value;
			}
		}


		//[DebuggerStepThrough()]
		public virtual bool DoesInheritInverseMappings()
		{
			if (!(m_InheritInverseMappings))
			{
				return false;
			}
			if (m_ReferenceType == ReferenceType.None)
			{
				return false;
			}
			if (!(m_Inverse.Length > 0))
			{
				return false;
			}
			IPropertyMap inv = GetInversePropertyMap();
			if (inv == null)
			{
				return false;
			}
			if (inv == this)
			{
				return false;
			}
			if (inv.InheritInverseMappings)
			{
				return false;
			}
			return true;
		}

		public virtual ReferenceType ReferenceType
		{
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			get
			{
				if (DoesInheritInverseMappings())
				{
					IPropertyMap inv = GetInversePropertyMap();
					if (inv.ReferenceType == ReferenceType.ManyToMany)
					{
						return ReferenceType.ManyToMany;
					}
					else if (inv.ReferenceType == ReferenceType.ManyToOne)
					{
						return ReferenceType.OneToMany;
					}
					else if (inv.ReferenceType == ReferenceType.OneToMany)
					{
						return ReferenceType.ManyToOne;
					}
					else if (inv.ReferenceType == ReferenceType.OneToOne)
					{
						return ReferenceType.OneToOne;
					}
					else if (inv.ReferenceType == ReferenceType.None)
					{
						return ReferenceType.None;
					}
					return ReferenceType.None;
				}
				else
				{
					return m_ReferenceType;
				}
			}
			//[DebuggerHidden()]
			//[DebuggerStepThrough()]
			set
			{
				m_ReferenceType = value;
			}
		}

		public ReferenceQualifier ReferenceQualifier
		{
			get { return this.m_ReferenceQualifier; }
			set { this.m_ReferenceQualifier = value; }
		}

		public virtual void UpdateName(string newName)
		{
			foreach (IClassMap checkClassMap in ClassMap.DomainMap.ClassMaps)
			{
				foreach (IPropertyMap propertyMap in checkClassMap.GetNonInheritedPropertyMaps())
				{
					if (!(propertyMap.ReferenceType == ReferenceType.None))
					{
						if (propertyMap.GetInversePropertyMap() == this)
						{
							propertyMap.Inverse = newName;
						}
					}
				}
			}
			m_Name = newName;
		}

		public virtual AccessibilityType Accessibility
		{
			get
			{
				return m_Accessibility;
			}
			set
			{
				m_Accessibility = value;
			}
		}

		public virtual AccessibilityType FieldAccessibility
		{
			get
			{
				return m_FieldAccessibility;
			}
			set
			{
				m_FieldAccessibility = value;
			}
		}

		public virtual OptimisticConcurrencyBehaviorType UpdateOptimisticConcurrencyBehavior
		{
			get
			{
				return m_UpdateOptimisticConcurrencyBehavior;
			}
			set
			{
				m_UpdateOptimisticConcurrencyBehavior = value;
			}
		}

		public virtual OptimisticConcurrencyBehaviorType DeleteOptimisticConcurrencyBehavior
		{
			get
			{
				return m_DeleteOptimisticConcurrencyBehavior;
			}
			set
			{
				m_DeleteOptimisticConcurrencyBehavior = value;
			}
		}

		public virtual PropertySpecialBehaviorType OnCreateBehavior
		{
			get
			{
				return m_OnCreateBehavior;
			}
			set
			{
				m_OnCreateBehavior = value;
			}
		}

		public virtual PropertySpecialBehaviorType OnPersistBehavior
		{
			get
			{
				return m_OnPersistBehavior;
			}
			set
			{
				m_OnPersistBehavior = value;
			}
		}

		public virtual string OrderBy
		{
			get
			{
				return m_OrderBy;
			}
			set
			{
				m_OrderBy = value;
			}
		}

		public virtual IPropertyMap GetOrderByPropertyMap()
		{
			if (m_OrderBy.Length < 1)
			{
				return null;
			}
			IClassMap classMap = GetReferencedClassMap();
			if (classMap != null)
			{
				return classMap.GetPropertyMap(m_OrderBy);
			}
			return null;
		}

		public virtual PropertyModifier PropertyModifier
		{
			get
			{
				return m_PropertyModifier;
			}
			set
			{
				m_PropertyModifier = value;
			}
		}

		public virtual MergeBehaviorType MergeBehavior
		{
			get
			{
				return m_MergeBehavior;
			}
			set
			{
				m_MergeBehavior = value;
			}
		}

		public virtual RefreshBehaviorType RefreshBehavior
		{
			get
			{
				return m_RefreshBehavior;
			}
			set
			{
				m_RefreshBehavior = value;
			}
		}

		public ArrayList GetAllColumnMaps()
		{
			ArrayList columnMaps = new ArrayList();
			IColumnMap columnMap = this.GetColumnMap();
			if (columnMap != null)
			{
				columnMaps.Add(columnMap);
			}
			//Thanks to Steven Miller for fixing a bug here
			columnMaps.AddRange(this.GetAdditionalColumnMaps());
			return columnMaps;
		}

		public ArrayList GetAllIdColumnMaps()
		{
			ArrayList columnMaps = new ArrayList();
			IColumnMap columnMap = this.GetIdColumnMap();
			if (columnMap != null)
			{
				columnMaps.Add(columnMap);
			}
			//Thanks to Steven Miller for fixing a bug here
			columnMaps.AddRange(this.GetAdditionalIdColumnMaps());
			return columnMaps;
		}


		public string GetDataOrItemType()
		{
			if (this.IsCollection)
			{
				return this.ItemType;
			}
			else
			{
				return this.DataType;
			}
		}
		
		public ValidationMode ValidationMode
		{
			get { return this.m_ValidationMode; }
			set { this.m_ValidationMode = value; }
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
				return this.ClassMap.GetTimeToLive();
			return this.m_TimeToLive;
		}
		
		public TimeToLiveBehavior GetTimeToLiveBehavior()
		{
			if (this.m_TimeToLiveBehavior == TimeToLiveBehavior.Default)
				return this.ClassMap.GetTimeToLiveBehavior();
			return this.m_TimeToLiveBehavior;
		}

		#endregion

		#region Object/Object Mapping

		public virtual string SourceProperty
		{
			get { return m_SourceProperty; }
			set { m_SourceProperty = value; }
		}

		public virtual IPropertyMap GetSourcePropertyMap()
		{
			if (m_SourceProperty == "")
				return null;

			IClassMap sourceClassMap = this.ClassMap.GetSourceClassMap();

			if (sourceClassMap != null)
			{
				return sourceClassMap.GetPropertyMap(m_SourceProperty);
			}

			return null;
		}


		public virtual IPropertyMap GetSourcePropertyMapOrSelf()
		{
			if (m_SourceProperty == "")
				return this;

			IPropertyMap sourcePropertyMap = this.GetSourcePropertyMap();

			if (sourcePropertyMap == null)
			{
				IClassMap sourceClassMap = this.ClassMap.GetSourceClassMapOrSelf() ;

				if (sourceClassMap != null)
				{
					sourcePropertyMap = sourceClassMap.GetPropertyMap(m_SourceProperty);					
				}
			}

			if (sourcePropertyMap == null)
				sourcePropertyMap = this;

			return sourcePropertyMap;
		}

		#endregion

		#region Object/Document Mapping
		
		public virtual string DocSource
		{
			get { return m_DocSource; }
			set { m_DocSource = value; }
		}
		
		public virtual ISourceMap GetDocSourceMap()
		{
			if (this.DocSource == "")
			{
				return m_ClassMap.GetDocSourceMap();
			}
			else
			{
				return m_ClassMap.DomainMap.GetSourceMap(this.DocSource);
			}
		}

		public virtual void SetDocSourceMap(ISourceMap value)
		{
			m_DocSource = value.Name;
		}

		public virtual string DocAttribute
		{
			get { return m_DocAttribute; }
			set { m_DocAttribute = value; }
		}

		public virtual string GetDocAttribute()
		{
			if (m_DocAttribute.Length > 0)
				return m_DocAttribute;

			return this.Name;
		}

		public virtual string DocElement
		{
			get { return m_DocElement; }
			set { m_DocElement = value; }
		}

		public virtual string GetDocElement()
		{
			if (m_DocElement.Length > 0)
				return m_DocElement;

			return this.Name;
		}
		
		public virtual DocPropertyMapMode DocPropertyMapMode
		{
			get { return m_DocPropertyMapMode; }
			set { m_DocPropertyMapMode = value; }
		}

		#endregion

		#region DOL additions

		private int m_dolFindByGroup = -1;
		private int m_dolFindByGroupIndex = -1;

		/// <summary>
		/// The "FindBy" group this property belongs to.
		/// </summary>
		public int DOLFindByGroup
		{
			get { return m_dolFindByGroup; }
			set { m_dolFindByGroup = value; }
		}

		/// <summary>
		/// The index of the property in the "FindBy" group.
		/// </summary>
		public int DOLFindByGroupIndex
		{
			get { return m_dolFindByGroupIndex; }
			set { m_dolFindByGroupIndex = value; }
		}
		
		#endregion
		
		#region Cloning

		public override IMap Clone()
		{
			IPropertyMap propertyMap = new PropertyMap();
			Copy(propertyMap);
			return propertyMap;
		}

		public override IMap DeepClone()
		{
			IPropertyMap propertyMap = new PropertyMap();
			DeepCopy(propertyMap);
			return propertyMap;
		}

		protected virtual void DoDeepCopy(IPropertyMap propertyMap)
		{
		}

		public override void DeepCopy(IMap mapObject)
		{
			IPropertyMap propertyMap = (IPropertyMap) mapObject;
			Copy(propertyMap);
			DoDeepCopy(propertyMap);
		}

		public override bool DeepCompare(IMap compareTo)
		{
			if (!(Compare(compareTo)))
			{
				return false;
			}
			return true;
		}

		public override void DeepMerge(IMap mapObject)
		{
			Copy(mapObject);
		}

		public override void Copy(IMap mapObject)
		{
			IPropertyMap propertyMap = (IPropertyMap) mapObject;
			propertyMap.Name = this.Name;
			propertyMap.Column = this.Column;
			propertyMap.ValidateMethod = this.ValidateMethod;
			propertyMap.ValidationMode = this.ValidationMode ;
			propertyMap.DataType = this.DataType;
			propertyMap.DefaultValue = this.DefaultValue;
			propertyMap.FieldName = this.FieldName;
			propertyMap.IdColumn = this.IdColumn;
			propertyMap.IdentityIndex = this.IdentityIndex;
			propertyMap.IsKey = this.IsKey;
			propertyMap.KeyIndex = this.KeyIndex;
			propertyMap.IdentityGenerator = this.IdentityGenerator;
			propertyMap.InheritInverseMappings = this.InheritInverseMappings;
			propertyMap.Inverse = this.Inverse;
			propertyMap.IsCollection = this.IsCollection;
			propertyMap.IsNullable = this.IsNullable;
			propertyMap.IsAssignedBySource = this.IsAssignedBySource;
			propertyMap.MaxLength = this.MaxLength;
			propertyMap.MinLength = this.MinLength;
			propertyMap.MaxValue= this.MaxValue;
			propertyMap.MinValue= this.MinValue;
			propertyMap.IsIdentity = this.IsIdentity;
			propertyMap.IsReadOnly = this.IsReadOnly;
			propertyMap.IsSlave = this.IsSlave;
			propertyMap.ItemType = this.ItemType;
			propertyMap.LazyLoad = this.LazyLoad;
			propertyMap.NoInverseManagement = this.NoInverseManagement;
			propertyMap.NullSubstitute = this.NullSubstitute;
			propertyMap.Source = this.Source;
			propertyMap.Table = this.Table;
			propertyMap.ReferenceType = this.ReferenceType;
			propertyMap.ReferenceQualifier = this.ReferenceQualifier;
			propertyMap.AdditionalColumns = (ArrayList) this.AdditionalColumns.Clone();
			propertyMap.AdditionalIdColumns = (ArrayList) this.AdditionalIdColumns.Clone();
			propertyMap.Accessibility = this.Accessibility;
			propertyMap.FieldAccessibility = this.FieldAccessibility;
			propertyMap.DeleteOptimisticConcurrencyBehavior = this.DeleteOptimisticConcurrencyBehavior;
			propertyMap.UpdateOptimisticConcurrencyBehavior = this.UpdateOptimisticConcurrencyBehavior;
			propertyMap.OnCreateBehavior = this.OnCreateBehavior;
			propertyMap.OnPersistBehavior = this.OnPersistBehavior;
			propertyMap.OrderBy = this.OrderBy;
			propertyMap.PropertyModifier = this.PropertyModifier;
			propertyMap.MergeBehavior = this.MergeBehavior;
			propertyMap.RefreshBehavior = this.RefreshBehavior;
			propertyMap.CascadingCreate = this.CascadingCreate;
			propertyMap.CascadingDelete = this.CascadingDelete;
			propertyMap.TimeToLive = this.TimeToLive;
			propertyMap.TimeToLiveBehavior = this.TimeToLiveBehavior;
			propertyMap.DocSource = this.DocSource;
			propertyMap.DocAttribute = this.DocAttribute;
			propertyMap.DocElement= this.DocElement;
			propertyMap.DocPropertyMapMode = this.DocPropertyMapMode;
		}

		public override bool Compare(IMap compareTo)
		{
			if (compareTo == null)
			{
				return false;
			}
			IPropertyMap propertyMap = (IPropertyMap) compareTo;
			if (!(propertyMap.Column == this.Column))
			{
				return false;
			}
			if (!(propertyMap.ValidateMethod == this.ValidateMethod))
			{
				return false;
			}
			if (!(propertyMap.ValidationMode == this.ValidationMode))
			{
				return false;
			}
			if (!(propertyMap.DataType == this.DataType))
			{
				return false;
			}
			if (!(propertyMap.DefaultValue == this.DefaultValue))
			{
				return false;
			}
			if (!(propertyMap.FieldName == this.FieldName))
			{
				return false;
			}
			if (!(propertyMap.IdColumn == this.IdColumn))
			{
				return false;
			}
			if (!(propertyMap.IdentityIndex == this.IdentityIndex))
			{
				return false;
			}
			if (!(propertyMap.IsKey == this.IsKey))
			{
				return false;
			}
			if (!(propertyMap.KeyIndex == this.KeyIndex))
			{
				return false;
			}
			if (!(propertyMap.IdentityGenerator == this.IdentityGenerator))
			{
				return false;
			}
			if (!(propertyMap.IsNullable == this.IsNullable))
			{
				return false;
			}
			if (!(propertyMap.IsAssignedBySource == this.IsAssignedBySource))
			{
				return false;
			}
			if (!(propertyMap.MaxLength == this.MaxLength))
			{
				return false;
			}
			if (!(propertyMap.MinLength == this.MinLength))
			{
				return false;
			}
			if (!(propertyMap.MaxValue == this.MaxValue))
			{
				return false;
			}
			if (!(propertyMap.MinValue == this.MinValue))
			{
				return false;
			}
			if (!(propertyMap.InheritInverseMappings == this.InheritInverseMappings))
			{
				return false;
			}
			if (!(propertyMap.Inverse == this.Inverse))
			{
				return false;
			}
			if (!(propertyMap.IsCollection == this.IsCollection))
			{
				return false;
			}
			if (!(propertyMap.IsIdentity == this.IsIdentity))
			{
				return false;
			}
			if (!(propertyMap.IsReadOnly == this.IsReadOnly))
			{
				return false;
			}
			if (!(propertyMap.IsSlave == this.IsSlave))
			{
				return false;
			}
			if (!(propertyMap.ItemType == this.ItemType))
			{
				return false;
			}
			if (!(propertyMap.LazyLoad == this.LazyLoad))
			{
				return false;
			}
			if (!(propertyMap.Name == this.Name))
			{
				return false;
			}
			if (!(propertyMap.NoInverseManagement == this.NoInverseManagement))
			{
				return false;
			}
			if (!(propertyMap.NullSubstitute == this.NullSubstitute))
			{
				return false;
			}
			if (!(propertyMap.Source == this.Source))
			{
				return false;
			}
			if (!(propertyMap.Table == this.Table))
			{
				return false;
			}
			if (!(propertyMap.ReferenceType == this.ReferenceType))
			{
				return false;
			}
			if (!(propertyMap.ReferenceQualifier == this.ReferenceQualifier))
			{
				return false;
			}
			if (!(propertyMap.Accessibility == this.Accessibility))
			{
				return false;
			}
			if (!(propertyMap.FieldAccessibility == this.FieldAccessibility))
			{
				return false;
			}
			if (!(propertyMap.DeleteOptimisticConcurrencyBehavior == this.DeleteOptimisticConcurrencyBehavior))
			{
				return false;
			}
			if (!(propertyMap.UpdateOptimisticConcurrencyBehavior == this.UpdateOptimisticConcurrencyBehavior))
			{
				return false;
			}
			if (!(propertyMap.OnCreateBehavior == this.OnCreateBehavior))
			{
				return false;
			}
			if (!(propertyMap.OnPersistBehavior == this.OnPersistBehavior))
			{
				return false;
			}
			if (!(propertyMap.OrderBy == this.OrderBy))
			{
				return false;
			}
			if (!(propertyMap.PropertyModifier == this.PropertyModifier))
			{
				return false;
			}
			if (!(propertyMap.MergeBehavior == this.MergeBehavior))
			{
				return false;
			}
			if (!(propertyMap.RefreshBehavior == this.RefreshBehavior))
			{
				return false;
			}
			if (!(propertyMap.CascadingCreate == this.CascadingCreate))
			{
				return false;
			}
			if (!(propertyMap.CascadingDelete == this.CascadingDelete))
			{
				return false;
			}
			if (!(propertyMap.TimeToLive == this.TimeToLive))
			{
				return false;
			}
			if (!(propertyMap.TimeToLiveBehavior == this.TimeToLiveBehavior))
			{
				return false;
			}
			if (!(propertyMap.DocSource == this.DocSource))
			{
				return false;
			}
			if (!(propertyMap.DocAttribute == this.DocAttribute))
			{
				return false;
			}
			if (!(propertyMap.DocElement == this.DocElement))
			{
				return false;
			}
			if (!(propertyMap.DocPropertyMapMode == this.DocPropertyMapMode))
			{
				return false;
			}
			if (!(CompareArrayLists(propertyMap.AdditionalColumns, this.AdditionalColumns)))
			{
				return false;
			}
			if (!(CompareArrayLists(propertyMap.AdditionalIdColumns, this.AdditionalIdColumns)))
			{
				return false;
			}
			return true;
		}

		#endregion

		#region IMap

		public override string GetKey()
		{
			return m_ClassMap.DomainMap.Name + "." + m_ClassMap.Name + "." + this.Name;
		}

		#endregion

		#region IFixate

		#endregion

	}
}