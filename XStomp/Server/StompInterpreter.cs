using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XStomp.Factory;
using XStomp.Unit;

namespace XStomp.Server
{
    /// <summary>
    /// Class to transform frames into an instance of a 'subclass' matching its command.
    /// </summary>
    public static class StompInterpreter
    {
        ///// <summary>
        ///// Transform a frame into a 'subclass' matching its command.
        ///// These subclasses are: ReceiptFrame, MessageFrame, ErrorFrame, ConnectedFrame.
        ///// </summary>
        ///// <param name="frame">Frame to be interpreted.</param>
        ///// <returns>A new frame which class matches its command type.</returns>
        //public static StompFrame Interpret(StompFrame frame,string session)
        //{
        //    if (frame == null)
        //        throw new ArgumentNullException("frame");

        //    switch (frame.Command)
        //    {
        //        case StompCommands.Heartbeat:
        //            return frame;
        //        case StompCommands.Receipt:
        //            if (!string.IsNullOrEmpty( frame.Body))
        //                throw new InvalidDataException("Receipt frame MUST NOT have a body.");
        //            return new ReceiptFrame(frame.Headers);
        //        case StompCommands.Message:
        //            return new MessageFrame(frame.Headers, frame.Body);
        //        case StompCommands.Error:
        //            return new ErrorFrame(frame.Headers, frame.Body);
        //        case StompCommands.Connected:
        //            if (frame.Body != "")
        //                throw new InvalidDataException("Connected frame MUST NOT have a body.");
        //            return StompServerFactory.CreateConnectedFrame(frame, session);
        //        default:
        //            throw new InvalidDataException(string.Format("'{0}' is not a valid STOMP server command.", frame.Command));
        //    }
        //}
    }
}
