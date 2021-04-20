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
using System.Linq;
using System.Text;

namespace XStomp
{

    public class StompFrame
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompFrame" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        public StompFrame(string command) : this(command, string.Empty)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompFrame" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        /// <param name = "body">The body.</param>
        public StompFrame(string command, string body) : this(command, body, new Dictionary<string, string>())
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "StompFrame" /> class.
        /// </summary>
        /// <param name = "command">The command.</param>
        /// <param name = "body">The body.</param>
        /// <param name = "headers">The headers.</param>
        internal StompFrame(string command, string body, Dictionary<string, string> headers)
        {
            Command = command;
            Body = body;
            _headers = headers;

            this["content-length"] = body.Length.ToString();
        }

        public Dictionary<string, string> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        public string Body { get; protected set; }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// Gets or sets the specified header attribute.
        /// </summary>
        public string this[string header]
        {
            get { return _headers.ContainsKey(header) ? _headers[header] : string.Empty; }
            set { _headers[header] = value; }
        }

        /// <summary>
        /// Create and throws the exception used when a mandatory header is not included.
        /// </summary>
        /// <param name="headerName">Name of the mandatory header.</param>
        protected static void ThrowMandatoryHeaderException(string headerName)
        {
            throw new ArgumentException("'" + headerName + "' header is mandatory.", headerName);
        }

        public override string ToString()
        {
            try
            {
                var buffer = new StringBuilder();
                buffer.Append(Command + "\n");
                if (Headers != null)
                {
                    foreach (var header in Headers)
                    {
                        buffer.Append(header.Key + ":" + header.Value + "\n");
                    }
                }
                buffer.Append("\n");
                buffer.Append(Body);

                buffer.Append("\r\n");

                return buffer.ToString();

            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e.ToString());
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                System.Diagnostics.Debug.WriteLine(e.Source);
                if (Headers != null)
                {
                    System.Diagnostics.Debug.WriteLine("Headers:");
                    foreach (var header in Headers)
                    {
                        System.Diagnostics.Debug.WriteLine(header.Key + ":" + header.Value + "\n");
                    }
                    System.Diagnostics.Debug.WriteLine("Body:");
                    System.Diagnostics.Debug.WriteLine(Body);
                }
#endif
            }
            return "";
        }



        //public static Dictionary<string, Suggestion> DeserializeJsonBody(string json)
        //{
        //    try
        //    {
        //        JsonSerializer serializer = new JsonSerializer();
        //        StringReader sr = new StringReader(json);
        //        //object o = serializer.Deserialize(new JsonTextReader(sr),typeof(Dictionary<string, Suggestion>));
        //        ///object o = serializer.Deserialize(new JsonTextReader(sr), typeof(Dictionary<string, Suggestion>));
        //        return serializer.Deserialize(new JsonTextReader(sr), typeof(Dictionary<string, Suggestion>)) as Dictionary<string, Suggestion>;
        //    }
        //    catch (Exception e)
        //    {
        //        return new Dictionary<string, StompFomart.Suggestion>();
        //    }
        //}
    }
}
