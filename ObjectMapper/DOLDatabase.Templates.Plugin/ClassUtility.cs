using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Puzzle.NPersist.Framework.Enumerations;
using Puzzle.NPersist.Framework.Mapping;

namespace DOLDatabase.Templates.Plugin
{
	/// <summary>
	/// Generic class methods, usabe by every generator.
	/// </summary>
	class ClassUtility
	{
		/// <summary>
		/// Appends a header to the file.
		/// </summary>
		/// <param name="file">The file.</param>
		public static void AppendHeader(StreamWriter file)
		{
			file.WriteLine("/*");
			file.WriteLine(" * DAWN OF LIGHT - The first free open source DAoC server emulator");
			file.WriteLine(" *");
			file.WriteLine(" * This program is free software; you can redistribute it and/or");
			file.WriteLine(" * modify it under the terms of the GNU General Public License");
			file.WriteLine(" * as published by the Free Software Foundation; either version 2");
			file.WriteLine(" * of the License, or (at your option) any later version.");
			file.WriteLine(" *");
			file.WriteLine(" * This program is distributed in the hope that it will be useful,");
			file.WriteLine(" * but WITHOUT ANY WARRANTY; without even the implied warranty of");
			file.WriteLine(" * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the");
			file.WriteLine(" * GNU General Public License for more details.");
			file.WriteLine(" *");
			file.WriteLine(" * You should have received a copy of the GNU General Public License");
			file.WriteLine(" * along with this program; if not, write to the Free Software");
			file.WriteLine(" * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.");
			file.WriteLine(" *");
			file.WriteLine(" */");
			file.WriteLine();
			file.WriteLine("using System;");
		}

		/// <summary>
		/// Strips the part of ful type name if it is defined in "using".
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static string StripType(string type)
		{
			if (type.StartsWith("System.", StringComparison.Ordinal))
			{
				return type.Substring(7);
			}
			return type;
		}

		/// <summary>
		/// Generates the property.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="property">The property.</param>
		/// <param name="isReadOnly">if set to <c>true</c> the readonly property is generated.</param>
		public static void GenerateProperty(StreamWriter file, IPropertyMap property, bool isReadOnly)
		{
			string fieldType = ConvertColumnTypeToCsType(property.GetColumnMap().DataType);
			string fieldName = GetFieldName(property);

			file.WriteLine("\t\tpublic " + fieldType + " " + property.Name);
			file.WriteLine("\t\t{");
			file.WriteLine("\t\t\tget { return " + fieldName + "; }");
			if (!isReadOnly)
			{
				file.WriteLine("\t\t\tset { " + fieldName + " = value; }");
			}
			file.WriteLine("\t\t}");
		}

		/// <summary>
		/// Generates the field.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <param name="property">The property.</param>
		public static void GenerateField(StreamWriter file, IPropertyMap property)
		{
			string fieldType = ConvertColumnTypeToCsType(property.GetColumnMap().DataType);
			string fieldName = GetFieldName(property);
			file.WriteLine("\t\tprivate " + fieldType + " " + fieldName + ";");
		}

		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		private static string GetFieldName(IPropertyMap property)
		{
			string fieldName = "m_" + char.ToLower(property.Name[0]);
			if (property.Name.Length > 1)
			{
				fieldName += property.Name.Substring(1);
			}
			return fieldName;
		}

		/// <summary>
		/// Creates the param name from property name.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public static string GetParamName(IPropertyMap property)
		{
			StringBuilder paramName = new StringBuilder(property.Name.Length);
			paramName.Append(char.ToLower(property.Name[0]));
			if (property.Name.Length > 1)
			{
				paramName.Append(property.Name.Substring(1));
			}

			return paramName.ToString();
		}

		/// <summary>
		/// Gets the primary key of a class.
		/// </summary>
		/// <param name="classMap">The class map.</param>
		/// <returns></returns>
		public static IPropertyMap GetPrimaryKey(IClassMap classMap)
		{
			ArrayList primary = classMap.GetIdentityPropertyMaps();
			if (primary.Count != 1)
			{
				throw new Exception("Wrong primary keys count for class " + classMap.Name + ": " + primary.Count);
			}

			return (IPropertyMap) primary[0];
		}

		/// <summary>
		/// Gets the root primary key, follows all references (foreign keys).
		/// </summary>
		/// <param name="propertyMap">The property map.</param>
		/// <returns></returns>
		public static IPropertyMap GetRootPrimaryKey(IPropertyMap propertyMap)
		{
			int count = 0;
			IPropertyMap key = propertyMap;
			while (key.ReferenceType != ReferenceType.None)
			{
				key = ClassUtility.GetPrimaryKey(propertyMap.GetReferencedClassMap());
				if (++count >= 10000)
				{
					return key;
					throw new Exception("Ring reference? count: " + count + "; property: " + propertyMap.Name + "; class: " + propertyMap.ClassMap.Name);
				}
			}
			return key;
		}

		/// <summary>
		/// Converts the type of the column type to C# type string.
		/// </summary>
		/// <param name="dbType">Type of the db.</param>
		/// <returns>C# type as string.</returns>
		public static string ConvertColumnTypeToCsType(DbType dbType)
		{
			switch (dbType)
			{
				case DbType.AnsiString:
				case DbType.AnsiStringFixedLength:
				case DbType.Guid:
				case DbType.String:
				case DbType.StringFixedLength:
					return "string";
				case DbType.Binary:
					return "byte[]";
				case DbType.Boolean:
					return "bool";
				case DbType.Byte:
					return "byte";
				case DbType.Date:
				case DbType.Time:
				case DbType.DateTime:
					return "DateTime";
				case DbType.Decimal:
					return "decimal";
				case DbType.Double:
					return "double";
				case DbType.Int16:
					return "short";
				case DbType.Int32:
					return "int";
				case DbType.Int64:
					return "long";
				case DbType.Object:
					return "object";
				case DbType.SByte:
					return "sbyte";
				case DbType.Single:
					return "float";
				case DbType.UInt16:
					return "ushort";
				case DbType.UInt32:
					return "uint";
				case DbType.UInt64:
					return "ulong";
				default:
					throw new Exception("unknown db field type: " + dbType.ToString());
			}
		}
	}
}
