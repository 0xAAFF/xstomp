// https://github.com/ernstnaezer/ultralight/blob/master/
// Copyright 2011 Ernst Naezer, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XStomp
{
    public class StompFrameSerializer
    {
        /// <summary>
        ///   Serializes the specified message.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <returns>A serialized version of the given <see cref="StompFrame"/></returns>
        public static string Serialize(StompFrame message)
        {
            try
            {
                var buffer = new StringBuilder();

                buffer.Append(message.Command + "\n");

                if (message.Headers != null)
                {
                    foreach (var header in message.Headers)
                    {
                        buffer.Append(header.Key + ":" + header.Value + "\n");
                    }
                }

                buffer.Append("\n");
                buffer.Append(message.Body);
                buffer.Append('\0');

                return buffer.ToString();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                System.Diagnostics.Debug.WriteLine(e.Source);
                if (message.Headers != null)
                {
                    System.Diagnostics.Debug.WriteLine("Headers:");
                    foreach (var header in message.Headers)
                    {
                        System.Diagnostics.Debug.WriteLine(header.Key + ":" + header.Value + "\n");
                    }
                    System.Diagnostics.Debug.WriteLine("Body:");
                    System.Diagnostics.Debug.WriteLine(message.Body);

                }

            }
            return "";

        }

        /// <summary>
        /// Deserializes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A <see cref="StompFrame"/> instance</returns>
        public static StompFrame Deserialize(string message)
        {
            var reader = new StringReader(message);

            var command = reader.ReadLine();

            var headers = new Dictionary<string, string>();

            var header = reader.ReadLine();
            while (!string.IsNullOrEmpty(header))
            {
                var split = header.Split(':');
                if (split.Length == 2) headers[split[0].Trim()] = split[1].Trim();
                header = reader.ReadLine() ?? string.Empty;
            }

            var body = reader.ReadToEnd() ?? string.Empty;
            body = body.TrimEnd('\r', '\n', '\0');

            return new StompFrame(command, body, headers);
        }
    
    
    }
}
