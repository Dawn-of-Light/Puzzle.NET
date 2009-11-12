using System;
using System.Collections.Generic;
using System.Text;
using Puzzle.NPersist.Framework.Mapping;

namespace DOLDatabase.Templates.Plugin
{
	/// <summary>
	/// Contains all typical converters from mappping to string.
	/// </summary>
	public class MapToStringConverters
	{
		/// <summary>
		/// Combines the specified parts of a string.
		/// </summary>
		/// <param name="str">The string to write to.</param>
		/// <param name="opening">The opening string.</param>
		/// <param name="leftStr">The left part.</param>
		/// <param name="middle">The middle string.</param>
		/// <param name="rightStr">The right part.</param>
		/// <param name="closing">The closing string.</param>
		private static void Combine(StringBuilder str, string opening, string leftStr, string middle, string rightStr, string closing)
		{
			// is it the first element
			if (leftStr == null)
			{
				if (opening != null)
				{
					str.Append(opening);
				}
			}
			else
			{
				str.Append(leftStr);
			}

			// is it the last element
			if (rightStr == null)
			{
				if (closing != null)
				{
					str.Append(closing);
				}
			}
			else if (middle != null && leftStr != null && rightStr != null)
			{
				str.Append(middle);
			}
		}
		
		/// <summary>
		/// Combines column map names to a string.
		/// </summary>
		/// <param name="leftMap">The left map.</param>
		/// <param name="rightMap">The right map.</param>
		/// <param name="leftIndex">Index of the left map.</param>
		/// <returns>Combined column names.</returns>
		public static void Columns(StringBuilder str, IMap leftMap, IMap rightMap, int leftIndex)
		{
			string left = leftMap == null ? null : leftMap.Name;
			string right = rightMap == null ? null : rightMap.Name;

			Combine(str, "`", left, "`,`", right, "`");
		}

		/// <summary>
		/// Combines property names using "And" as a glue.
		/// </summary>
		/// <param name="leftMap">The left map.</param>
		/// <param name="rightMap">The right map.</param>
		/// <param name="leftIndex">Index of the left map.</param>
		/// <returns>Combined property names.</returns>
		public static void PropertyAnd(StringBuilder str, IMap leftMap, IMap rightMap, int leftIndex)
		{
			string left = leftMap == null ? null : leftMap.Name;
			string right = rightMap == null ? null : rightMap.Name;
			
			Combine(str, null, left, "And", right, null);
		}

		/// <summary>
		/// Joins the all the map names.
		/// </summary>
		/// <param name="str">The string to write to.</param>
		/// <param name="leftMap">The left map.</param>
		/// <param name="rightMap">The right map.</param>
		/// <param name="leftIndex">Index of the left map.</param>
		public static void Join(StringBuilder str, IMap leftMap, IMap rightMap, int leftIndex)
		{
			string left = leftMap == null ? null : leftMap.Name;
			string right = rightMap == null ? null : rightMap.Name;

			Combine(str, null, left, null, right, null);
		}
	}
}
