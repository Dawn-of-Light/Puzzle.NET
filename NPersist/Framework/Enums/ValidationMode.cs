// *
// * Copyright (C) 2005 Mats Helander : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

namespace Puzzle.NPersist.Framework.Enumerations
{
	/// <summary>
	/// Summary description for ValidatinMode.
	/// </summary>
	public enum ValidationMode
	{
		Default = 0, // Inherits ValidationMode from the ValidationMode holder above. A propertyMap inherits from its classMap, that inherits from its DomainMap, which inherits from the Context. If the Context has ValidationMode.Default, this translates to ValidationMode.ValidateDirty
		ValidateDirty = 1, // Validates all (and only) Dirty properties
		ValidateLoaded = 2, //Validates all Loaded properties (including all Dirty properties)
		ValidateAll = 3, //Validates all Loaded, Dirty and NotLoaded properties (forces loading of NotLoaded properties for validation)
		Off = 4 //No validation 
	}
}
