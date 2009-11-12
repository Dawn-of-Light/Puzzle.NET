// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *


using System.Collections;
using Puzzle.NPersist.Framework.Interfaces;
using Puzzle.NAspect.Framework.Aop;
using Puzzle.NAspect.Framework;

namespace Puzzle.NPersist.Framework.Proxy.Mixins
{
	public class IdentityHelperMixin : IIdentityHelper , IProxyAware
	{
    //    private IAopProxy target;
        private string identity;
        public string GetIdentity()
        {
            return identity;
        }

        public string GetIdentity(Puzzle.NPersist.Framework.Mapping.IPropertyMap propertyMap, object value)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void SetIdentity(string identity)
        {
            this.identity = identity;
        }

        public void SetProxy(IAopProxy target)
        {
        //    this.target = target;
        }

        #region Property IsInvalid 
        private bool isInvalid = true;
        public bool IsInvalid
        {
            get
            {
                return this.isInvalid;
            }
            set
            {
                this.isInvalid = value;
            }
        }                        
        #endregion
    }
}
