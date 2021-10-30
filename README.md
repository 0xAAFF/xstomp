# XStomp

是C#语言编写的组件:
**XStomp**和**XStompServer**


**XStomp** 支持.Net的各种版本


**XStompServer** 支持.NET3.5及以上,因为这个组件的server是基于**WebSocket-sharp**的,此组件最低支持.NET3.5


**XStomp**是对stomp协议的基本支持,包括了Stomp的数据帧的解析和对相应命令的数据帧的创建.


**XStompServer** Based on **WebSocket-sharp**


use sta's<https://github.com/sta> websocket-sharp project (https://github.com/sta/websocket-sharp)


use Carlos Campo's(https://github.com/krlito) StompNet project(https://github.com/krlito/StompNet) 


and use Ernst Naezer's(https://github.com/ernstnaezer) ultralight Project (https://github.com/ernstnaezer/ultralight)

## ConsoleVueServer
A Little WebServer,Http & WebSocket(Stomp)Server,Port:80
提供了80端口上的一个web服务和websocket服务.可以使用浏览器访问网页,也支持websocket交互(使用了stompserver)



## Useage

- Start ConsoleVueServer.exe
- Unzip service.zip(Create By Vue),put service/ with ConsoleVueServer.exe same Path.

- broswer->http://127.0.0.1 ->Preass F12(DeveloperTool) -> console see stomp Log

- broswer->http://127.0.0.1/?all=123 ->Preass F12(DeveloperTool) -> console see stomp Log
    (all=123 means Send Stomp to Server)


all client will reveive same stomp frame form ConsoleVueServer.exe


## 使用方式
1. 生成 ConsoleVueServer.exe
2. service.zip 解压到 ConsoleVueServer.exe 同目录下
3. 启动ConsoleVueServer.exe

4. 浏览器访问 http://127.0.0.1 和 http://127.0.0.1/?all=123
5. 浏览器上打开"开发者工具"->console
    all=123的页面会定时向 ConsoleVueServer.exe 发送Stomp包,服务端的机制是收到包后,返回一个包到所有的client上.

主要是用来测试 单发,群发,和组发,这个例子只是测试发送群发stomp包


## Copyright

```
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
 ```


 ## AboutMe

 [MyPage](https://www.jianshu.com/u/dbdb14e006b3)