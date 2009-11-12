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

namespace Puzzle.NCore.Framework.Logging
{
    public interface ILogManager
    {
        void Debug(object message, object verbose);
        void Info(object message, object verbose);
        void Warn(object message, object verbose);
        void Error(object message, object verbose);
        void Fatal(object message, object verbose);

        void Debug(object message, object verbose, Exception t);
        void Info(object message, object verbose, Exception t);
        void Warn(object message, object verbose, Exception t);
        void Error(object message, object verbose, Exception t);
        void Fatal(object message, object verbose, Exception t);

        void Debug(object sender, object message, object verbose);
        void Info(object sender, object message, object verbose);
        void Warn(object sender, object message, object verbose);
        void Error(object sender, object message, object verbose);
        void Fatal(object sender, object message, object verbose);

        void Debug(object sender, object message, object verbose, Exception t);
        void Info(object sender, object message, object verbose, Exception t);
        void Warn(object sender, object message, object verbose, Exception t);
        void Error(object sender, object message, object verbose, Exception t);
        void Fatal(object sender, object message, object verbose, Exception t);

        IList Loggers { get; set; }
    }
}