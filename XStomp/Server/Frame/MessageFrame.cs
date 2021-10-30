/*******************************************************************************
 *
 *  Copyright (c) 2020 0xAAFF<littletools@outlook.com>
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 * 
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace XStomp.Server.Frame
{
    /// <summary>
    /// Class representing a STOMP message frame.
    /// </summary>
    public class MessageFrame : StompFrame
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="messageid"></param>
        /// <param name="subscription"></param>
        /// <param name="userDefinedHeaders"></param>
        /// <param name="body"></param>
        public MessageFrame(string destination, string messageid, string subscription, Dictionary<string, string> userDefinedHeaders, string body, string ackValue = null) : base(StompCommands.Message, body)
        {
            /*
             * 把自定义的head头先复制到head头列表中
             * 这个头列表中,可能已经包含了destination,message-id,ack,subscription头信息
             * 
             * copy user defined headers
             * maybe already include destination,message-id,ack,subscription
             */
            if (userDefinedHeaders != null)
            {
                foreach (var v in userDefinedHeaders)
                {
                    if (v.Key != "" && v.Value != "")
                    {
                        Headers[v.Key] = v.Value;
                    }
                }
            }



            // destination
            //if (!string.IsNullOrEmpty(destination))
            //{
                Headers[StompHeaders.Destination] = destination;
            //}
            //else
            //{
            //    throw new InvalidDataException("destination can not be NULL/Empty.\n\n" + "The MESSAGE frame MUST include a destination header indicating the destination the message was sent to. \nIf the message has been sent using STOMP, this destination header SHOULD be identical to the one used in the corresponding SEND frame.");
            //}

            if (string.IsNullOrEmpty(Headers[StompHeaders.Destination]))
            {
                throw new InvalidDataException("destination can not be NULL/Empty.\n\n" + "The MESSAGE frame MUST include a destination header indicating the destination the message was sent to. \nIf the message has been sent using STOMP, this destination header SHOULD be identical to the one used in the corresponding SEND frame.");
            }
            // message-id
            //if (!string.IsNullOrEmpty(messageid))
            //{
                Headers[StompHeaders.MessageId] = messageid;
            //}
            if (string.IsNullOrEmpty(Headers[StompHeaders.MessageId]))
            {
                throw new InvalidDataException("message-id can not be NULL/Empty.\n\n" + "The MESSAGE frame MUST also contain a message-id header with a unique identifier for that message and a subscription header matching the identifier of the subscription that is receiving the message.");
            }
            // ack 
            /*
             * SpringBoot WebSocket 不支持 ack,receipts ,仅仅实现了stomp的子集.当前版本也决定暂时不实现此功能
             * SpringBoot WebSocket Not Support ack,receipts
             * The simple broker is great for getting started but supports only a subset of STOMP commands (e.g. no acks, receipts, etc.), relies on a simple message sending loop, and is not suitable for clustering. As an alternative, applications can upgrade to using a full-featured message broker.
             */
            //if (!string.IsNullOrEmpty(ackValue))
            //{
            //    Headers[StompHeaders.Ack] = ackValue;
            //}
            //subscription
            //if (!string.IsNullOrEmpty(subscription))
            //{
                Headers[StompHeaders.Subscription] = subscription;
            //}
            if (string.IsNullOrEmpty(Headers[StompHeaders.Subscription]))
            {
                throw new InvalidDataException("subscription can not be NULL/Empty.\n\n" + "订阅包里的id头就必须是唯一的,这个订阅id将用在MESSAGE和取消订阅的操作中,否则客户端无法分辨数据来源于哪个订阅id \n Since a single connection can have multiple open subscriptions with a server, an id header MUST be included in the frame to uniquely identify the subscription. The id header allows the client and server to relate subsequent MESSAGE or UNSUBSCRIBE frames to the original subscription.");
            }
            Headers[StompHeaders.ContentType] = MediaTypeNames.Text.Plain + ";charset=" + StompServerConfig.ServerCharset.WebName.ToUpper();
            Headers[StompHeaders.ContentLength] = StompServerConfig.ServerCharset.GetBytes(Body).Length.ToString();
        }






    }
}
