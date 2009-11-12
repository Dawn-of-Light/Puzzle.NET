using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Puzzle.NPersist.Framework.Mapping;

namespace DOLDatabase.Templates.Plugin.MySql
{
	class MySqlDataAccessObjectGenerator : DataAccessObjectGenerator
	{
		/// <summary>
		/// Gets the namespace.
		/// </summary>
		/// <value>The namespace.</value>
		public override string Namespace
		{
			get { return MySqlConstants.DAO_NAMESPACE; }
		}

		/// <summary>
		/// Gets the name of the state class.
		/// </summary>
		/// <value>The name of the state class.</value>
		public override string StateClassName
		{
			get { return MySqlConstants.STATE_CLASS_NAME; }
		}

		/// <summary>
		/// Inserts the usings.
		/// </summary>
		/// <param name="file">The file.</param>
		public override void InsertUsings(StreamWriter file)
		{
			base.InsertUsings(file);
			file.WriteLine("using MySql.Data.MySqlClient;");
		}

		/// <summary>
		/// Generates custom fields and methods of the class.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateCustomFieldsAndMethods(StreamWriter file, IClassMap classMap)
		{
			ArrayList		allColumns = classMap.GetTableMap().ColumnMaps;
			IColumnMap[]	allColumnsTyped = (IColumnMap[]) allColumns.ToArray(typeof (IColumnMap));
			string			columnNames = StringUtility.CombineObjects(allColumnsTyped, MapToStringConverters.Columns).ToString();

			file.WriteLine("		protected static readonly string c_rowFields = \"" + columnNames + "\";");
			
			base.GenerateCustomFieldsAndMethods(file, classMap);
		}

		/// <summary>
		/// Generates the 'find' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateFind(StreamWriter file, IClassMap classMap)
		{
			ArrayList primaryColumns = classMap.GetTableMap().GetPrimaryKeyColumnMaps();

			StringBuilder whereQuery = new StringBuilder();
			StringBuilder findParams = new StringBuilder();
			bool first = true;

			foreach (IColumnMap primColumn in primaryColumns)
			{
				IPropertyMap propertyMap = classMap.GetPropertyMapForColumnMap(primColumn);

				if (!first)
				{
					whereQuery.Append(", ");
					findParams.Append(", ");
				}

				whereQuery
					.Append('`')
					.Append(primColumn.Name)
					.Append("`='\" + m_state.EscapeString(")
					.Append(ClassUtility.GetParamName(propertyMap))
					.Append(".ToString()) + \"'");

				string paramName = ClassUtility.GetParamName(propertyMap);
				string paramType = ClassUtility.ConvertColumnTypeToCsType(primColumn.DataType);
					
				findParams.Append(paramType + " " + paramName);

				first = false;
			}
			
			string entityClassName = EntityGenerator.GetTypeName(classMap);
			string sqlCommand = "SELECT \" + c_rowFields + \""
				+ " FROM `" + classMap.GetTableMap().Name + "`"
				+ " WHERE " + whereQuery.ToString();

			file.WriteLine("		public virtual " + entityClassName + " Find(" + findParams.ToString() + ")");
			file.WriteLine("		{");
			file.WriteLine("			" + entityClassName + " result = new " + entityClassName + "();");
			file.WriteLine("			string command = \"" + sqlCommand + "\";");
			file.WriteLine();
			file.WriteLine("			m_state.ExecuteQuery(");
			file.WriteLine("				command,");
			file.WriteLine("				CommandBehavior.SingleRow,");
			file.WriteLine("				delegate(MySqlDataReader reader)");
			file.WriteLine("				{");
			file.WriteLine("					if (!reader.Read())");
			file.WriteLine("					{");
			file.WriteLine("						result = null;");
			file.WriteLine("					}");
			file.WriteLine("					else");
			file.WriteLine("					{");
			file.WriteLine("						FillEntityWithRow(ref result, reader);");
			file.WriteLine("					}");
			file.WriteLine("				}");
			file.WriteLine("			);");
			file.WriteLine();
			file.WriteLine("			return result;");
			file.WriteLine("		}");
			file.WriteLine();
		}

