using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XStomp.Server.Frame
{
    /// <summary>
    /// Class representing a Heartbeat.
    /// </summary>
    public class Heartbeat
    {
        public readonly static Heartbeat NoHeartbeat = new Heartbeat(0, 0);

        public int Outgoing { get; private set; }
        public int Incoming { get; private set; }
        public string RawHeartbeat { get; private set; }

        public Heartbeat(int outgoing, int incoming)
        {
            Outgoing = outgoing;
            Incoming = incoming;
            RawHeartbeat = outgoing + "," + incoming;
        }

        public static Heartbeat GetHeartbeat(string heartbeat)
        {
            if (string.IsNullOrEmpty(heartbeat))
                return NoHeartbeat;

            string[] heartBeatParts = heartbeat.Split(',');

            if (heartBeatParts.Length != 2)
                throw new FormatException("A heart-beat header MUST contain two positive integers separated by a comma.");

            int outgoing, incoming;
            if (!int.TryParse(heartBeatParts[0], out outgoing) || outgoing < 0 || !int.TryParse(heartBeatParts[1], out incoming) || incoming < 0)
                throw new FormatException("A heart-beat header MUST contain two positive integers separated by a comma.");

            if (outgoing == 0 && incoming == 0)
                return NoHeartbeat;

            return new Heartbeat(outgoing, incoming);
        }
    }
}
