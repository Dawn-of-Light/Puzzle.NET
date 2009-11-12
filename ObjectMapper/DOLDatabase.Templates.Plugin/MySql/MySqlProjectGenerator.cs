using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Puzzle.NPersist.Framework.Mapping;
using Puzzle.ObjectMapper.GUI.ProjectModel;
using Puzzle.ObjectMapper.Plugin;

namespace DOLDatabase.Templates.Plugin.MySql
{
	[PluginClass("DOLDatabase", "DOLDatabase MySql")]
	public class MySqlProjectGenerator
	{
		/// <summary>
		/// Generates all the MySql DAOs.
		/// </summary>
		/// <param name="domMap">The domain map.</param>
		[PluginMethod(typeof(IDomainMap), null, "Generate MySql DAOs")]
		public static void GenerateDaos(IDomainMap domMap)
		{
			if (Directory.Exists(MySqlConstants.BASE_PATH))
			{
				Directory.Delete(MySqlConstants.BASE_PATH, true);
			}
			Directory.CreateDirectory(MySqlConstants.BASE_PATH);


			DataAccessInterfaceGenerator.GenerateAll(MySqlConstants.BASE_PATH, domMap);
			EntityGenerator.GenerateAll(MySqlConstants.BASE_PATH, domMap);

			MySqlDataAccessObjectGenerator gen = new MySqlDataAccessObjectGenerator();
			gen.GenerateAll(MySqlConstants.BASE_PATH + "MySqlDAO\\", domMap);
		}

		/// <summary>
		/// Generates the XML config.
		/// </summary>
		/// <param name="domMap">The domain map.</param>
		[PluginMethod(typeof(IDomainMap), null, "Generate XML config")]
		public static void GenerateXmlConfig(IDomainMap domMap)
		{
			// create directory if doesn't exist
			if (!Directory.Exists(MySqlConstants.BASE_PATH))
			{
				Directory.CreateDirectory(MySqlConstants.BASE_PATH);
			}

			// create XML document
			XmlDocument		xmlDoc = new XmlDocument();
			XmlDeclaration	declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
			XmlElement		rootNode = xmlDoc.CreateElement("DatabaseMgrConfiguration");
			XmlAttribute	attribute;
			XmlNode			node;

			// xml header
			xmlDoc.InsertBefore(declaration, xmlDoc.DocumentElement);
			xmlDoc.AppendChild(rootNode);
			
			// database mgr
			node			= xmlDoc.CreateElement("IDatabaseMgr");
			// type
			attribute		= xmlDoc.CreateAttribute("type");
			attribute.Value	= MySqlConstants.DATABASE_MGR;
			node.Attributes.Append(attribute);
			rootNode.AppendChild(node);
			
			// state
			node			= xmlDoc.CreateElement("State");
			// connection string
			attribute		= xmlDoc.CreateAttribute("connectionString");
			attribute.Value	= MySqlConstants.CONNECTION_STRING;
			node.Attributes.Append(attribute);
			// type
			attribute		= xmlDoc.CreateAttribute("type");
			attribute.Value	= MySqlConstants.ASSEMBLY_NAMESPACE + "." + MySqlConstants.STATE_CLASS_NAME + ", " + MySqlConstants.ASSEMBLY_NAME;
			node.Attributes.Append(attribute);
			rootNode.AppendChild(node);
			
			// all mappings
			foreach (IClassMap classMap in domMap.ClassMaps)
			{
				node			= xmlDoc.CreateElement("Register");
				
				// interface
				attribute		= xmlDoc.CreateAttribute("interface");
				attribute.Value	=	DatabaseConstants.DAI_NAMESPACE +
									"." +
									DataAccessInterfaceGenerator.GetInterfaceName(classMap) +
									", " +
									DatabaseConstants.ASSEMBLY_NAME;
				node.Attributes.Append(attribute);
				
				// dao
				attribute		= xmlDoc.CreateAttribute("dao");
				attribute.Value = MySqlConstants.DAO_NAMESPACE + "." + DataAccessObjectGenerator.GetDaoName(classMap) + ", " + MySqlConstants.ASSEMBLY_NAME;
				node.Attributes.Append(attribute);
				
				rootNode.AppendChild(node);
			}

			xmlDoc.Save(MySqlConstants.BASE_PATH + MySqlConstants.XML_CONFIG_NAME);
		}

//		[PluginMethod(typeof(IProject), typeof(string), "Generate MySql IProject")]
//		public static string GenerateProject(IProject project)
//		{
//			return "project: " + project.Name;
//		}
//
//		[PluginMethod(typeof(IClassMap), typeof(string), "Generate MySql IClassMap")]
//		public static string GenerateClass(IClassMap classMap)
//		{
//			return "class: " + classMap.Name;
//		}
//
//		[PluginMethod(typeof(IPropertyMap), typeof(string), "Generate MySql IPropertyMap")]
//		public static string GenerateProperty(IPropertyMap property)
//		{
//			return "property: " + property.Name;
//		}

	}
}
