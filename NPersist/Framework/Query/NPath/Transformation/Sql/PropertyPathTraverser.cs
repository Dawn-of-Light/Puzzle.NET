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
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Exceptions;
using Puzzle.NPersist.Framework.Mapping;
using Puzzle.NPersist.Framework.Sql.Dom;

namespace Puzzle.NPersist.Framework.NPath.Sql
{
	public class PropertyPathTraverser
	{		
		public PropertyPathTraverser(SqlEmitter sqlEmitter)
		{
			this.sqlEmitter = sqlEmitter;

			joinTree = new JoinTree(this);

			joinTree.TableMap = sqlEmitter.RootClassMap.MustGetTableMap();

		}

		private SqlEmitter sqlEmitter;
		
		private JoinTree joinTree = null ;
		private Hashtable joinedNonPrimaries = new Hashtable() ;

		public JoinTree JoinTree
		{
			get { return this.joinTree; }
			set { this.joinTree = value; }
		}

		public SqlEmitter SqlEmitter
		{
			get { return this.sqlEmitter; }
			set { this.sqlEmitter = value; }
		}

		public virtual SqlTableAlias GetClassTable(string className)
		{
			IClassMap classMap = sqlEmitter.RootClassMap.DomainMap.MustGetClassMap(className);
			ITableMap tableMap = classMap.MustGetTableMap();

			return sqlEmitter.Select.GetSqlTableAlias(tableMap);						
		}

		
		public virtual SqlColumnAlias GetPropertyColumn(IPropertyMap propertyMap, object hash) 
		{	
			if (hash == null) { hash = propertyMap; }
			SqlTableAlias tbl = sqlEmitter.GetTableAlias(propertyMap.MustGetTableMap(), hash)  ;

            IColumnMap columnMap = propertyMap.GetColumnMap();

            return tbl.GetSqlColumnAlias(columnMap);
		}

		public virtual string GetPathParent(string path)
		{
			string[] pathNames = GetPathPropertyNames(path); 
			string pathParent = "";
			for (int i = 0; i < pathNames.Length -1; i++)
			{
				pathParent += pathNames[i];
				if (i < pathNames.Length - 2)
				{
					pathParent += ".";
				}
			}

			return pathParent;
		}

		public virtual void GetPropertyColumnNamesAndAliases(IPropertyMap propertyMap, object hash, Hashtable columns, ArrayList order, string path, string propPath, string suggestion)
		{	

			if (hash == null) { hash = propertyMap; }
			SqlTableAlias tbl = sqlEmitter.GetTableAlias(propertyMap.MustGetTableMap(), hash)  ;
			IList columnAliases = new ArrayList();

			if (propertyMap.IsIdentity)
			{
				IClassMap classMap = propertyMap.ClassMap;
				IColumnMap typeColumnMap = classMap.GetTypeColumnMap();
				if (typeColumnMap != null)
				{
					string pathParent = GetPathParent(path);
					SqlColumnAlias column = GetPropertyColumnAlias(tbl, pathParent + ".NPersistTypeColumn" , typeColumnMap, pathParent + ".NPersistTypeColumn");
					columnAliases.Add(column);
				}
			}

            if (suggestion == "")
                suggestion = propPath;
      //      bool hasTypeColumn = false;

            IPropertyMap inverse = propertyMap.GetInversePropertyMap();

            if (inverse != null)
            {
                foreach (IColumnMap columnMap in propertyMap.GetAllColumnMaps())
                {
                    IColumnMap inverseTypeColumnMap = inverse.ClassMap.GetTypeColumnMap();
                    if (inverseTypeColumnMap != null && inverseTypeColumnMap == columnMap.GetPrimaryKeyColumnMap())
                    {
                        string suggestionString;
                        suggestionString = propPath + ".NPersistTypeColumn";

                        SqlColumnAlias column = GetPropertyColumnAlias(tbl, path, columnMap, suggestionString);
                        columnAliases.Add(column);
                    }                
                }
            }

			foreach (IColumnMap columnMap in propertyMap.GetAllColumnMaps())
			{
                if (inverse != null) 
                {
                    IColumnMap inverseTypeColumnMap = inverse.ClassMap.GetTypeColumnMap();
                    if ( inverseTypeColumnMap != null && inverseTypeColumnMap == columnMap.GetPrimaryKeyColumnMap())
                    {
                        continue;
                    }
                }

                string suggestionString;

                suggestionString = suggestion;
                    
				SqlColumnAlias column = GetPropertyColumnAlias(tbl, path, columnMap, suggestionString);
				columnAliases.Add(column);
			}
 
			foreach (SqlColumnAlias column in columnAliases)
			{

				if (!(columns.ContainsKey(column)))
				{
					columns[column] = column;					
					order.Add(column);

					//Note: Important stuff, right here in nowhere-ville!!
					if (this.sqlEmitter.PropertyColumnMap.ContainsKey(propPath))
					{
						ArrayList arrAliases;
						if (this.sqlEmitter.PropertyColumnMap[propPath] is string)
						{
							arrAliases = new ArrayList() ;
							arrAliases.Add(this.sqlEmitter.PropertyColumnMap[propPath]);
							this.sqlEmitter.PropertyColumnMap[propPath] = arrAliases;
						}
						else
						{
							arrAliases = (ArrayList) this.sqlEmitter.PropertyColumnMap[propPath];
						}
						arrAliases.Add(column.Alias);
					}
					else
					{
						this.sqlEmitter.PropertyColumnMap[propPath] = column.Alias;						
					}				
				}
			}			
		}

