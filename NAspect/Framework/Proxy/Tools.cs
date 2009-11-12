// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System.Reflection;

namespace Puzzle.NAspect.Framework.Utils
{
    /// <summary>
    /// Util class.
    /// </summary>
    public class AopTools
    {
        #region GetMethodSignature

        /// <summary>
        /// Returns the signature for a method,property or ctor.
        /// </summary>
        /// <param name="method">a method,property or ctor</param>
        /// <returns>string based representation of the method signature</returns>
        public static string GetMethodSignature(MethodBase method)
        {
            return method.ToString();
        }

        #endregion
    }
}