		/// <summary>
		/// Generates the 'find by' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="propertyMaps">The property maps.</param>
		/// <param name="returnedClassMap">The returned class map.</param>
		public override void GenerateFindBy(StreamWriter file, IList<IPropertyMap> propertyMaps, IClassMap returnedClassMap)
		{
			ArrayList allColumns = returnedClassMap.GetTableMap().ColumnMaps;
			
			// build the 'where' statement
			bool first = true;
			StringBuilder whereQuery = new StringBuilder(16 * propertyMaps.Count);
			foreach (IPropertyMap propertyMap in propertyMaps)
			{
				if (!first)
				{
					whereQuery.Append(" AND ");
				}

				whereQuery
					.Append('`')
					.Append(propertyMap.GetColumnMap().Name)
					.Append("`='\" + m_state.EscapeString(")
					.Append(ClassUtility.GetParamName(propertyMap))
					.Append(".ToString()) + \"'");
				
				first = false;
			}

			string entityClassName = EntityGenerator.GetTypeName(returnedClassMap);
			string sqlCommand = "SELECT  \" + c_rowFields + \""
				+ " FROM `" + returnedClassMap.GetTableMap().Name + "`"
				+ " WHERE " + whereQuery.ToString();

			// create an array and store results
			file.WriteLine("			" + entityClassName + " entity;");
			file.WriteLine("			List<" + entityClassName + "> results = null;");
			file.WriteLine();
			file.WriteLine("			m_state.ExecuteQuery(");
			file.WriteLine("				\"" + sqlCommand + "\",");
			file.WriteLine("				CommandBehavior.Default,");
			file.WriteLine("				delegate(MySqlDataReader reader)");
			file.WriteLine("				{");
			file.WriteLine("					results = new List<" + entityClassName + ">(reader.FieldCount);");
			file.WriteLine("					while (reader.Read())");
			file.WriteLine("					{");
            file.WriteLine("						entity = new " + entityClassName + "();");
			file.WriteLine("						FillEntityWithRow(ref entity, reader);");
			file.WriteLine("						results.Add(entity);");
			file.WriteLine("					}");
			file.WriteLine("				}");
			file.WriteLine("			);");
			file.WriteLine();
			file.WriteLine("			return results;");
		}

		/// <summary>
		/// Generates the 'count by' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="propertyMaps">The property maps.</param>
		/// <param name="returnedClassMap">The returned class map.</param>
		public override void GenerateCountBy(StreamWriter file, IList<IPropertyMap> propertyMaps, IClassMap returnedClassMap)
		{
			ArrayList allColumns = returnedClassMap.GetTableMap().ColumnMaps;

			// build the 'where' statement
			bool first = true;
			StringBuilder whereQuery = new StringBuilder(16 * propertyMaps.Count);
			foreach (IPropertyMap propertyMap in propertyMaps)
			{
				if (!first)
				{
					whereQuery.Append(" AND ");
				}

				whereQuery
					.Append('`')
					.Append(propertyMap.GetColumnMap().Name)
					.Append("`='\" + m_state.EscapeString(")
					.Append(ClassUtility.GetParamName(propertyMap))
					.Append(".ToString()) + \"'");

				first = false;
			}

			string entityClassName = EntityGenerator.GetTypeName(returnedClassMap);
			string sqlCommand = "SELECT  count(*)"
				+ " FROM `" + returnedClassMap.GetTableMap().Name + "`"
				+ " WHERE " + whereQuery.ToString();

			// create an array and store results
			file.WriteLine();
			file.WriteLine("			return (long) m_state.ExecuteScalar(");
			file.WriteLine("				\"" + sqlCommand + "\");");
			file.WriteLine();
		}

		/// <summary>
		/// Generates the 'create' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateCreate(StreamWriter file, IClassMap classMap)
		{
			bool first = true;

			// create fields list
			StringBuilder valuesQuery = new StringBuilder(16 * classMap.PropertyMaps.Count);
			foreach (IPropertyMap propertyMap in classMap.PropertyMaps)
			{
				if (!first)
				{
					valuesQuery.Append(",");
				}

				valuesQuery.Append("'\" + m_state.EscapeString(obj." + propertyMap.Name + ".ToString()) + \"'");
				
				first = false;
			}

			string sqlCommand = "INSERT INTO `" + classMap.GetTableMap().Name + "`"
				+ " VALUES (" + valuesQuery + ");";
			
			// find auto_incremented values
			IPropertyMap autoIncrementProperty = null;
			foreach (IPropertyMap propertyMap in classMap.PropertyMaps)
			{
				IColumnMap columnMap = propertyMap.GetColumnMap();
				if (columnMap.IsAutoIncrease)
				{
					// just one auto_increment column
					autoIncrementProperty = propertyMap;
					break;
				}
			}

			file.WriteLine("			m_state.ExecuteNonQuery(");
			file.WriteLine("				\"" + sqlCommand + "\");");
			
			if (autoIncrementProperty != null)
			{
				file.WriteLine("			object insertedId = m_state.ExecuteScalar(\"SELECT LAST_INSERT_ID();\");");
				file.WriteLine("			obj." + autoIncrementProperty.Name + " = (" + ClassUtility.ConvertColumnTypeToCsType(autoIncrementProperty.GetColumnMap().DataType) + ") (long) insertedId;");
			}
		}

