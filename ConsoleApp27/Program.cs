using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace ConsoleApp27
{
    class Program
    {


        static string ID = Guid.NewGuid().ToString();
        static ConsulClient consulClient = null;

        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(cancelHandler, true);


            var config = new HttpSelfHostConfiguration("http://localhost:5000"); //配置主机

            config.Routes.MapHttpRoute(    //配置路由
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            using (HttpSelfHostServer server = new HttpSelfHostServer(config)) //监听HTTP
            {

                consulClient = new ConsulClient(x => x.Address = new Uri("http://127.0.0.1:8500"));//请求注册的 Consul 地址
                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(2),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                    HTTP = $"http://localhost:5000/api/health",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(2)
                };
                // Register service with consul
                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    ID = ID,
                    Name = "webapiTest",
                    Address = "localhost",
                    Port = 5000,
                    //Tags = new[] { $"urlprefix-/{serviceEntity.ServiceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                };
                consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）

                server.OpenAsync().Wait(); //开启来自客户端的请求
                Console.WriteLine("Press Enter to quit");
                Console.ReadLine();
            }
        }

        private static void Console_CancelKeyPress()
        {
            consulClient.Agent.ServiceDeregister(ID).Wait();//服务停止时取消注册
        }


        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);
        private static ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

        public static bool HandlerRoutine(int CtrlType)
        {
            switch (CtrlType)
            {
                case 0:
                    Console_CancelKeyPress(); //Ctrl+C关闭  
                    break;
                case 2:
                    Console_CancelKeyPress();//按控制台关闭按钮关闭  
                    break;
            }
            return false;
        }
    }
}
