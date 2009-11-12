// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *
using System;
using System.Collections;
using System.Reflection;
using Puzzle.NAspect.Framework.Aop;
using Puzzle.NPersist.Framework.Mapping;

namespace Puzzle.NPersist.Framework.Aop

{
	/// <summary>
	/// Summary description for NPersistPropertyPointcut.
	/// </summary>
	public class NPersistEntityPropertyPointcut : IPointcut
	{
		private IContext context; 
		private static volatile IList interceptors = CreateInterceptors();

		private static IList CreateInterceptors()
		{
			IList arr = new ArrayList();
			arr.Add(new NPersistPropertyInterceptor ());
			return arr;
		}

		public NPersistEntityPropertyPointcut(IContext context)
		{
			this.context = context;
		}

		public IList Interceptors
		{
			get 
			{
				return interceptors;
			}
		}

		public bool IsMatch(MethodBase method)
		{
			string methodName = method.Name;
			if (!(methodName.StartsWith("get_") || methodName.StartsWith("set_")))
				return false;

			methodName = methodName.Substring(4);

			IClassMap classmap = context.DomainMap.GetClassMap(method.DeclaringType);
			if (classmap == null) 
				return false;
			return (classmap.GetPropertyMap(methodName) != null);
		}
	}
}