		/// <summary>
		/// Generates the 'update' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateUpdate(StreamWriter file, IClassMap classMap)
		{
			ArrayList primaryColumns = classMap.GetTableMap().GetPrimaryKeyColumnMaps();
			
			bool first = true;
			StringBuilder whereQuery = new StringBuilder(16 * primaryColumns.Count);
			foreach (IColumnMap columnMap in primaryColumns)
			{
				IPropertyMap propertyMap = classMap.GetPropertyMapForColumnMap(columnMap);
				
				if (!first)
				{
					whereQuery.Append("AND ");
				}

				whereQuery
					.Append('`')
					.Append(columnMap.Name)
					.Append("`='\" + m_state.EscapeString(")
					.Append("obj." + propertyMap.Name)
					.Append(".ToString()) + \"'");

				first = false;
			}

			first = true;
			StringBuilder setQuery = new StringBuilder(16 * classMap.PropertyMaps.Count);
			
			foreach (IPropertyMap propertyMap in classMap.PropertyMaps)
			{
				if (!first)
				{
					setQuery.Append(", ");
				}

				setQuery
					.Append('`')
					.Append(propertyMap.GetColumnMap().Name)
					.Append("`='\" + m_state.EscapeString(")
					.Append("obj." + propertyMap.Name)
					.Append(".ToString()) + \"'");

				first = false;
			}

			string sqlCommand = "UPDATE `" + classMap.GetTableMap().Name + "`"
				+ " SET " + setQuery.ToString()
				+ " WHERE " + whereQuery.ToString();
			
			file.WriteLine("			m_state.ExecuteNonQuery(");
			file.WriteLine("				\"" + sqlCommand + "\");");
		}

		/// <summary>
		/// Generates the 'delete' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateDelete(StreamWriter file, IClassMap classMap)
		{
			ArrayList primaryColumns = classMap.GetTableMap().GetPrimaryKeyColumnMaps();

			bool first = true;
			StringBuilder whereQuery = new StringBuilder(16 * primaryColumns.Count);

			foreach (IColumnMap columnMap in primaryColumns)
			{
				IPropertyMap propertyMap = classMap.GetPropertyMapForColumnMap(columnMap);
				
				if (!first)
				{
					whereQuery.Append("AND ");
				}

				whereQuery
					.Append('`')
					.Append(columnMap.Name)
					.Append("`='\" + m_state.EscapeString(")
					.Append("obj." + propertyMap.Name)
					.Append(".ToString()) + \"'");

				first = false;
			}

			string sqlCommand = "DELETE FROM `" + classMap.GetTableMap().Name + "`"
				+ " WHERE " + whereQuery.ToString();

			file.WriteLine("			m_state.ExecuteNonQuery(");
			file.WriteLine("				\"" + sqlCommand + "\");");
		}

		/// <summary>
		/// Generates the 'save all' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateSaveAll(StreamWriter file, IClassMap classMap)
		{
			file.WriteLine("			// not used by this implementation");
		}

		/// <summary>
		/// Generates the 'select all' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateSelectAll(StreamWriter file, IClassMap classMap)
		{
			string sqlCommand = "SELECT \" + c_rowFields + \" FROM `" + classMap.GetTableMap().Name + "`";
			string entityClassName = EntityGenerator.GetTypeName(classMap);

			// create an array and store results
			file.WriteLine("			" + entityClassName + " entity;");
			file.WriteLine("			List<" + entityClassName + "> results = null;");
			file.WriteLine();
			file.WriteLine("			m_state.ExecuteQuery(");
			file.WriteLine("				\"" + sqlCommand + "\",");
			file.WriteLine("				CommandBehavior.Default,");
			file.WriteLine("				delegate(MySqlDataReader reader)");
			file.WriteLine("				{");
			file.WriteLine("					results = new List<" + entityClassName + ">();");
			file.WriteLine("					while (reader.Read())");
			file.WriteLine("					{");
			file.WriteLine("						entity = new " + entityClassName + "();");
			file.WriteLine("						FillEntityWithRow(ref entity, reader);");
			file.WriteLine("						results.Add(entity);");
			file.WriteLine("					}");
			file.WriteLine("				}");
			file.WriteLine("			);");
			file.WriteLine();
			file.WriteLine("			return results;");
		}

