using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DOLDatabase.Templates.Plugin
{
	/// <summary>
	/// Combines 2 objects.
	/// </summary>
	/// <typeparam name="T">The objects' type.</typeparam>
	/// <param name="left">The first object to combine.</param>
	/// <param name="right">The second object to combine.</param>
	/// <param name="leftIndex">The index of first object, can be -1.</param>
	/// <returns>Result of combine.</returns>
	public delegate void GlueStringCallback<T>(StringBuilder str, T left, T right, int leftIndex);
	
	/// <summary>
	/// String helper functions.
	/// </summary>
	public class StringUtility
	{
		/// <summary>
		/// Combines the objects into a string.
		/// </summary>
		/// <param name="objects">The objects.</param>
		/// <param name="glueCallback">The glue callback.</param>
		/// <returns></returns>
		public static StringBuilder CombineObjects<T>(IEnumerable<T> objects, GlueStringCallback<T> glueCallback)
		{
			if (glueCallback == null)
			{
				throw new ArgumentNullException("glueCallback");
			}
			
			int leftIndex = -1;
			T left = default(T);
			T right = default(T);
			StringBuilder res = new StringBuilder(32);

			foreach (T obj in objects)
			{
				right = obj;
				
				if (glueCallback != null)
				{
					glueCallback(res, left, right, leftIndex);
				}

				++leftIndex;
				left = right;
			}
			
			glueCallback(res, left, default(T), leftIndex);

			return res;
		}
	}
}
