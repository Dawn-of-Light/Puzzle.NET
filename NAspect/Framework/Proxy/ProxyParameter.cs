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
using System.Diagnostics;

namespace Puzzle.NAspect.Framework
{
    /// <summary>
    /// Enum for parameter directions
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// ByValue parameter
        /// </summary>
        ByVal,
        /// <summary>
        /// ByReference parameter
        /// </summary>
        Ref,
        /// <summary>
        /// Output parameter
        /// </summary>
        Out
    }

    /// <summary>
    /// Representation of an intercepted call parameter.
    /// </summary>
    public class InterceptedParameter
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Index of the parameter in the method signature.
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// Data type of the parameter.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Direction of the parameter.
        /// </summary>
        public readonly ParameterType ParameterType;

        /// <summary>
        /// Boxed value of the parameter.
        /// (dont even think about throwing the old generics[T] card at me here, I've tried it and its slower than boxing  //Roger)
        /// </summary>
        public object Value;

        /// <summary>
        /// Ctor for an intercepted parameter
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="index">Index of the parameter in the method signature</param>
        /// <param name="type">Data type of the parameter.</param>
        /// <param name="value">Boxed value of the parameter.</param>
        /// <param name="parametertype">Direction of the parameter.</param>
        [DebuggerStepThrough()]
        public InterceptedParameter(string name, int index, Type type, object value, ParameterType parametertype)
        {
            Name = name;
            Index = index;
            Type = type;
            Value = value;
            ParameterType = parametertype;
        }
    }
}