		/// <summary>
		/// Generates the 'count all' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateCountAll(StreamWriter file, IClassMap classMap)
		{
			string sqlCommand = "SELECT COUNT(*) FROM `" + classMap.GetTableMap().Name + "`";

			file.WriteLine("			return (long) m_state.ExecuteScalar(\"" + sqlCommand + "\");");
		}

		/// <summary>
		/// Generates the 'verify schema' code.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public override void GenerateVerifySchema(StreamWriter file, IClassMap classMap)
		{
#warning TODO: finish VerifySchema method
			if (classMap.PropertyMaps.Count <= 0)
			{
				file.WriteLine("			return null; // no property maps");
				return; // ???
			}
			
			// no table - create it
			bool firstColumn = true;
			StringBuilder sqlCommandText = new StringBuilder("\"CREATE TABLE IF NOT EXISTS `" + classMap.Table + "` (\"", classMap.PropertyMaps.Count * 16);
			foreach (IPropertyMap propertyMap in classMap.PropertyMaps)
			{
				IColumnMap columnMap = propertyMap.GetColumnMap();

				// close prev statement's quote, start new line
				if (firstColumn)
				{
					sqlCommandText.Append("\n");
					firstColumn = false;
				}
				else
				{
					sqlCommandText.Append(",\"\n");
				}

				// opening quote
				sqlCommandText.Append("				+\"");

				
				// build one field creation statement
				sqlCommandText
					.Append("`")
					.Append(columnMap.Name)
					.Append("` ")
					.Append(MySqlUtility.GetDataType(columnMap));

				// is nulls allowed
				if (!columnMap.AllowNulls)
				{
					sqlCommandText.Append(" NOT NULL");
				}

				// default value
				if (columnMap.DefaultValue != null && columnMap.DefaultValue.Length > 0)
				{
					sqlCommandText
						.Append(" default '")
						.Append(MySqlUtility.Escape(columnMap.DefaultValue))
						.Append("'");
				}
				
				// auto_increment?
				if (columnMap.IsAutoIncrease)
				{
					sqlCommandText.Append(" auto_increment");
				}
			}

			// close last statement's quotes, start new line
			sqlCommandText.Append("\"\n");

			// primary keys
			ArrayList		primaryKeyMaps = classMap.GetTableMap().GetPrimaryKeyColumnMaps();
			if (primaryKeyMaps.Count > 0)
			{
				// opening quote
				sqlCommandText.Append("				+\"");
				
				// write primary key name and open fields list
				string pkName = StringUtility.CombineObjects((IColumnMap[])primaryKeyMaps.ToArray(typeof(IColumnMap)), MapToStringConverters.Join).ToString();
				sqlCommandText.Append(", primary key `" + pkName + "`");

				// combine primary key fields
				IColumnMap[]	primaryKeyMapsTyped = (IColumnMap[])primaryKeyMaps.ToArray(typeof(IColumnMap));
				StringBuilder	primaryKeyMapNames = StringUtility.CombineObjects(primaryKeyMapsTyped, MapToStringConverters.Columns);

				sqlCommandText.Append(" (");
				sqlCommandText.Append(primaryKeyMapNames);
				sqlCommandText.Append(")\"\n");
			}
			else
			{
				// no primary
//				sqlCommandText.Append("\n");
			}
			
			// close the field list
			sqlCommandText.Append("				+\")\"");

			
			
			// write to file
			file.WriteLine("			m_state.ExecuteNonQuery(" + sqlCommandText.ToString() + "\n			);");
			file.WriteLine("			m_state.ExecuteNonQuery(\"OPTIMIZE TABLE `" + classMap.GetTableMap().Name + "`\");");
			file.WriteLine("			return null;");
		}

		/// <summary>
		/// Generates the 'fill entity with row' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="map">The map.</param>
		public override void GenerateFillEntityWithRow(StreamWriter file, IClassMap map)
		{
//			string entityTypeName = EntityGenerator.GetTypeName(map);
			
//			file.WriteLine("			" + entityTypeName + " entity = new " + entityTypeName + "();");

			int columnIndex = 0;
			foreach (IPropertyMap propertyMap in map.PropertyMaps)
			{
				file.Write("			entity." + propertyMap.Name);
				file.Write(" = reader." + MySqlUtility.GetReaderMethod(propertyMap.GetColumnMap().DataType));
				file.Write("(" + columnIndex.ToString() + ");");
				file.WriteLine();
				++columnIndex;
			}
			
//			file.WriteLine("			return entity;");
		}
	}
}
