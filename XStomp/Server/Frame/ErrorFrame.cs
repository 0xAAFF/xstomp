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
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace XStomp.Server.Frame
{
    /// <summary>
    /// Class representing a STOMP ERROR frame.
    /// </summary>
    public class ErrorFrame : StompFrame
    {
        /// <summary>
        /// 创建一个Stomp的ERROR帧
        /// </summary>
        /// <param name="errorTips"></param>
        /// <param name="errorDetail"></param>
        /// <param name="sourceFrame"></param>
        public ErrorFrame(string errorTips, string errorDetail = "", StompFrame sourceFrame = null) : base(StompCommands.Error)
        {
            if (sourceFrame != null && sourceFrame.Headers.ContainsKey(StompHeaders.ReceiptId) && !string.IsNullOrEmpty(sourceFrame.Headers[StompHeaders.ReceiptId]))
            {
                Headers[StompHeaders.ReceiptId] = sourceFrame.Headers[StompHeaders.ReceiptId];// 携带上回执信息的值
            }

            if (!string.IsNullOrEmpty(errorTips))
            {
                Headers[StompHeaders.Message] = errorTips;
            }

            Headers[StompHeaders.ContentType] = MediaTypeNames.Text.Plain + ";charset=" + StompServerConfig.ServerCharset.WebName.ToUpper();
            string errorbody = "Server Cannot processed This Frame\n";
            errorbody += "-------------------\n";
            if (sourceFrame != null)
            {
                errorbody += sourceFrame;
                errorbody += "-------------------\n";
            }
            errorbody += errorDetail;
            Body = errorbody;
            Headers[StompHeaders.ContentLength] = StompServerConfig.ServerCharset.GetBytes(Body).Length.ToString();
        }
    }
}
