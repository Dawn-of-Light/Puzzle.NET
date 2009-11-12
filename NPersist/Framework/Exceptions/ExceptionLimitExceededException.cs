// *
// * Copyright (C) 2005 Mats Helander : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Puzzle.NPersist.Framework.Exceptions
{
	public class ExceptionLimitExceededException : CompositeException
	{
		public ExceptionLimitExceededException() : base()
		{
		}

		public ExceptionLimitExceededException(string message) : base(message)
		{
		}

		public ExceptionLimitExceededException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public ExceptionLimitExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public ExceptionLimitExceededException(IList innerExceptions) : base("Maximum number of allowed exceptions during an atomic operation was exceeded. Please inspect the InnerExceptions property of this exception to see the exceptions that were encountered.", innerExceptions)
		{
		}

		public ExceptionLimitExceededException(string message, IList innerExceptions) : base(message, innerExceptions)
		{
		}

	}
}
