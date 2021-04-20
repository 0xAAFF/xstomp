using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using XStomp;
using XStomp.Server;
using XStomp.Server.Frame;
using XStompServer.StompSessionManager;

namespace XStompServer
{
    /*
     * StompBehavior Based On websocket-sharp (https://github.com/sta/websocket-sharp)
     * So You Need :
     *  Add websocket-sharp in Project
     *  using WebSocketSharp;
     *  using WebSocketSharp.Server;
     * 
     */
    public class StompBehavior : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            if (isConnected)
            {
                // 此处使用了多线程处理
                Thread reflexThread = new Thread(
                    () =>
                    {
                        StompFrame StompFrame = StompFrameSerializer.Deserialize(e.Data);
                        OnStompProtocol(StompFrame);
                    });
                reflexThread.Start();
            }
            else
            {
                StompFrame StompFrame = StompFrameSerializer.Deserialize(e.Data);
                OnStompProtocol(StompFrame);
            }
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            PublishManager.StompBehaviorOnClose(this);
            Thread.Sleep(300);// server will likely only allow closed connections to linger for short time before the connection is reset.
            base.OnClose(e);
        }


        #region Stomp Support
        public Dictionary<string, string> topicPathSubidDictionary = new Dictionary<string, string>();// 存放订阅的客户端session  Key:Topic Path  Value:subid
        public Dictionary<string, string> topicSubidAckDictionary = new Dictionary<string, string>();//  存放订阅的sunid和对应的ack
        string stompVersion = "";
        string heartbeat = "";
        bool isConnected = false;
        private StompPublishManager publishManager = StompPublishManager.Instance;


        public StompPublishManager PublishManager
        {
            get
            {
                return publishManager;
            }
            protected set
            {
                publishManager = value;
            }
        }


        /// <summary>
        /// 是否是Stomp认证的用户
        /// </summary>
        public bool IsUser
        {
            get;private set;
        }

        public string MessageID
        {
            get => PublishManager.MessageID;
        }
        //
        #endregion

        private void OnStompProtocol(StompFrame stomp)
        {
            switch (stomp.Command)
            {
                case StompCommands.Connect:
                    {
                        /*
                         * 这里需要做一些协议支持处理 http://stomp.github.io/stomp-specification-1.1.html#Protocol_Negotiation
                         * 
                         * 可接受的协议 Done.
                         * host验证
                         * 
                         * login
                         * passcode 
                         */
                        /*
                         * TODO secured STOMP server
                         */
                        StompFrame connectedFrame = StompServerFactory.CreateConnectedFrame(stomp, ID);
                        SendStompMessage(connectedFrame);

                        if (connectedFrame.Command != StompCommands.Error)// or != StompCommands.Connected
                        {
                            stompVersion = connectedFrame[StompHeaders.Version];
                            heartbeat = connectedFrame[StompHeaders.Heartbeat];
                            // TODO if you May Support Heartbeat

                            // TODO if you May Support Host 
                        }
                        else
                        {
                            isConnected = true;
                        }
                        break;
                    }
                case StompCommands.Subscribe:
                    {
                        
                        StompFrame errorFrame = PublishManager.SubScribe(stomp, this);
                        if (errorFrame != null)
                        {
                            SendStompMessage(errorFrame);
                        }
                        else
                        {
                            Reflex(stomp);// Application
                        }
                        break;
                    }
                case StompCommands.Unsubscribe:
                    {
                        PublishManager.UnSubScribe(stomp, this);
                        break;
                    }
                case StompCommands.Send:
                    {
                        Reflex(stomp);
                        break;
                    }
                default: 
                    {
                        ErrorFrame stompFrame = new ErrorFrame("Stomp Server Can not Support This Command", "StompServer Only Support Connect/Subscribe/Unsubscribe/Send",stomp);
                        SendStompMessage(stompFrame);
                        break;
                    }
            }
        }


        /// <summary>
        /// 发布一个MESSAGE包
        /// </summary>
        /// <param name="stompMessageFrame"></param>
        public void Publish(MessageFrame stompMessageFrame)
        {
            if (stompMessageFrame.Headers.ContainsKey(StompHeaders.Destination) && topicPathSubidDictionary.ContainsKey(stompMessageFrame.Headers[StompHeaders.Destination]) && !string.IsNullOrEmpty(topicPathSubidDictionary[stompMessageFrame.Headers[StompHeaders.Destination]]))
            {
                // Append header subscription
                stompMessageFrame[StompHeaders.Subscription] = topicPathSubidDictionary[stompMessageFrame.Headers[StompHeaders.Destination]];//compare sub-id
                if (stompMessageFrame.Command == StompCommands.Message)
                {
                    SendStompMessage(stompMessageFrame);
                }
            }
        }

        private void SendStompMessage(StompFrame stompMessageFrame)
        {
            if (StompCommands.IsServerCommand(stompMessageFrame.Command))
            {
                try
                {
                    Send(StompFrameSerializer.Serialize(stompMessageFrame));
                }
                catch
                {
                    Close();// still Exceprion//
                }
            }

            /// If the server cannot successfully process the SEND frame for any reason, the server MUST send the client an ERROR frame and then close the connection.
            if (stompMessageFrame.Command == StompCommands.Error)
            {
                Thread.Sleep(500);
                this.Close();
            }
        }

        public virtual void Reflex(StompFrame sourceStomp)
        {

            //Example A
            //switch (sourceStomp[StompHeaders.Destination])
            //{
            //    case "/topic/":
            //        {
            //            //PublishManager.Publish();
            //            break;
            //        }
            //    case "/app":
            //        {
            //            //SendStompMessage()
            //            break;
            //        }
            //    case "/user/privateData":
            //        {
            //            //Do SomeThing
            //            // CreaterNew MessageFrame
            //            //PublishManager.Publish(newMessageFrame);
            //            break;
            //        }
            //    default:
            //        {
            //            StompFrame reflexFrame = new StompFrame(StompCommands.Error, "Can not Support");
            //            SendStompMessage(reflexFrame);
            //            break;
            //        }
            //}

            //Example B
            //if (sourceStomp[StompHeaders.Destination] == ConfigurationManager.AppSettings["messageToAllTips"])//app.config <add key="messageToAllTips" value="/topic/tips"/>
            //{
            //    //PublishManager.Publish(tipsMessage);
            //}
            //else if (sourceStomp[StompHeaders.Destination] == ConfigurationManager.AppSettings["messageToApplicationCMD"])//app.config <add key="messageToApplicationCMD" value="/application/cmd"/>
            //{
            //    //SendStompMessage(newCMDResultStompMessage);                
            //}
            //else if ()//
            //{ }
            //else
            //{ }
        }



    }
}
