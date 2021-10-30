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
using XStomp.Server.Frame;
using XStomp.Unit;

namespace XStomp.Server
{
    public static class StompServerFactory
    {
        /*
            STOMP 1.2 servers MUST set the following headers:

            version : The version of the STOMP protocol the session will be using. See Protocol Negotiation for more details.
            STOMP 1.2 servers MAY set the following headers:

            heart-beat : The Heart-beating settings.

            session : A session identifier that uniquely identifies the session.

            server : A field that contains information about the STOMP server. The field MUST contain a server-name field and MAY be followed by optional comment fields delimited by a space character.

            The server-name field consists of a name token followed by an optional version number token.

            server = name ["/" version] *(comment)

            Example:

            server:Apache/1.3.9
         */
        public static StompFrame CreateConnectedFrame(StompFrame connectFrame, string session)
        {
            StompFrame ConnectedFrame = new StompFrame(StompCommands.Connected);

            if (connectFrame.Headers.ContainsKey(StompHeaders.AcceptVersion))
            {
                int maxVersion = -1;
                foreach (var v in connectFrame.Headers[StompHeaders.AcceptVersion].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Reverse())
                {
                    if (v == "1.2")
                    {
                        maxVersion = 2;
                        break;
                    }
                    else if (v == "1.1")
                    {
                        maxVersion = maxVersion > 1 ? maxVersion : 1;
                    }
                    else if (v == "1.0")
                    {
                        maxVersion = maxVersion > 0 ? maxVersion : 0;
                    }
                }
                if (maxVersion > -1)
                {
                    ConnectedFrame[StompHeaders.Version] = "1." + maxVersion;
                }

                if (!ConnectedFrame.Headers.ContainsKey(StompHeaders.Version))
                {
                    string tips = "Version Unsupport";
                    string detail = "XStomp Server Supported Protocol Versions Are 1.0,1.1,1.2";
                    ErrorFrame errorFrame = new ErrorFrame(tips, detail, connectFrame);
                    return errorFrame;
                }
            }
            else
            {
                // Only Support Stomp 1.0
                // 没有这个头信息,表示只支持1.0版本的协议
                ConnectedFrame[StompHeaders.Version] = "1.0";
            }

            // 
            ConnectedFrame[StompHeaders.Session] = session; // headers.ContainsKey(StompHeaders.Session) ? headers[StompHeaders.Session] : session;

            // if you want support client try connect host ,Code here
            // 客户端希望连接的主机名,如果你要实现得更完整,在这里可以实现.我就默认支持此版本server了
            ConnectedFrame[StompHeaders.Server] = "XStompServer/1.0.0"; //ConnectedFrame.Headers.ContainsKey(StompHeaders.Server) ? ConnectedFrame[StompHeaders.Server] : "StompServerX/1.0.0";

            // close HeartBeat ,If you want support, Code here
            // 默认关闭心跳机制
            ConnectedFrame[StompHeaders.Heartbeat] = "0,0"; //ConnectedFrame.Headers.ContainsKey(StompHeaders.Heartbeat) ? ConnectedFrame.Headers[StompHeaders.Heartbeat] : "0,0";

            return ConnectedFrame;
        }


    }
}
