using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XStomp;
using XStomp.Server.Frame;
using XStompServer;

namespace ConsoleStompServer
{
    class StompServer : StompBehavior
    {
        public StompServer()
        {
            // 群发 单发  组发 地址头设置
            PublishManager.AddToAllDestination("/topic","/toAll/", "/broadcast");
            PublishManager.AddToApplicationDestination("/application", "/applications/");
            PublishManager.AddToUsersDestination("/user");



            // 注册订阅地址            
            //PublishManager.AddSubscribeDestination("/topic/cmd", "/application/control", "/application/cmd");            
            //or
            PublishManager.AddSubscribeDestination("/topic/cmd");
            PublishManager.AddSubscribeDestination("/application/control");// 
            PublishManager.AddSubscribeDestination("/application/cmd");//
        }

        public override void Reflex(StompFrame sourceStomp)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(sourceStomp.ToString());
#endif
            switch (sourceStomp.Headers[StompHeaders.Destination])
            {
                case "/application/cmd"://  ---> /topic/cmd
                    {
                        try
                        {
                            MessageFrame messageFrame = new MessageFrame("/topic/cmd", MessageID, topicPathSubidDictionary[sourceStomp.Headers[StompHeaders.Destination]], null, sourceStomp.Body + ":Application OK", null);
                            PublishManager.Publish(messageFrame);
                        }
                        catch(Exception ex)
                        {
#if DEBUG
                            System.Diagnostics.Debug.WriteLine(ex.ToString());
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                            System.Diagnostics.Debug.WriteLine(ex.Source);
#endif
                        }
                        break;
                    }
                case "/application/control":// ---> /application/control
                    {
                        try
                        {
                            MessageFrame messageFrame = new MessageFrame("/application/control", MessageID, topicPathSubidDictionary[sourceStomp.Headers[StompHeaders.Destination]], null, sourceStomp.Body + ": Control OK", null);
                            Publish(messageFrame);
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            System.Diagnostics.Debug.WriteLine(ex.ToString());
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                            System.Diagnostics.Debug.WriteLine(ex.Source);
#endif
                        }
                        break;
                    }
            }
        }
    }
}
