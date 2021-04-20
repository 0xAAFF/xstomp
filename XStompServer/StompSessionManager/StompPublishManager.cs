using System;
using System.Collections.Generic;
using System.Linq;
using XStomp;
using XStomp.Server.Frame;

namespace XStompServer.StompSessionManager
{
    public sealed class StompPublishManager
    {

        #region Instance
        private static StompPublishManager instance = null;
        private static readonly object stompPublishSessionManagerLock = new object();
        StompPublishManager() { }
        public static StompPublishManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (stompPublishSessionManagerLock)
                    {
                        if (instance == null)
                        {
                            instance = new StompPublishManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Private Fields
        private object rootListLock = new object();
        private List<string> toAllDestinationRootList = new List<string>();
        private List<string> toApplicationRootList = new List<string>();
        private List<string> toUsersRootList = new List<string>();

        private string guid = Guid.NewGuid().ToString();
        private long id = 0;
        private object guidLock = new object();

        // subscribe Supported TopicList
        private List<string> subscribeSupportTopicList = new List<string>();//支持的订阅地址列表
        // 订阅地址
        private Dictionary<string, Dictionary<string, StompBehavior>> stompSubscribeDictionary = new Dictionary<string, Dictionary<string, StompBehavior>>();//<>
        private object stompSubscribeDictionaryLock = new object();

        #endregion

        public string MessageID
        {
            get 
             {
                if (id > 0xFFFFFFFF)
                {
                    guid= Guid.NewGuid().ToString();
                    id = 0;
                }

                return guid + "-" + id++;
            }
        }


        #region Config Broadcast Message Type 
        /// <summary>
        /// 配置群发的根地址
        /// <para>以这个地址开头的destination都将产生的消息群发(客户端需要订阅相应的地址)</para>
        /// <para>例如增加一个"/topic",以"/topic"开头的地址产生的消息都将被群发到所有连接且订阅相应地址的客户端上</para>
        /// <para>"/topic","/topic/a","/topic/bb","/topic/ccc","/topic/a/aaa","/topic/aaaa/bbb","/topic/a/aaa/aaaa"订阅这些地址的客户端都将收到对应的信息</para>
        /// </summary>
        /// <param name="destinationRoot">destinationRoot</param>
        public void AddToAllDestination(params string[] destinationRoots)
        {
            lock (rootListLock)
            {
                foreach (string d in destinationRoots)
                {
                    string destinationRoot = d;
                    if (!destinationRoot.EndsWith("/"))
                    {
                        destinationRoot +=  "/";
                    }
                    if (!toAllDestinationRootList.Contains(destinationRoot))
                    {
                        toAllDestinationRootList.Add(destinationRoot);
                    }
                }
            }
        }

        /// <summary>
        /// 配置单发的根地址
        /// <para>以这个地址开头的destination只返回信息给原客户端,不会群发</para>
        /// <para>例如增加一个"/application",以"/application"开头的地址都将返回给源客户端</para>
        /// <para>"/application","/application/a","/application/bb","/application/ccc","/application/a/aaa","/application/aaaa/bbb","/application/a/aaa/aaaa"只返回给源客户端</para>
        /// </summary>
        /// <param name="destinationRoot">destinationRoot</param>
        public void AddToApplicationDestination(params string[] destinationRoots)
        {
            lock (rootListLock)
            {
                foreach (string d in destinationRoots)
                {
                    string destinationRoot = d;
                    if (!destinationRoot.EndsWith("/"))
                    {
                        destinationRoot += "/";
                    }
                    if (!toApplicationRootList.Contains(destinationRoot))
                    {
                        toApplicationRootList.Add(destinationRoot);
                    }
                }
            }
        }

        /// <summary>
        /// 配置组发给已登录客户端的根地址
        /// <para>以这个地址开头的destination将产生的消息群发给已登录的客户端(客户端需要订阅相应的地址)</para>
        /// <para>例如增加一个"/user",以"/user"开头的地址产生的消息都将被群发到所有连接且订阅相应地址的客户端上</para>
        /// <para>"/user","/user/a","/user/bb","/user/ccc","/user/a/aaa","/user/aaaa/bbb","/user/a/aaa/aaaa"订阅这些地址的客户端且客户端被认为登录都将收到对应的信息</para>
        /// </summary>
        /// <param name="destinationRoot">destinationRoot</param>
        public void AddToUsersDestination(params string[] destinationRoots)
        {
            lock (rootListLock)
            {
                foreach (string d in destinationRoots)
                {
                    string destinationRoot = d;
                    if (!destinationRoot.EndsWith("/"))
                    {
                        destinationRoot += "/";
                    }
                    if (!toUsersRootList.Contains(destinationRoot))
                    {
                        toUsersRootList.Add(destinationRoot);
                    }
                }
            }
        }

        /// <summary>
        /// 根据订阅地址获取应该将信息发送方式
        /// <para> All > Application > Users </para>
        /// </summary>
        /// <param name="destination">destination</param>
        /// <returns></returns>
        public SubscribeFlowDirction GetSubscribeFlowDirction(string destination)
        {
            int index = destination.IndexOf('/', 1);
            if (index > -1)
            {
                destination = destination.Substring(0, index + 1);
            }
            string root = destination;
            if (toAllDestinationRootList.Contains(root))
            {
                return SubscribeFlowDirction.All;
            }
            else if (toApplicationRootList.Contains(root))
            {
                return SubscribeFlowDirction.Application;
            }
            else if (toUsersRootList.Contains(root))
            {
                return SubscribeFlowDirction.Users;
            }
            else
            {
                return SubscribeFlowDirction.Unkonw;
            }
        }


        #endregion



        /// <summary>
        /// 添加服务端支持的destination
        /// </summary>
        /// <param name="destination">支持的destination</param>
        public void AddSubscribeDestination(params string[] destinations)
        {
            lock (stompSubscribeDictionaryLock)
            {
                foreach (string destination in destinations)
                {
                    if (!subscribeSupportTopicList.Contains(destination))
                    {
                        subscribeSupportTopicList.Add(destination);
                    }
                }
            }
        }


        /// <summary>
        /// 添加服务端支持的destination列表
        /// </summary>
        /// <param name="destinationList">支持的destination列表</param>
        public void AddSubscribeDestination(List<string> destinationList)
        {
            lock (stompSubscribeDictionaryLock)
            {
                foreach (string destination in destinationList)
                {
                    if (!subscribeSupportTopicList.Contains(destination))
                    {
                        subscribeSupportTopicList.Add(destination);
                    }
                }
            }
        }
        
        /// <summary>
        /// 服务端移出不想支持的destination 函数可能多余了
        /// </summary>
        /// <param name="destination">要停止支持的destination</param>
        public void RemoveSubscribeDestination(string destination)
        {
            lock (stompSubscribeDictionaryLock)
            {
                if (subscribeSupportTopicList.Contains(destination))
                {
                    subscribeSupportTopicList.Remove(destination);

                    if (stompSubscribeDictionary.ContainsKey(destination))
                    {
                        stompSubscribeDictionary.Remove(destination);
                    }
                }
            }
        }

        /// <summary>
        /// 添加一个新的订阅地址
        /// </summary>
        /// <param name="subScribeFrame">客户端的订阅数据包</param>
        /// <param name="stompBehavior">客户端连接对象</param>
        /// <returns>null:Success,not NULL:ERROR</returns>
        public StompFrame SubScribe(StompFrame subScribeFrame, StompBehavior stompBehavior)
        {
             //* If the server cannot successfully create the subscription, the server MUST send the client an ERROR frame and disconnect the client.
            lock (stompSubscribeDictionaryLock)
            {
                if (!subScribeFrame.Headers.ContainsKey(StompHeaders.Destination))
                {
                    return new ErrorFrame("Headers missing:'destination'", "SUBSCRIBE 包必须携带'destination'头,而且值不能为空.\nSUBSCRIBE Frame's headers MUST Contain 'destination',and the value MUST NOT null/empty", subScribeFrame);
                }
                else if (string.IsNullOrEmpty(subScribeFrame.Headers[StompHeaders.Destination]))
                {
                    return new ErrorFrame("Header['destination'] MUST NOT be null or empty", "SUBSCRIBE 包规定header['destination']的值不能为空.\nSUBSCRIBE Frame's header['destination']: MUST NOT be null/empty", subScribeFrame);
                }
                else if (!subscribeSupportTopicList.Contains(subScribeFrame.Headers[StompHeaders.Destination]))
                {
                    return new ErrorFrame("Stomp Server not support this destination", "Stomp Server not support this destination : '" + subScribeFrame.Headers[StompHeaders.Destination] + "'\n", subScribeFrame);
                }
                else if (!subScribeFrame.Headers.ContainsKey(StompHeaders.Id))//subid
                {
                    return new ErrorFrame("Headers missing: 'id'", "SUBSCRIBE 包必须携带'id'头,而且值不能为空.\nSUBSCRIBE Frame's headers MUST Contain 'id',and the value MUST uniquely" + "\nSince a single connection can have multiple open subscriptions with a server, an id header MUST be included in the frame to uniquely identify the subscription.", subScribeFrame);
                }
                else if (string.IsNullOrEmpty(subScribeFrame.Headers[StompHeaders.Id]))
                {
                    return new ErrorFrame("Header['id'] MUST NOT be null or empty,and MUST uniquely.", "SUBSCRIBE 包必须携带'id'头,而且值必须唯一.\nSUBSCRIBE Frame's header['id'] MUST NOT null/empty,and MUST uniquely.", subScribeFrame);
                }
                if (stompBehavior.topicPathSubidDictionary.Values.Contains(subScribeFrame.Headers[StompHeaders.Id]))// check id
                {
                    if (stompBehavior.topicPathSubidDictionary.Keys.Contains(subScribeFrame.Headers[StompHeaders.Destination]) && stompBehavior.topicPathSubidDictionary[subScribeFrame.Headers[StompHeaders.Destination]] == subScribeFrame.Headers[StompHeaders.Id])
                    {
                        return null;
                    }
                    else
                    {
                        // subid 已经用在了别的地址上
                        string destinationOfsubid = stompBehavior.topicPathSubidDictionary.FirstOrDefault(d => d.Value == subScribeFrame.Headers[StompHeaders.Id]).Key;
                        return new ErrorFrame("SUBSCRIBE: ['id'] MUST uniquely", "this sub-id is alread been used with this destination:" + destinationOfsubid + ".\n sub-id MUST uniquely,please use another id", subScribeFrame);

                        //stompBehavior.topicPathSubidDictionary[subScribeFrame.Headers[StompHeaders.Destination]] = subScribeFrame.Headers[StompHeaders.Id];//update sub-id
                    }
                }
                
                if (stompBehavior.topicPathSubidDictionary.ContainsKey(subScribeFrame.Headers[StompHeaders.Destination]))//check destination
                {
                    // 已经订阅过,id都匹配
                    if (stompBehavior.topicPathSubidDictionary[subScribeFrame.Headers[StompHeaders.Destination]] == subScribeFrame.Headers[StompHeaders.Id])
                    {
                        return null;
                    }
                    else //换了一个sub-id
                    {
                        // destination已经匹配了一个sub-id
                        //return new ErrorFrame("SUBSCRIBE: ['destination'] already linked a sub-id", "this destination is alread linked a sub-id:" + stompBehavior.topicPathSubidDictionary[subScribeFrame.Headers[StompHeaders.Destination]] + ".\n Stomp Server Don't want client Subscribe one destination by many times.", subScribeFrame);


                        // update sub-id
                        stompBehavior.topicPathSubidDictionary[subScribeFrame.Headers[StompHeaders.Destination]] = subScribeFrame.Headers[StompHeaders.Id];
                    }
                }

                //
                // ack ... Not support!
                //


                //
                // Stomp1.0
                // The body of the SUBSCRIBE command is ignored.
                //




                if (!stompBehavior.topicPathSubidDictionary.ContainsKey(subScribeFrame.Headers[StompHeaders.Destination]))
                {
                    stompBehavior.topicPathSubidDictionary.Add(subScribeFrame.Headers[StompHeaders.Destination], subScribeFrame.Headers[StompHeaders.Id]);//stompBehavior<destination:id> Add
                }

                //SubscribeFlowDirction subscribeFlowDirction = GetSubscribeFlowDirction(subScribeFrame.Headers[StompHeaders.Destination]);
                //if (subscribeFlowDirction == SubscribeFlowDirction.Application || subscribeFlowDirction == SubscribeFlowDirction.Unkonw)//Unkonw  is Error ?
                //{
                //    return null;
                //}


                if (stompSubscribeDictionary.ContainsKey(subScribeFrame.Headers[StompHeaders.Destination]))
                {
                    var idStompBehaviorKV = stompSubscribeDictionary[subScribeFrame.Headers[StompHeaders.Destination]];
                    if (!idStompBehaviorKV.ContainsKey(stompBehavior.ID))
                    {
                        stompSubscribeDictionary[subScribeFrame.Headers[StompHeaders.Destination]].Add(stompBehavior.ID, stompBehavior);
                    }
                }
                else
                {
                    stompSubscribeDictionary.Add(subScribeFrame.Headers[StompHeaders.Destination], new Dictionary<string, StompBehavior>() { { stompBehavior.ID, stompBehavior } });
                }
            }
            return null;
        }

        /// <summary>
        /// 客户端取消订阅
        /// </summary>
        /// <param name="unSubScribeFrame"></param>
        /// <param name="stompBehavior"></param>
        public void UnSubScribe(StompFrame unSubScribeFrame, StompBehavior stompBehavior)
        {
            lock (stompSubscribeDictionaryLock)
            {
                if (unSubScribeFrame.Headers.ContainsKey(StompHeaders.Id) && string.IsNullOrEmpty(unSubScribeFrame.Headers[StompHeaders.Id]))
                {
                    string destinationOfsubid = stompBehavior.topicPathSubidDictionary.FirstOrDefault(d => d.Value == unSubScribeFrame.Headers[StompHeaders.Id]).Key;
                    if (stompSubscribeDictionary.ContainsKey(destinationOfsubid))
                    {
                        if (stompSubscribeDictionary[destinationOfsubid].ContainsKey(stompBehavior.ID))
                        {
                            stompSubscribeDictionary[destinationOfsubid].Remove(stompBehavior.ID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// when stompBehavior closing,need remove subscribe
        /// </summary>
        /// <param name="stompBehavior"></param>
        public void StompBehaviorOnClose(StompBehavior stompBehavior)
        {
            lock (stompSubscribeDictionaryLock)
            {
                foreach (var destination in stompBehavior.topicPathSubidDictionary.Keys)
                {
                    if (stompSubscribeDictionary.ContainsKey(destination))
                    {
                        if (stompSubscribeDictionary[destination].ContainsKey(stompBehavior.ID))
                        {
                            stompSubscribeDictionary[destination].Remove(stompBehavior.ID);

                            if (stompSubscribeDictionary[destination].Count == 0)
                            {
                                stompSubscribeDictionary.Remove(destination);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 用来发布订阅的内容.
        /// <para>Header MUST contain ['destination'],['message-id'] 必须有</para>
        /// <para>If header include ["subscription"],will be replace 如果有[subscription]头信息,会被定向替代,就算设置了也会被替换掉</para>
        /// <para>this frame header["subscription"] will be auto set client sub-id , if contain</para>
        /// </summary>
        /// <param name="stompMessageFrame"></param>
        public void Publish(MessageFrame stompMessageFrame)
        {
            lock (stompSubscribeDictionaryLock)
            {
                if (stompMessageFrame.Command == StompCommands.Message && stompMessageFrame.Headers.ContainsKey(StompHeaders.Destination) && !string.IsNullOrEmpty(stompMessageFrame.Headers[StompHeaders.Destination]) && stompMessageFrame.Headers.ContainsKey(StompHeaders.MessageId) && !string.IsNullOrEmpty(stompMessageFrame.Headers[StompHeaders.MessageId]))
                {
                    if (stompSubscribeDictionary.ContainsKey(stompMessageFrame.Headers[StompHeaders.Destination]))
                    {
                        SubscribeFlowDirction subscribeFlowDirction = GetSubscribeFlowDirction(stompMessageFrame.Headers[StompHeaders.Destination]);

                        switch (subscribeFlowDirction)
                        {
                            case SubscribeFlowDirction.All:
                                {
                                    foreach (var idStompBehavior in stompSubscribeDictionary[stompMessageFrame.Headers[StompHeaders.Destination]].ToList())
                                    {
                                        try
                                        {
                                            idStompBehavior.Value.Publish(stompMessageFrame);
                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }
                                    break;
                                }
                            case SubscribeFlowDirction.Users:
                                {
                                    foreach (var idStompBehavior in stompSubscribeDictionary[stompMessageFrame.Headers[StompHeaders.Destination]].ToList())
                                    {
                                        try
                                        {
                                            if (idStompBehavior.Value.IsUser)
                                            {
                                                idStompBehavior.Value.Publish(stompMessageFrame);
                                            }
                                        }
                                        catch
                                        {
                                            //
                                        }
                                    }
                                    break;
                                }
                            default:
                                {
                                    return;
                                }
                        }
                    }
                }
            }
        }


    }
}
