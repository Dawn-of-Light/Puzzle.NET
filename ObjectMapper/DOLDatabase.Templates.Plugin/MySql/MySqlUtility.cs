using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Puzzle.NPersist.Framework.Mapping;

namespace DOLDatabase.Templates.Plugin.MySql
{
	/// <summary>
	/// Various MySql-specific methods.
	/// </summary>
	public class MySqlUtility
	{
		/// <summary>
		/// Converts database type to C# type.
		/// </summary>
		/// <param name="columnMap">The column map.</param>
		/// <returns>The C# type.</returns>
		public static string GetDataType(IColumnMap columnMap)
		{
			switch (columnMap.DataType)
			{
				case DbType.AnsiString:
					return "varchar(" + columnMap.Length + ") character set latin1";
				case DbType.AnsiStringFixedLength:
					return "char(" + columnMap.Length + ") character set latin1";
				case DbType.Binary:
					return "blob(" + columnMap.Length + ")";
				case DbType.Boolean:
					return "bit";
				case DbType.Byte:
					return "tinyint unsigned";
				case DbType.Date:
					return "date";
				case DbType.DateTime:
					return "datetime";
				case DbType.Decimal:
					return "decimal";
				case DbType.Double:
					return "double";
				case DbType.Int16:
					return "smallint";
				case DbType.Int32:
					return "int";
				case DbType.Int64:
					return "bigint";
				case DbType.SByte:
					return "tinyint";
				case DbType.Single:
					return "float";
				case DbType.String:
					return "varchar(" + columnMap.Length + ") character set utf8";
				case DbType.StringFixedLength:
					return "char(" + columnMap.Length + ") character set utf8";
				case DbType.Time:
					return "timespan";
				case DbType.UInt16:
					return "smallint unsigned";
				case DbType.UInt32:
					return "int unsigned";
				case DbType.UInt64:
					return "bigint unsigned";
				default:
					throw new ArgumentException("unknown db data type: " + columnMap.DataType.ToString(), "columnMap");
			}
		}

		public static string GetReaderMethod(DbType dbType)
		{
			switch (dbType)
			{
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.Guid:
				case DbType.String:
				case DbType.StringFixedLength:
					return "GetString";
//				case DbType.Binary: // not implemented
//					return "byte[]";
				case DbType.Boolean:
					return "GetBoolean";
				case DbType.Byte:
					return "GetByte";
				case DbType.Date:
				case DbType.Time:
				case DbType.DateTime:
					return "GetDateTime";
				case DbType.Decimal:
					return "GetDecimal";
				case DbType.Double:
					return "GetDouble";
				case DbType.Int16:
					return "GetInt16";
				case DbType.Int32:
					return "GetInt32";
				case DbType.Int64:
					return "GetInt64";
//				case DbType.Object:
//					return "object";
//				case DbType.SByte: // have to cast byte?
//					return "sbyte";
				case DbType.Single:
					return "GetFloat";
				case DbType.UInt16:
					return "GetUInt16";
				case DbType.UInt32:
					return "GetUInt32";
				case DbType.UInt64:
					return "GetUInt64";
				default:
					throw new Exception("unknown db field type: " + dbType.ToString());
			}
		}

		/// <summary>
		/// Escapes the specified value string for use in MySQL queries.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The escaped string.</returns>
		public static string Escape(string value)
		{
			return value;
		}
	}
}