		public virtual SqlTableAlias GetClassTableAlias(string className)
		{	
			IClassMap classMap = sqlEmitter.RootClassMap.DomainMap.MustGetClassMap(className);
			return sqlEmitter.GetTableAlias(classMap.MustGetTableMap(), classMap)  ;
		}

		public virtual SqlColumnAlias GetPropertyColumnAlias(SqlTableAlias tableAlias, string propertyPath, IColumnMap columnMap, string suggestion)
		{	
			return sqlEmitter.GetColumnAlias(tableAlias, columnMap, propertyPath, GetAliasSuggestionFromPropertyPath(propertyPath, suggestion))  ;
		}

		public virtual SqlColumnAlias GetPropertyColumnAlias(SqlTableAlias tableAlias, IPropertyMap propertyMap, string propertyPath, string suggestion)
		{	
			return sqlEmitter.GetColumnAlias(tableAlias, propertyMap.GetColumnMap(), propertyPath, GetAliasSuggestionFromPropertyPath(propertyPath, suggestion))  ;
		}

		public virtual string GetAliasSuggestionFromPropertyPath(string propertyPath, string suggestion)
		{
			if (suggestion.Length < 1)
				suggestion = propertyPath;
	
			return suggestion;
		}

		public ArrayList GetPathPropertyMaps(string propertyPath)
		{
			string special = "";
			return GetPathPropertyMaps(propertyPath, ref special);
		}

		public ArrayList GetPathPropertyMaps(string propertyPath, ref string special)
		{
			ArrayList propertyMaps = new ArrayList();
			IPropertyMap propertyMap = null;
			IClassMap classMap = sqlEmitter.RootClassMap;
			bool skip = false;
			bool first = true;
            int cnt = 1;
            IList pathPropertyNames = GetPathPropertyNames(propertyPath); 
			foreach (string name in pathPropertyNames)
			{
				if (name.Length > 0)
				{
					skip = false;
					if (first)
					{
						if (name.ToLower(CultureInfo.InvariantCulture) == sqlEmitter.RootClassMap.Name.ToLower())
						{
							skip = true;
						}					
					}
					first = false;
					if (!(skip))
					{
						if (!(name.Equals("*")))
						{
							propertyMap = classMap.MustGetPropertyMap(name);
							propertyMaps.Add(propertyMap);

                            if (cnt < pathPropertyNames.Count)
							    classMap = propertyMap.MustGetReferencedClassMap();					
						}
						else
						{
                            //HACK: hmmm * => ID ??
							special = name;
							//special - we trade for the first id property
							propertyMap = (IPropertyMap) classMap.GetIdentityPropertyMaps()[0];
							propertyMaps.Add(propertyMap);
						}
					}					
				}
                cnt++;
			}
			return propertyMaps;
		}


		public string[] GetPathPropertyNames(string propertyPath)
		{
			return propertyPath.Split('.');			
		}

		public virtual void TraverseSpan(string span, Hashtable selectedColumns, ArrayList columnOrder, string suggestion)
		{	
			string special = "";
			ArrayList propertyMaps = GetPathPropertyMaps(span, ref special);
			TraverseSingleSpan(propertyMaps, selectedColumns, columnOrder, special, suggestion);
		}

