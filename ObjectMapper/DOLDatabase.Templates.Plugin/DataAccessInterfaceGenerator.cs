using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Mapping;

namespace DOLDatabase.Templates.Plugin
{
	class DataAccessInterfaceGenerator
	{
		/// <summary>
		/// Generates all the DAO interface files.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="domain">The domain.</param>
		public static void GenerateAll(string path, IDomainMap domain)
		{
			path += "DataAccessInterfaces\\";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			WriteGenericDao(path, domain);
			
			foreach (IClassMap classMap in domain.ClassMaps)
			{
				GenerateDai(path, classMap);
			}
		}
		
		/// <summary>
		/// Generates the DAO interface file.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="classMap">The class map.</param>
		private static void GenerateDai(string path, IClassMap classMap)
		{
			
			using (StreamWriter file = new StreamWriter(path + GetInterfaceName(classMap) + ".cs", false))
			{
				ClassUtility.AppendHeader(file);
				
				file.WriteLine("using System.Collections.Generic;");
				file.WriteLine("using DOL.Database.DataTransferObjects;");
				file.WriteLine();

				file.WriteLine("namespace DOL.Database.DataAccessInterfaces");
				file.WriteLine("{");
				file.WriteLine("	public interface " + GetInterfaceName(classMap) + " : IGenericDao<" + EntityGenerator.GetTypeName(classMap) + ">");
				file.WriteLine("	{");

				// generate 'find' methods
				GenerateFindMethods(classMap, file);

				// generate 'find by' methods
				GenerateFindByMethods(classMap, file);
				
				// generate 'cout by' methods
				GenerateCountByMethods(classMap, file);

				file.WriteLine("	}");
				file.WriteLine("}");
			}
		}

		/// <summary>
		/// Generates all the 'find by' methods of this interface.
		/// </summary>
		/// <param name="classMap">The class map.</param>
		/// <param name="file">The file.</param>
		private static void GenerateFindByMethods(IClassMap classMap, StreamWriter file)
		{
			foreach (KeyValuePair<int, IList<IPropertyMap>> pair in classMap.DOLGetFindByGroups())
			{
				IList<IPropertyMap> paramProps = pair.Value;
				
				string findBy = StringUtility.CombineObjects(paramProps, MapToStringConverters.PropertyAnd)
					.ToString();
				
				// method name
				file.Write("		IList<" + EntityGenerator.GetTypeName(classMap) + "> FindBy" + findBy);
				
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
				file.Write(");");
					
				file.WriteLine();

			}
		}

		/// <summary>
		/// Generates all the 'count by' methods of this interface.
		/// </summary>
		/// <param name="classMap">The class map.</param>
		/// <param name="file">The file.</param>
		private static void GenerateCountByMethods(IClassMap classMap, StreamWriter file)
		{
			foreach (KeyValuePair<int, IList<IPropertyMap>> pair in classMap.DOLGetFindByGroups())
			{
				IList<IPropertyMap> paramProps = pair.Value;
				string findBy = StringUtility.CombineObjects(paramProps, MapToStringConverters.PropertyAnd)
					.ToString();
				
				// method name
				file.Write("		long CountBy" + findBy);

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
				file.Write(");");

				file.WriteLine();

			}
		}

		/// <summary>
		/// Generates all the 'find' methods of this interface.
		/// </summary>
		/// <param name="classMap">The class map.</param>
		/// <param name="file">The file.</param>
		private static void GenerateFindMethods (IClassMap classMap, StreamWriter file)
		{				
			// method name
			file.Write("		" + EntityGenerator.GetTypeName(classMap) + " Find(");
			ArrayList primaryColumns = classMap.GetTableMap().GetPrimaryKeyColumnMaps();

			// method's params
			bool first = true;
			foreach (IColumnMap columnMap in primaryColumns)
			{
				IPropertyMap propertyMap = classMap.GetPropertyMapForColumnMap(columnMap);

				if (!first)
				{
					file.Write(", ");
				}

				// param type and name
				string paramName = ClassUtility.GetParamName(propertyMap);
				string paramType = ClassUtility.ConvertColumnTypeToCsType(columnMap.DataType);
				file.Write(paramType + " " + paramName);
				first = false;

			}
			file.Write(");");

			file.WriteLine();
		}

		/// <summary>
		/// Writes the generic DAO.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="domain">The domain.</param>
		private static void WriteGenericDao(string path, IDomainMap domain)
		{
			string fileName = path + "IGenericDao.cs";
			
			using (StreamWriter file = new StreamWriter(fileName, false))
			{
				ClassUtility.AppendHeader(file);

                file.WriteLine("using System.Collections.Generic;");
				file.WriteLine();
				file.WriteLine("namespace DOL.Database.DataAccessInterfaces");
				file.WriteLine("{");
				file.WriteLine("	/// <summary>");
				file.WriteLine("	/// Generic interface with base DAO methods.");
				file.WriteLine("	/// </summary>");
				file.WriteLine("	/// <typeparam name=\"TTransferObject\">The transfer object's type.</typeparam>");
				file.WriteLine("	public interface IGenericDao<TTransferObject> : IDataAccessObject");
				file.WriteLine("	{");
				file.WriteLine();
				file.WriteLine("		/// <summary>");
				file.WriteLine("		/// Creates an object in a database.");
				file.WriteLine("		/// </summary>");
				file.WriteLine("		/// <param name=\"obj\">The object to save.</param>");
				file.WriteLine("		void Create(ref TTransferObject obj);");
				file.WriteLine();
				file.WriteLine("		/// <summary>");
				file.WriteLine("		/// Updates the persistent instance with data from transfer object.");
				file.WriteLine("		/// </summary>");
				file.WriteLine("		/// <param name=\"obj\">The data.</param>");
				file.WriteLine("		void Update(TTransferObject obj);");
				file.WriteLine();
				file.WriteLine("		/// <summary>");
				file.WriteLine("		/// Deletes an object from a database.");
				file.WriteLine("		/// </summary>");
				file.WriteLine("		/// <param name=\"obj\">The object to delete.</param>");
				file.WriteLine("		void Delete(TTransferObject obj);");
                file.WriteLine();
				file.WriteLine("		/// <summary>");
				file.WriteLine("		/// Gets all stored objects.");
				file.WriteLine("		/// </summary>");
				file.WriteLine("		/// <value>all objects.</value>");
                file.WriteLine("		IList<TTransferObject> SelectAll();");
				file.WriteLine();
				file.WriteLine("		/// <summary>");
				file.WriteLine("		/// Gets the count of all stored objects.");
				file.WriteLine("		/// </summary>");
				file.WriteLine("		/// <value>The count of all objects.</value>");
				file.WriteLine("		long CountAll();");
				file.WriteLine("	}");
				file.WriteLine("}");
				file.WriteLine();
			}
		}

		/// <summary>
		/// Gets the name of the interface.
		/// </summary>
		/// <param name="classMap">The class map.</param>
		/// <returns></returns>
		public static string GetInterfaceName(IClassMap classMap)
		{
			return "I" + classMap.Name + "Dao";
		}
	}
}
