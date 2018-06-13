using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleApp27
{
    public class HealthController : ApiController
    {

        //获取所有数据
        [HttpGet]
        public string GetAll()
        {
            return "";
        }
    }
}