		private void TraverseSingleSpan(ArrayList propertyMaps, Hashtable selectedColumns, ArrayList columnOrder, string special, string suggestion)
		{
			IPropertyMap parentMap = null;
			string path = "";
			foreach (IPropertyMap pathMap in propertyMaps)
			{
				IPropertyMap propertyMap = pathMap;
				if (propertyMaps.IndexOf(propertyMap) < propertyMaps.Count - 1)
				{
					NPathQueryType queryType = this.sqlEmitter.npathQueryType;
					if (queryType == NPathQueryType.SelectObjects || queryType == NPathQueryType.SelectMixed)
					{
						if (path == "")
						{
							//GetPropertyColumnNamesAndAliases(propertyMap, propertyMap, selectedColumns, columnOrder, path, propertyMap.Name, "");
							GetPropertyColumnNamesAndAliases(propertyMap, propertyMap, selectedColumns, columnOrder, propertyMap.Name, propertyMap.Name, "");
						}
						else
						{
							GetPropertyColumnNamesAndAliases(propertyMap, path, selectedColumns, columnOrder, path, path + "." + propertyMap.Name, "");
						}
					}
					if (path.Length > 0) { path += "." ;}
					path += propertyMap.Name;
					JoinType joinType;
					if (HasNullableColumn(propertyMap) || propertyMap.IsCollection)
					{
						joinType = JoinType.OuterJoin;
					}
					else
					{
						joinType = JoinType.InnerJoin;						
					}
					joinTree.SetupJoin(propertyMap, parentMap, path, joinType);
				}
				else
				{
					if (special == "*")
					{

						IClassMap classMap = null;
						if (propertyMaps.Count == 1)
						{
							classMap = this.sqlEmitter.RootClassMap;							
						}
						else
						{
							//Should actually take refClassMap from previous prop in path!!
							classMap = propertyMap.ClassMap;
						}

						foreach (IPropertyMap iPropertyMap in classMap.GetAllPropertyMaps() )
						{
							if (!(iPropertyMap.ReferenceType != ReferenceType.None && iPropertyMap.AdditionalColumns.Count > 0))
							{
								if (!(iPropertyMap.IsCollection))
								{
                                    //This if should be removed some day when the "JoinNonPrimary()" call a bit further down
                                    //has been refined to handle nullable OneToOne slaves...
                                    if (!(iPropertyMap.ReferenceType == ReferenceType.OneToOne && iPropertyMap.IsSlave && HasNullableColumn(iPropertyMap)))
                                    {
									    if (iPropertyMap.Column.Length > 0)
									    {
										    //Exclude inverse property to property leading to this point in the path
										    bool isInverse = false;
										    if (parentMap != null)
										    {											
											    if (iPropertyMap.Inverse.Length > 0)
											    {
												    if (parentMap == iPropertyMap.GetInversePropertyMap())
												    {
													    isInverse = true;
												    }
											    }
										    }
										    if (!isInverse)
										    {
											    if (path == "")
											    {
												    GetPropertyColumnNamesAndAliases(iPropertyMap, iPropertyMap, selectedColumns, columnOrder, iPropertyMap.Name, iPropertyMap.Name, "");
											    }
											    else
											    {
												    //GetPropertyColumnNamesAndAliases(iPropertyMap, path, selectedColumns, columnOrder, path, path + "." + iPropertyMap.Name, "");
												    GetPropertyColumnNamesAndAliases(iPropertyMap, path, selectedColumns, columnOrder, path + "." + iPropertyMap.Name, path + "." + iPropertyMap.Name, suggestion);                                                
											    }
											    if (iPropertyMap.MustGetTableMap() != iPropertyMap.ClassMap.MustGetTableMap())
											    {
												    JoinNonPrimary(iPropertyMap);
											    }																							
										    }
									    }

                                    }
								}
							}
						}						
					}
					else
					{
						if (path == "")
						{
							GetPropertyColumnNamesAndAliases(propertyMap, propertyMap, selectedColumns, columnOrder, propertyMap.Name, propertyMap.Name, suggestion);
						}
						else
						{
							//GetPropertyColumnNamesAndAliases(propertyMap, path, selectedColumns, columnOrder, path, path + "." + propertyMap.Name, suggestion);
							GetPropertyColumnNamesAndAliases(propertyMap, path, selectedColumns, columnOrder, path + "." + propertyMap.Name, path + "." + propertyMap.Name, suggestion);
						}													
						if (propertyMap.MustGetTableMap() != propertyMap.ClassMap.MustGetTableMap())
						{
							JoinNonPrimary(propertyMap);
						}
						
					}
				}
				parentMap = propertyMap;
			}
		}

