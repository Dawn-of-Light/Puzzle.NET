using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Puzzle.NPersist.Framework.Mapping;
using Puzzle.ObjectMapper.Plugin;

namespace DOLDatabase.Templates.Plugin.Converters
{
	/// <summary>
	/// This class converts table and field names to class names.
	/// </summary>
	[ConverterClass("DOL name converters")]
	public class TableToClassNameConverter
	{
		static readonly StringBuilder m_builder = new StringBuilder();
		
		/// <summary>
		/// Converts class name to table name.
		/// </summary>
		/// <param name="tableName">Name of table.</param>
		/// <returns>Name of class.</returns>
		[ConverterMethod("Table to class")]
		public string GetClassNameFromTable(string tableName)
		{
			return TableToClassName(tableName);
		}
		
		/// <summary>
		/// Converts column name to class property.
		/// </summary>
		/// <param name="columnMap">Column.</param>
		/// <returns>Class property name.</returns>
		[ConverterMethod("Column to property")]
		public string GetPropertyNameFromColumn(IColumnMap columnMap)
		{
			return TableToClassName(columnMap.Name);
		}
		
		/// <summary>
		/// Converts table name to class name.
		/// </summary>
		/// <param name="name">Table name to convert.</param>
		/// <returns>Class name.</returns>
		private static string TableToClassName(string name)
		{
			// Clear buffer
			m_builder.Length = 0;
			
			// Split name on parts
			string[] parts = name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
			
			// Combine all parts
			foreach (string part in parts)
			{
				// Capitalize first letter
				if (part.Length > 0)
				{
					m_builder.Append(char.ToUpper(part[0]));
				}
				
				// Add the rest
				if (part.Length > 1)
				{
					m_builder.Append(part.ToLower(), 1, part.Length - 1);
				}
			}
			
			// Get result and clear buffer
			string ret = m_builder.ToString();
			m_builder.Length = 0;

			return ret;
		}
	}
}
