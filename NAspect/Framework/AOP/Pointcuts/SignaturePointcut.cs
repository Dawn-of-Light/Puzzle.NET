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
using Puzzle.NAspect.Framework.Tools;
using Puzzle.NAspect.Framework.Utils;

namespace Puzzle.NAspect.Framework.Aop
{
    /// <summary>
    /// Pointcut that matches method signatures.
    /// </summary>
    public class SignaturePointcut : PointcutBase
    {
        /// <summary>
        /// Wildcard pattern of the method signatures to match
        /// </summary>
        public string TargetMethodSignature;

        /// <summary>
        /// SignaturePointcut ctor. 
        /// </summary>
        /// <param name="targetMethodSignature">Wildcard pattern of the method signatures to match</param>
        /// <param name="interceptors">Untyped list of <c>IInterceptor</c>s to be applied to by this pointcut</param>
        public SignaturePointcut(string targetMethodSignature, IList interceptors)
        {
            TargetMethodSignature = targetMethodSignature;
            Interceptors = interceptors;
        }

        /// <summary>
        /// SignaturePointcut ctor.
        /// </summary>
        /// <param name="targetMethodSignature">Wildcard pattern of the method signatures to match</param>
        /// <param name="interceptors">Array of <c>IInterceptors</c> to be applied by this pointcut</param>
        public SignaturePointcut(string targetMethodSignature, IInterceptor[] interceptors)
        {
            TargetMethodSignature = targetMethodSignature;
            Interceptors = new ArrayList(interceptors);
        }

        /// <summary>
        /// SignaturePointcut ctor.
        /// </summary>
        /// <param name="targetMethodSignature">Wildcard pattern of the method signatures to match</param>
        /// <param name="interceptor">a single <c>IInterceptor</c> that should be applied by this pointcut</param>
        public SignaturePointcut(string targetMethodSignature, IInterceptor interceptor)
        {
            TargetMethodSignature = targetMethodSignature;
            Interceptors = new ArrayList(new IInterceptor[] {interceptor});
        }

        /// <summary>
        /// SignaturePointcut ctor.
        /// </summary>
        /// <param name="targetMethodSignature">Wildcard pattern of the method signatures to match</param>
        /// <param name="interceptor">Interceptor delegate to apply on matched methods, valid delegates are <c>BeforeDelegate</c>, <c>AroundDelegate</c> and <c>AfterDelegate</c></param>
        public SignaturePointcut(string targetMethodSignature, Delegate interceptor)
        {
            TargetMethodSignature = targetMethodSignature;
            ArrayList arr = new ArrayList();
            arr.Add(interceptor);
            Interceptors = arr;
        }

        /// <summary>
        /// Matches a method with the pointuct
        /// </summary>
        /// <param name="method">The method to match</param>
        /// <returns>True if the pointcut matched the method, otherwise false</returns>
        public override bool IsMatch(MethodBase method)
        {
            string methodsignature = AopTools.GetMethodSignature(method);
            if (Text.IsMatch(methodsignature, TargetMethodSignature))
                return true;
            else
                return false;
        }
    }
}