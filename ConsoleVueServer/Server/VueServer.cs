using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Server.Vue
{
    public class VueServer
    {
        public VueServer(int port)
        {
            this.port = port;
            httpsv = new HttpServer(IPAddress.Loopback, port);
        }

        //Dictionary<string, MAction> dic = new Dictionary<string, MAction>();
        //IContainer container;
        public HttpServer httpsv;
        private int port;
        /// <summary>
        /// 跨域参数设置
        /// </summary>
        public string corsOptions { get; set; }

        /// <summary>
        /// 开启监听
        /// </summary>
        public void Start()
        {

            //httpsv.Log.Level = LogLevel.Trace;
            httpsv.OnGet += Httpsv_OnGet;
            //httpsv.OnPost += Httpsv_OnPost;
            httpsv.OnOptions += Httpsv_OnOptions;
            httpsv.Start();
        }
        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop()
        {
            httpsv.OnGet -= Httpsv_OnGet;
            //httpsv.OnPost -= Httpsv_OnPost;
            httpsv.OnOptions -= Httpsv_OnOptions;
            httpsv.Stop();
        }
        /// <summary>
        /// 跨域请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Httpsv_OnOptions(object sender, HttpRequestEventArgs e)
        {
            e.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            e.Response.Headers.Add("Access-Control-Allow-Methods", "POST,PUT,DELETE,GET");
            e.Response.Headers.Add("Access-Control-Allow-Headers", "*");
            e.Response.Headers.Add("Access-Control-Max-Age", "1728000");
            e.Response.Close();
        }


        #region
        public static string ROOT_PATH = AppDomain.CurrentDomain.BaseDirectory+"service\\";// CommonName.shared.dataPath;
        public static string CSS_PATH = "/static/css";
        public static string JS_PATH = "/static/js";
        public static string FONT_PATH = "/static/fonts";
        public static string IMG_PATH = "/static/img";
        public static string IMAGE_PATH = "/images";
        public static string MAIN_PATH = "index.html";



        private void Httpsv_OnPost(object sender, HttpRequestEventArgs e)
        {
            //MAction emm = new MAction();
            //string logResult = string.Empty;
            //try
            //{
            //    byte[] bf = new byte[e.Request.ContentLength64];
            //    string result = Encoding.UTF8.GetString(GetResult(e, bf));   //获取http请求内容
            //    logResult = result;
            //    if (dic.ContainsKey(e.Request.RawUrl))
            //    {
            //        emm = dic[e.Request.RawUrl];
            //        object obj;
            //        object[] objs = new object[emm.parameterInfo.Length];
            //        if (emm.parameterInfo.Length > 1)
            //        {
            //            throw new Exception("POST方法只允许有一个参数");
            //        }
            //        else if (emm.parameterInfo.Length == 0)
            //        {
            //            obj = new object();
            //        }
            //        else
            //        {
            //            obj = Newtonsoft.Json.JsonConvert.DeserializeObject(result, emm.parameterInfo[0].ParameterType);
            //            objs[0] = obj;
            //        }
            //        var action = container.Resolve(emm.controllerType);
            //        var data = emm.action.Invoke(action, objs);
            //        e.Response.ContentType = "application/json";
            //        if (data == null)
            //        {
            //            e.Response.StatusCode = 204;
            //        }
            //        else
            //        {
            //            byte[] buffer = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(data));
            //            e.Response.WriteContent(buffer);
            //        }
            //    }
            //    else
            //    {
            //        e.Response.StatusCode = 404;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (emm.exceptionFilter == null)
            //    {
            //        byte[] buffer = Encoding.UTF8.GetBytes(ex.Message + ex.StackTrace);
            //        e.Response.WriteContent(buffer);
            //        e.Response.StatusCode = 500;
            //    }
            //    else
            //    {
            //        if (emm.exceptionFilter != null)
            //        {
            //            if (ex.InnerException != null)
            //            {
            //                emm.exceptionFilter.OnException(ex.InnerException, e);
            //            }
            //            else
            //            {
            //                emm.exceptionFilter.OnException(ex, e);
            //            }
            //        }
            //    }
            //}
        }

        private void Httpsv_OnGet(object sender, HttpRequestEventArgs e)
        {
            var req = e.Request;
            var res = e.Response;
            
            string filePath = req.RawUrl == "/" ? MAIN_PATH : req.RawUrl;

            Console.WriteLine(req.Url);

            if (filePath.IndexOf("?") > 0)
            {
                filePath = filePath.Substring(0, filePath.IndexOf("?"));
                filePath = filePath == "/" ? MAIN_PATH : filePath;
            }

            //Console.WriteLine("req.RawUrl:" + req.RawUrl);

            try
            {
                if (Path.GetExtension(filePath).Length > 0)
                {
                    string path = ROOT_PATH + filePath;
                    byte[] contents;
                    if (File.Exists(path))
                    {
                        contents = File.ReadAllBytes(path);
                    }
                    else
                    {
                        res.StatusCode = (int)HttpStatusCode.NotFound;
                        return;
                    }
                    string contentType = MimeMapping.GetMimeMapping(path);
                    res.ContentType = contentType;
                    res.ContentEncoding = Encoding.UTF8;
                    res.ContentLength64 = contents.LongLength;
                    res.OutputStream.Write(contents, 0, contents.Length);
                    Console.WriteLine(filePath.PadRight(70) + " ------------------> [ OK ]");
                }
            }
            catch (Exception ex)
            {
                    byte[] buffer = Encoding.UTF8.GetBytes(ex.Message + ex.StackTrace);
                    e.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    e.Response.StatusCode = 500;
            }
            res.Close();
        }

        /// <summary>
        /// 获取数据结果
        /// </summary>
        /// <param name="e"></param>
        /// <param name="bf"></param>
        /// <returns></returns>
        private byte[] GetResult(HttpRequestEventArgs e, byte[] bf)
        {
            System.IO.Stream st = e.Request.InputStream;
            st.Read(bf, 0, bf.Length);

            string value = Encoding.UTF8.GetString(bf).TrimEnd((char)0);
            byte[] newbf = Encoding.UTF8.GetBytes(value);

            //判断数据是否已经下载完整
            if (newbf.Length < bf.Length)
            {
                GetResult(e, new byte[bf.Length - newbf.Length]).CopyTo(bf, newbf.Length);
            }
            return bf;
        }

        #endregion
    }

}