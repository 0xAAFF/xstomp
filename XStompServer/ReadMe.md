## XStompServer ##

XStompServer is base on *websocket-sharp.dll*.
so you need Use websocket-sharp(https://github.com/sta/websocket-sharp)


It is a STOMP Server,but now Only Support Connect/Subscribe/Unsubscribe/Send 

- .NET Framework **3.5** or later (includes compatible environment such as [Mono])


**XStompServer** 是一个STOMP服务端,基于.NET 3.5及以上的环境(主要是*websocket-sharp.dll*就支持最低3.5),是基于*websocket-sharp.dll*做的.


## 为什么要用**XStompServer** ##
- 项目中需要一个.NET语言开发的Server或者Service
- 多种语言开发的各种客户端(大部分浏览器支持的Web页面[IE9+,Other],windows客户端)
- 当某客户端更改了服务端的一个值,各个客户端必须同步这一值或状态(状态同步)
- 数据支持订阅/取消订阅
- 数据支持单发单收
- 数据支持单发群收
- 传输和接收数据的格式统一

## 为什么是STOMP协议 ##
- 数据格式和数据交互由一个统一的STOMP协议支持
- 大部分开发语言都有对应的客户端组件
- 协议简单易于理解
- 支持自定义的头信息
- ...


# 其他
Web端需要支持的Websocket,所以浏览器需要IE9及以上,谷歌浏览器,火狐等其他浏览器都会支持.所以在项目支持方面这点满足大部分的需求

## 使用 ##
#### Step 1 ####
Create New Class "StompServer"(Example),
- 新建一个StompServer类,然后继承StompBehavior,
- 在构造函数中,添加支持群发,单发,组发的根地址
- override void Reflex(StompFrame sourceStomp)然后实现对应地址的处理方式,如果产生了新的数据包,要发送给客户端,可以使用Publish()函数
```C#
using XStomp;
using XStomp.Server.Frame;
using XStompServer;

class StompServer : StompBehavior
{
    public StompServer()
    {
        // Add Root Path
        // 群发 单发  组发 地址头设置
        //PublishManager.AddToAllDestination("/topic","/toAll/", "/broadcast");   // 群发
        //PublishManager.AddToApplicationDestination("/application", "/applications/");//单发
        //PublishManager.AddToUsersDestination("/user");//已经认证登录成功的用户



        // 注册订阅地址            
        //PublishManager.AddSubscribeDestination("/topic/cmd", "/application/control", "/application/cmd");            
        //or
        /*
        PublishManager.AddSubscribeDestination("/topic/cmd");
        PublishManager.AddSubscribeDestination("/application/control");// 
        PublishManager.AddSubscribeDestination("/application/cmd");//
        */
    }
    public override void Reflex(StompFrame sourceStomp)
    {
// #if DEBUG
//             System.Diagnostics.Debug.WriteLine(sourceStomp.ToString());
// #endif
//             switch (sourceStomp.Headers[StompHeaders.Destination])
//             {
//                 case "/application/cmd"://  ---> /topic/cmd
//                     {
//                         try
//                         {
//                             MessageFrame messageFrame = new MessageFrame("/topic/cmd", MessageID, topicPathSubidDictionary[sourceStomp.Headers[StompHeaders.Destination]], null, sourceStomp.Body + ":Application OK", null);
//                             PublishManager.Publish(messageFrame);
//                         }
//                         catch(Exception ex)
//                         {
// #if DEBUG
//                             System.Diagnostics.Debug.WriteLine(ex.ToString());
//                             System.Diagnostics.Debug.WriteLine(ex.StackTrace);
//                             System.Diagnostics.Debug.WriteLine(ex.Source);
// #endif
//                         }
//                         break;
//                     }
//                 case "/application/control":// ---> /application/control
//                     {
//                         try
//                         {
//                             MessageFrame messageFrame = new MessageFrame("/application/control", MessageID, topicPathSubidDictionary[sourceStomp.Headers[StompHeaders.Destination]], null, sourceStomp.Body + ": Control OK", null);
//                             Publish(messageFrame);
//                         }
//                         catch (Exception ex)
//                         {
// #if DEBUG
//                             System.Diagnostics.Debug.WriteLine(ex.ToString());
//                             System.Diagnostics.Debug.WriteLine(ex.StackTrace);
//                             System.Diagnostics.Debug.WriteLine(ex.Source);
// #endif
//                         }
//                         break;
//                     }
//             }
    }

}

```

#### Step 2 ####

使用方式参考WebSocketSharp
```C#
//A
using WebSocketSharp.Server;
//B
namespace ConsoleWebSocket
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             * TODO 获取80端口的程序  然后杀掉对应进程的pid
             */
            WebSocketServer wsServer = new WebSocketServer(System.Net.IPAddress.Loopback,80, false);
            wsServer.AddWebSocketService<StompServer>("/stompserver"); //*******************************C*******************************
            wsServer.Start();
            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();
            wsServer.Stop();
        }
    }
}
```




# 关于开发
因为是基于开源的组件和开源代码的基础上做的,项目中也将放入参考的项目地址.
在XSTOMP中,使用和修改了开源的代码,项目地址也将放在一起.
如果有涉及到你的代码,但是没有注明的,请联系我<littletools@outlook.com>

如果你发现代码中有Bug,请联系我.

## License ##
websocket-sharp is provided under [The MIT License].具体请查阅此处(https://github.com/sta/websocket-sharp)

XStomp:
- Apache License 2.0 Copyright 2011 Ernst Naezer, et. al.(https://github.com/ernstnaezer/ultralight/blob/master/)
- Apache License 2.0 Copyright (c) 2014 Carlos Campo <carlos@campo.com.co>(https://github.com/krlito/StompNet/blob/master/StompNet)
- Apache License 2.0 Copyright (c) 2020 0xAAFF <littletools@outlook.com>



XStompServer is provided under [Apache License, Version 2.0]
/*******************************************************************************
 *
 *  Copyright (c) 2020 0xAAFF <littletools@outlook.com>
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


## 




