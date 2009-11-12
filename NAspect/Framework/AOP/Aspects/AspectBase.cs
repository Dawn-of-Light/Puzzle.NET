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

namespace Puzzle.NAspect.Framework.Aop
{
    /// <summary>
    /// Base class for aspects.
    /// Contains lists of mixins and pointcuts for the aspect.
    /// You generally do not need to use this class by your self.
    /// Use the AttributeAspect , SignatureAspect or Typed aspects.
    /// </summary>
    /// <example>
    /// <para>When inheriting this class you must override the <c>IsMatch</c> method.</para>
    /// <code lang="CS">
    /// //aspect that matches all types whose name start with MyClass
    /// public override bool IsMatch(Type type)
    /// {
    ///     if (type.Name.StartsWith("MyClass"))
    ///         return true;
    ///     else
    ///         return false;
    /// }
    /// </code>
    /// </example>
    public abstract class GenericAspectBase : IGenericAspect
    {
        private string name;
        private IList mixins = new ArrayList();
        private IList pointcuts = new ArrayList();

        /// <summary>
        /// Just a name of the aspect, has no real purpose today.
        /// Features to fetch named aspects might be added later.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Override this method in a subclasses to match specific types.
        /// </summary>
        /// <param name="type">Target that might get this aspect applied to it</param>
        /// <returns>return true if the given type should get this aspect applied, otherwise false</returns>
        public abstract bool IsMatch(Type type);


        /// <summary>
        /// List of mixin types.
        /// Since this is .NET 1.x compatible and we are lazy farts, you get this in an untyped manner.
        /// The element type of this list should be <c>System.Type</c>        
        /// </summary>
        /// <example>
        /// <code lang="CS">
        /// myAspect.Mixins.Add(typeof(MyMixin));
        /// myAspect.Mixins.Add(typeof(ISomeMarkerInterfaceWOImplementation));
        /// </code>
        /// </example>
        public IList Mixins
        {
            get { return mixins; }
            set { mixins = value; }
        }

        /// <summary>
        /// List of pointcuts.
        /// Since this is .NET 1.x compatible and we are lazy farts, you get this in an untyped manner.
        /// The element type of this list should be <c>IPointcut</c>.
        /// </summary>
        public IList Pointcuts
        {
            get { return pointcuts; }
            set { pointcuts = value; }
        }
    }
}