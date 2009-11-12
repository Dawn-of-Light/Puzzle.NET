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
	/// Summary description for PersistenceType.
	/// </summary>
	public enum PersistenceType
	{
		Default = 0, 
		ObjectRelational = 1,
		ObjectDocument = 2,
		ObjectService = 3,
		ObjectObject = 4, 
		Transient = 5,
		Other = 6
	}
}
