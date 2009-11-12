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
	public enum ObjectStatus
	{
		NotLoaded = 0,
		UpForCreation = 1,
		UpForDeletion = 2,
		Dirty = 3,
		Clean = 4,
		Deleted = 5,
		NotRegistered = 6
	}
}