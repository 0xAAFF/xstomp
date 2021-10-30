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
    public static class ServerSupport
    {
        public static string SupportVersion
        {
            get => "1.0,1.1,1.2";
        }

        /// <summary>
        /// Server Support Versions & Client Support Versions Max Version.if not Compare, retur ""
        /// </summary>
        /// <param name="acceptVersion">Client CONNECT Frame Header["accept-version"] Value</param>
        /// <returns>Server and Client Both Support Max Version </returns>
        public static string CompareMaxVersion(string acceptVersion)
        {
            if (acceptVersion != null && acceptVersion != "")
            {
                foreach (var v in acceptVersion.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Reverse())
                {
                    if (v == "1.2" || v == "1.1" || v == "1.0")
                    {
                        return v;
                    }
                }
                return "";
            }
            else
            {
                return "1.0";
            }
        }

        public static bool Heart_BeatSupport
        {
            get => false;
        }


    }
}