		private void JoinNonPrimary(IPropertyMap iPropertyMap)
		{
			foreach (IColumnMap idColumn in iPropertyMap.GetAllIdColumnMaps() )
			{				
				SqlTableAlias thisTableAlias =  this.sqlEmitter.Select.GetSqlTableAlias(idColumn.TableMap.Name);
				SqlColumnAlias thisColAlias =  thisTableAlias.GetSqlColumnAlias(idColumn.Name) ;

				SqlTableAlias parentTableAlias =  this.sqlEmitter.Select.GetSqlTableAlias(idColumn.PrimaryKeyTable);
				SqlColumnAlias parentColAlias =  parentTableAlias.GetSqlColumnAlias(idColumn.PrimaryKeyColumn) ;

				if (!joinedNonPrimaries.ContainsKey(thisColAlias))
				{
					if (!(joinedNonPrimaries[thisColAlias] == parentColAlias))
					{
						SqlSearchCondition search = this.sqlEmitter.Select.SqlWhereClause.GetNextSqlSearchCondition();
						search.GetSqlComparePredicate(parentColAlias, SqlCompareOperatorType.Equals, thisColAlias);												
						
						joinedNonPrimaries[thisColAlias] = parentColAlias;
					}					
				}
			}
		}


		public virtual SqlColumnAlias TraversePropertyPath(string propertyPath)
		{	
			IPropertyMap propertyMap = null;
			return TraversePropertyPath(propertyPath, ref propertyMap);
		}

		public virtual SqlColumnAlias TraversePropertyPath(string propertyPath, ref IPropertyMap propertyMap)
		{	
			SqlColumnAlias result = null;
			IPropertyMap parentMap = null;
			ArrayList propertyMaps = GetPathPropertyMaps(propertyPath);
			string path = "";
			foreach (IPropertyMap pathMap in propertyMaps)
			{
				propertyMap = pathMap;
				if (propertyMaps.IndexOf(propertyMap) < propertyMaps.Count - 1)
				{
					if (path.Length > 0) { path += "." ;}
					path += propertyMap.Name;
					joinTree.SetupJoin(propertyMap, parentMap, path);
				}
				else
				{
					if (path == "")
					{
						result = GetPropertyColumn(propertyMap, propertyMap);											
					}
					else
					{
						result = GetPropertyColumn(propertyMap, path);											
					}
				}
				parentMap = propertyMap;
			}

			return result;
		}

		public virtual SqlColumnAlias TraverseSimplePropertySpan(string propertyPath)
		{	
			IPropertyMap propertyMap = null;
			return TraverseSimplePropertySpan(propertyPath, ref propertyMap);
		}

		public virtual SqlColumnAlias TraverseSimplePropertySpan(string propertyPath, ref IPropertyMap propertyMap)
		{	
			SqlColumnAlias result = null;
			IPropertyMap parentMap = null;
			ArrayList propertyMaps = GetPathPropertyMaps(propertyPath);
			string path = "";
			JoinType joinType;
			foreach (IPropertyMap pathMap in propertyMaps)
			{
				propertyMap = pathMap;
				if (propertyMaps.IndexOf(propertyMap) < propertyMaps.Count - 1)
				{
					if (path.Length > 0) { path += "." ;}
					path += propertyMap.Name;

					if (HasNullableColumn(propertyMap))
					{
						joinType = JoinType.OuterJoin;
					}
					else
					{
						joinType = JoinType.InnerJoin;						
					}

					joinTree.SetupJoin(propertyMap, parentMap, path,joinType);
				}
				else
				{
					if (path == "")
					{
						result = GetPropertyColumn(propertyMap, propertyMap);											
					}
					else
					{
						result = GetPropertyColumn(propertyMap, path);											
					}
				}
				parentMap = propertyMap;
			}

			return result;
		}

		protected virtual bool HasNullableColumn(IPropertyMap propertyMap)
		{
			return propertyMap.GetIsNullable();
//			foreach (IColumnMap columnMap in propertyMap.GetAllColumnMaps())
//			{
//				if (columnMap.AllowNulls)
//				{
//					return true;
//				}				
//			}
//			return false;
		}
	}
}
