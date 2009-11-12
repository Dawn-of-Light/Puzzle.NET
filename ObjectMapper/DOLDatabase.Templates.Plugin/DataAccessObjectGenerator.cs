using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Mapping;

namespace DOLDatabase.Templates.Plugin
{
	/// <summary>
	/// Generates data access object files.
	/// </summary>
	public abstract class DataAccessObjectGenerator
	{
		/// <summary>
		/// Generates all the DAO files.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="domain">The domain.</param>
		public virtual void GenerateAll(string path, IDomainMap domain)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}


			foreach (IClassMap classMap in domain.ClassMaps)
			{
				GenerateDao(path, classMap);
			}
		}

		/// <summary>
		/// Generates the DAO file.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="classMap">The class map.</param>
		public virtual void GenerateDao(string path, IClassMap classMap)
		{
			using (StreamWriter file = new StreamWriter(path + GetDaoName(classMap) + ".cs", false))
			{
				
				string entityClassName = EntityGenerator.GetTypeName(classMap);

				ClassUtility.AppendHeader(file);
				InsertUsings(file);
				file.WriteLine();

				// namespace
				file.WriteLine("namespace " + Namespace);
				file.WriteLine("{");
				file.WriteLine("	public class " + GetDaoName(classMap) + " : " + DataAccessInterfaceGenerator.GetInterfaceName(classMap));
				file.WriteLine("	{");

				// state's object storage and other fields/methods
				GenerateCustomFieldsAndMethods(file, classMap);
				file.WriteLine();

				// find by primary key
				GenerateFind(file, classMap);

				// 'find by properties' methods, if any
				GenerateAllFindBy(file, classMap);

				// 'count by properties' methods, if any
				GenerateAllCountBy(file, classMap);

				// sql "insert"
				file.WriteLine("		public virtual void Create(ref " + entityClassName + " obj)");
				file.WriteLine("		{");
				GenerateCreate(file, classMap);
				file.WriteLine("		}");
				file.WriteLine();

				// sql "update"
				file.WriteLine("		public virtual void Update(" + entityClassName + " obj)");
				file.WriteLine("		{");
				GenerateUpdate(file, classMap);
				file.WriteLine("		}");
				file.WriteLine();

				// sql "delete"
				file.WriteLine("		public virtual void Delete(" + entityClassName + " obj)");
				file.WriteLine("		{");
				GenerateDelete(file, classMap);
				file.WriteLine("		}");
				file.WriteLine();

				// flush cache
				file.WriteLine("		public virtual void SaveAll()");
				file.WriteLine("		{");
				GenerateSaveAll(file, classMap);
				file.WriteLine("		}");
				file.WriteLine();
				
				// select all objects
				file.WriteLine("		public virtual IList<" + entityClassName + "> SelectAll()");
				file.WriteLine("		{");
				GenerateSelectAll(file, classMap);
				file.WriteLine("		}");
				file.WriteLine();
				
				// count all objects
				file.WriteLine("		public virtual long CountAll()");
				file.WriteLine("		{");
				GenerateCountAll(file, classMap);
				file.WriteLine("		}");
				file.WriteLine();

				// fill entity with DB row
				file.WriteLine("		protected virtual void FillEntityWithRow(ref " + entityClassName + " entity, MySqlDataReader reader)");
				file.WriteLine("		{");
				GenerateFillEntityWithRow(file, classMap);
				file.WriteLine("		}");
				file.WriteLine();

				// gets the TO's type
				file.WriteLine("		public virtual Type TransferObjectType");
				file.WriteLine("		{");
				file.WriteLine("			get {{ return typeof({0}); }}", entityClassName);
				file.WriteLine("		}");
				file.WriteLine();
				
				// very schema
				file.WriteLine("		public IList<string> VerifySchema()");
				file.WriteLine("		{");
				GenerateVerifySchema(file, classMap);
				file.WriteLine("		}");
				file.WriteLine();
				
				// constructor
				file.WriteLine("		public " + GetDaoName(classMap) + "(" + StateClassName + " state)");
				file.WriteLine("		{");
				file.WriteLine("			if (state == null)");
				file.WriteLine("			{");
				file.WriteLine("				throw new ArgumentNullException(\"state\");");
				file.WriteLine("			}");
				file.WriteLine("			m_state = state;");
				file.WriteLine("		}");
				
				file.WriteLine("	}");
				file.WriteLine("}");
			}
		}

		/// <summary>
		/// Inserts the usings.
		/// </summary>
		/// <param name="file">The file.</param>
		public virtual void InsertUsings(StreamWriter file)
		{
			file.WriteLine("using System.Collections.Generic;");
			file.WriteLine("using System.Data;");
			file.WriteLine("using DOL.Database.DataAccessInterfaces;");
			file.WriteLine("using DOL.Database.DataTransferObjects;");
		}

		/// <summary>
		/// Generates custom fields and methods of the class.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public virtual void GenerateCustomFieldsAndMethods(StreamWriter file, IClassMap classMap)
		{
			file.WriteLine("		protected readonly " + StateClassName + " m_state;");
		}

		/// <summary>
		/// Generates all the 'find by' methods.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		private void GenerateAllFindBy(StreamWriter file, IClassMap classMap)
		{
			// get all 'find by' groups
			foreach (KeyValuePair<int, IList<IPropertyMap>> pair in classMap.DOLGetFindByGroups())
			{
				IList<IPropertyMap> paramProps = pair.Value;
				string findBy = StringUtility.CombineObjects(paramProps, MapToStringConverters.PropertyAnd)
					.ToString();
				
				// method name
				file.Write("		public virtual IList<" + EntityGenerator.GetTypeName(classMap) + "> FindBy" + findBy);

				// method's params
				file.Write("(");
				bool first = true;
				foreach (IPropertyMap propertyMap in paramProps)
				{
					if (!first)
					{
						file.Write(", ");
					}

					// param type and name
					string paramName = ClassUtility.GetParamName(propertyMap);
					string paramType = ClassUtility.ConvertColumnTypeToCsType(propertyMap.GetColumnMap().DataType);
					file.Write(paramType + " " + paramName);
					first = false;
				}
				file.Write(")");

				file.WriteLine();
				file.WriteLine("		{");
				GenerateFindBy(file, paramProps, classMap);
				file.WriteLine("		}");
				
				file.WriteLine();
			}
		}

		/// <summary>
		/// Generates all the 'count by' methods.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		private void GenerateAllCountBy(StreamWriter file, IClassMap classMap)
		{
			// get all 'count by' groups
			foreach (KeyValuePair<int, IList<IPropertyMap>> pair in classMap.DOLGetFindByGroups())
			{
				IList<IPropertyMap> paramProps = pair.Value;
				string findBy = StringUtility.CombineObjects(paramProps, MapToStringConverters.PropertyAnd)
					.ToString();
				
				// method name
				file.Write("		public virtual long CountBy" + findBy);

				// method's params
				file.Write("(");
				bool first = true;
				foreach (IPropertyMap propertyMap in paramProps)
				{
					if (!first)
					{
						file.Write(", ");
					}

					// param type and name
					string paramName = ClassUtility.GetParamName(propertyMap);
					string paramType = ClassUtility.ConvertColumnTypeToCsType(propertyMap.GetColumnMap().DataType);
					file.Write(paramType + " " + paramName);
					first = false;
				}
				file.Write(")");

				file.WriteLine();
				file.WriteLine("		{");
				GenerateCountBy(file, paramProps, classMap);
				file.WriteLine("		}");

				file.WriteLine();
			}
		}

		/// <summary>
		/// Generates the 'verify schema' code.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public abstract void GenerateVerifySchema(StreamWriter file, IClassMap classMap);

		/// <summary>
		/// Gets the name of the DAO.
		/// </summary>
		/// <param name="classMap">The class map.</param>
		/// <returns></returns>
		public static string GetDaoName(IClassMap classMap)
		{
			return classMap.Name + "Dao";
		}

		/// <summary>
		/// Gets the namespace.
		/// </summary>
		/// <value>The namespace.</value>
		public abstract string Namespace { get; }

		/// <summary>
		/// Gets the name of the state class.
		/// </summary>
		/// <value>The name of the state class.</value>
		public abstract string StateClassName { get; }

		/// <summary>
		/// Generates the 'find' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public abstract void GenerateFind(StreamWriter file, IClassMap classMap);

		/// <summary>
		/// Generates the 'find by' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="propertyMaps">The property maps.</param>
		/// <param name="returnedClassMap">The returned class map.</param>
		public abstract void GenerateFindBy(StreamWriter file, IList<IPropertyMap> propertyMaps, IClassMap returnedClassMap);

		/// <summary>
		/// Generates the 'find by' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="propertyMaps">The property maps.</param>
		/// <param name="returnedClassMap">The returned class map.</param>
		public abstract void GenerateCountBy(StreamWriter file, IList<IPropertyMap> propertyMaps, IClassMap returnedClassMap);

		/// <summary>
		/// Generates the 'create' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public abstract void GenerateCreate(StreamWriter file, IClassMap classMap);

		/// <summary>
		/// Generates the 'update' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public abstract void GenerateUpdate(StreamWriter file, IClassMap classMap);

		/// <summary>
		/// Generates the 'delete' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public abstract void GenerateDelete(StreamWriter file, IClassMap classMap);

		/// <summary>
		/// Generates the 'save all' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public abstract void GenerateSaveAll(StreamWriter file, IClassMap classMap);

		/// <summary>
		/// Generates the 'select all' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public abstract void GenerateSelectAll(StreamWriter file, IClassMap classMap);

		/// <summary>
		/// Generates the 'count all' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		public abstract void GenerateCountAll(StreamWriter file, IClassMap classMap);

		/// <summary>
		/// Generates the 'fill entity with row' method.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="map">The map.</param>
		public abstract void GenerateFillEntityWithRow(StreamWriter file, IClassMap map);
	}
}
