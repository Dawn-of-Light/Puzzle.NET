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
using Puzzle.NAspect.Framework.Aop;
using Puzzle.NCore.Framework.Exceptions;
using Puzzle.NPersist.Framework.Exceptions;
using Puzzle.NPersist.Framework.Interfaces;

namespace Puzzle.NPersist.Framework.Aop
{
	/// <summary>
	/// Summary description for NPersistAspect.
	/// </summary>
    public class NPersistEntityAspect : IGenericAspect
	{
		private IContext context;
		public NPersistEntityAspect(IContext context)
		{
			this.context = context;
		}		

		public string Name
		{
			get { return "NPersistEntityAspect"; }
			set { throw new IAmOpenSourcePleaseImplementMeException(); }
		}

		public bool IsMatch(Type type)
		{
			return (context.DomainMap.GetClassMap(type) != null);
		}

		public IList Mixins
		{
			get 
			{
				IList arr = new ArrayList();
				arr.Add(typeof( NPersistProxyMixin ));
				arr.Add(typeof( Puzzle.NPersist.Framework.Proxy.Mixins.NullValueHelperMixin ));
				arr.Add(typeof( Puzzle.NPersist.Framework.Proxy.Mixins.ObjectStatusHelperMixin ));
				arr.Add(typeof( Puzzle.NPersist.Framework.Proxy.Mixins.CloneHelperMixin ));
				arr.Add(typeof( Puzzle.NPersist.Framework.Proxy.Mixins.OriginalValueHelperMixin ));
				arr.Add(typeof( Puzzle.NPersist.Framework.Proxy.Mixins.UpdatedPropertyTrackerMixin ));
                arr.Add(typeof(Puzzle.NPersist.Framework.Proxy.Mixins.IdentityHelperMixin));
#if NET2
                arr.Add(typeof(Puzzle.NPersist.Framework.Proxy.Mixins.PropertyChangedHelperMixin));
                arr.Add(typeof(Puzzle.NPersist.Framework.Proxy.Mixins.EditableObjectMixin));
#endif
				return arr;
			}
		}

		public IList Pointcuts
		{
			get 
			{
				IList arr = new ArrayList();
				arr.Add(new NPersistEntityCtorPointcut(context));
				arr.Add(new NPersistEntityPropertyPointcut(context));
				return arr;
			}
		}
	}
}
