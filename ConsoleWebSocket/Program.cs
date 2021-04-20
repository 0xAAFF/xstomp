using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;

/*
 * XStompServer Base On WebSocketSharp(https://github.com/sta/websocket-sharp)
 * You Need :
 *      add import websocketsharp.dll
 *      using WebSocketSharp;
 *      using WebSocketSharp.Server;
 *      
 */

namespace ConsoleStompServer
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             * TODO 获取80端口的程序  然后杀掉对应进程的pid
             */
            WebSocketServer wsServer = new WebSocketServer(System.Net.IPAddress.Loopback,80, false);
            wsServer.AddWebSocketService<StompServer>("/stompserver");

            wsServer.Start();

            if (wsServer.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", wsServer.Port);
                foreach (var path in wsServer.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }



            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();

            wsServer.Stop();

        }
    }
}
