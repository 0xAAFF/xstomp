using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XStomp.Unit
{
    /// <summary>
    /// Ack values as defined by STOMP protocol specification.
    /// </summary>
    public static class StompAckValues
    {
        public const string AckAutoValue = "auto";
        public const string AckClientValue = "client";
        public const string AckClientIndividualValue = "client-individual";

        private static readonly List<string> _ackValidValues = new List<string> { AckAutoValue, AckClientIndividualValue, AckClientValue };

        public static bool IsValidAckValue(string ackValue)
        {
            return _ackValidValues.Contains(ackValue);
        }

        public static void ThrowIfInvalidAckValue(string ackValue)
        {
            if (!_ackValidValues.Contains(ackValue))
                throw new ArgumentException(string.Format("{0} header value MUST be: '{1}', '{2}' or '{3}'", StompHeaders.Ack, AckAutoValue, AckClientIndividualValue, AckClientValue));

        }
    }
}
