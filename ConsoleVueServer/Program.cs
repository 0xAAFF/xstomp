using Server.Vue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleVueServer
{
    class Program
    {
        static void Main(string[] args)
        {
            VueServer vueServer = new VueServer(80);

            vueServer.httpsv.AddWebSocketService<StompServer>("/stompserver");

            vueServer.Start();



            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();
            vueServer.Stop();

        }
    }
}
