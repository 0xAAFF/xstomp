using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XStomp.Server.Frame
{
    /*
     * A RECEIPT frame is sent from the server to the client once a server has successfully processed a client frame that requests a receipt. A RECEIPT frame will include the header receipt-id, where the value is the value of the receipt header in the frame which this is a receipt for.
     *
     *RECEIPT
     *receipt-id:message-12345
     *
     *^@
     *
     *The receipt body will be empty.
     */
    /// <summary>
    /// Class representing a STOMP message frame.
    /// </summary>
    public class ReceiptFrame : StompFrame
    {
        public string ReceiptId { get; private set; }
        
        public ReceiptFrame(Dictionary<string, string> headers) : base(StompCommands.Receipt,"",headers)
        {
            ReceiptId = Headers.FirstOrDefault(header => header.Key == StompHeaders.ReceiptId).Value;
            if (string.IsNullOrEmpty(ReceiptId))
            {
                ThrowMandatoryHeaderException(StompHeaders.ReceiptId);
            }
        }
    }
}
