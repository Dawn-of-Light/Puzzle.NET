using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Puzzle.NPersist.Framework.Mapping;

namespace DOLDatabase.Templates.Plugin
{
	/// <summary>
	/// Generates entity files.
	/// </summary>
	class EntityGenerator
	{
		/// <summary>
		/// Generates all entity files.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="domain">The domain.</param>
		public static void GenerateAll(string path, IDomainMap domain)
		{
			path += "Entities\\";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			
			
			foreach (IClassMap classMap in domain.ClassMaps)
			{
				GenerateEntity(path, classMap);
			}
		}

		/// <summary>
		/// Generates one entity file.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="classMap">The class map.</param>
		private static void GenerateEntity(string path, IClassMap classMap)
		{
			using (StreamWriter file = new StreamWriter(path + GetTypeName(classMap) + ".cs", false))
			{
				ClassUtility.AppendHeader(file);
				file.WriteLine();

				file.WriteLine("namespace DOL.Database.DataTransferObjects");
				file.WriteLine("{");
				file.WriteLine("	[Serializable]");
				file.WriteLine("	public class " + GetTypeName(classMap));
				file.WriteLine("	{");
				
				GeneratePrivateFields(file, classMap);
				
				file.WriteLine();
				
				GenerateProperties(file, classMap);
				
				file.WriteLine("	}");
				file.WriteLine("}");
			}
		}

		/// <summary>
		/// Generates the public access properties.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		private static void GenerateProperties(StreamWriter file, IClassMap classMap)
		{
			foreach (IPropertyMap property in classMap.PropertyMaps)
			{
				ClassUtility.GenerateProperty(file, property, false);
			}
		}

		/// <summary>
		/// Generates the private fields of the entity.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="classMap">The class map.</param>
		private static void GeneratePrivateFields(StreamWriter file, IClassMap classMap)
		{
			foreach (IPropertyMap property in classMap.PropertyMaps)
			{
				ClassUtility.GenerateField(file, property);
			}
		}

		/// <summary>
		/// Gets the name of the entity class.
		/// </summary>
		/// <param name="classMap">The class map.</param>
		/// <returns></returns>
		public static string GetTypeName(IClassMap classMap)
		{
			return classMap.Name + "Entity";
		}
	}
}

