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
using Puzzle.NAspect.Framework.Interception;

namespace Puzzle.NAspect.Framework.Aop
{
    /// <summary>
    /// Pointcut that matches attributes on target methods.
    /// </summary>
    public class AttributePointcut : PointcutBase
    {
        /// <summary>
        /// The attribute type that should be matched by this pointcut.
        /// </summary>
        public Type AttributeType;

        #region Pointcut

        /// <summary>
        /// AttributePointcut ctor.
        /// </summary>
        /// <param name="attributeType">Attribute type to match</param>
        /// <param name="interceptors">Untyped list of <c>IInterceptor</c>s to apply on matched methods</param>
        public AttributePointcut(Type attributeType, IList interceptors)
        {
            AttributeType = attributeType;
            Interceptors = interceptors;
        }

        #endregion

        #region Pointcut

        /// <summary>
        /// AttributePointcut ctor.
        /// </summary>
        /// <param name="attributeType">Attribute type to match</param>
        /// <param name="interceptors">Array of <c>IInterceptor</c>s to apply on matched methods</param>
        public AttributePointcut(Type attributeType, IInterceptor[] interceptors)
        {
            AttributeType = attributeType;
            Interceptors = new ArrayList(interceptors);
        }

        #endregion

        #region Pointcut

        /// <summary>
        /// AttributePointcut ctor.
        /// </summary>
        /// <param name="attributeType">Attribute type to match</param>
        /// <param name="interceptor"><c>IInterceptor</c> instance to appy on matched methods.</param>
        public AttributePointcut(Type attributeType, IInterceptor interceptor)
        {
            AttributeType = attributeType;
            Interceptors = new ArrayList(new IInterceptor[] {interceptor});
        }

        #endregion

        #region Pointcut

        /// <summary>
        /// AttributePointcut ctor.
        /// </summary>
        /// <param name="attributeType">Attribute type to match</param>
        /// <param name="interceptor">Interceptor delegate to apply on matched methods, valid delegates are <c>BeforeDelegate</c>, <c>AroundDelegate</c> and <c>AfterDelegate</c></param>
        public AttributePointcut(Type attributeType, Delegate interceptor)
        {
            AttributeType = attributeType;
            ArrayList arr = new ArrayList();
            arr.Add(interceptor);
            Interceptors = arr;
        }

        #endregion

        /// <summary>
        /// Matches a method with the pointuct
        /// </summary>
        /// <param name="method">The method to match</param>
        /// <returns>True if the pointcut matched the method, otherwise false</returns>
        public override bool IsMatch(MethodBase method)
        {
            if (method.GetCustomAttributes(AttributeType, true).Length > 0)
                return true;
            else
                return false;
        }
    }
}