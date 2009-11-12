using System;
using System.Collections.Generic;
using System.Text;

namespace DOLDatabase.Templates.Plugin.MySql
{
	/// <summary>
	/// All the MySql constants.
	/// </summary>
	class MySqlConstants
	{
		// read path from config?
		public static readonly string BASE_PATH				= ".\\generated\\";
		public static readonly string XML_CONFIG_NAME		= "DatabaseMgrConfig.xml";
		public static readonly string CONNECTION_STRING		= "Server=localhost; Database=nhibernate; User ID=root; Password=root";

		public static readonly string MANAGER_CLASS_NAME	= "DatabaseMgr";
		public static readonly string STATE_CLASS_NAME		= "MySqlState";

		public static readonly string ASSEMBLY_NAME			= DatabaseConstants.ASSEMBLY_NAME + ".MySql";
		public static readonly string ASSEMBLY_NAMESPACE	= DatabaseConstants.NAMESPACE + ".MySql";
		public static readonly string DAO_NAMESPACE			= ASSEMBLY_NAMESPACE + ".DataAccessObjects";
		public static readonly string DATABASE_MGR			= DatabaseConstants.NAMESPACE + "." + MANAGER_CLASS_NAME + ", " + DatabaseConstants.ASSEMBLY_NAME;
	}
}
