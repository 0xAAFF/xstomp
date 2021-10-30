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
using System.Text;

namespace XStomp.Server
{
    public class StompServerConfig
    {
        #region Instance
        private static StompServerConfig instance = null;
        private static readonly object stompServerConfigLock = new object();
        StompServerConfig() { }
        public static StompServerConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (stompServerConfigLock)
                    {
                        if (instance == null)
                        {
                            instance = new StompServerConfig();
                        }

                    }
                }
                return instance;
            }
        }
        #endregion

        //  header
        private static readonly int maxHeadersCount = 30;
        private static readonly int maxHeaderLineLength = 1024;
        private static readonly int maxBodyLineLenth = 3 * 1024 * 1024 * 5;
        private static readonly Encoding serverCharset = Encoding.UTF8;

        public static int AllowMaxHeaderCount
        {
            get => maxHeadersCount;
        }

        public static int AllowMaxHeaderLineLength
        {
            get => maxHeaderLineLength;
        }

        public static int AllowBodyLineLenth
        {
            get => maxBodyLineLenth;
        }



        //
        public static Encoding ServerCharset { get => serverCharset; }





    }
}
