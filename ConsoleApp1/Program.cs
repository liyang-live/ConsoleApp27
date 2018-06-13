using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri("http://127.0.0.1:8500"));//请求注册的 Consul 地址
            var result = consulClient.Catalog.Service("webapiTest").Result;

            foreach (var item in result.Response)
            {
                Console.WriteLine(item.ServiceAddress+"  "+item.ServicePort);
            }

        }
    }
}
