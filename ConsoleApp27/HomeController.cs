using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleApp27
{
    public class HomeController : ApiController
    {
        static List<Product> modelList = new List<Product>()
        {
            new Product(){Id=1,Name="电脑",Description="电器"},
            new Product(){Id=2,Name="冰箱",Description="电器"},
        };

        //获取所有数据
        [HttpGet]
        public List<Product> GetAll()
        {
            return modelList;
        }

        //获取一条数据
        [HttpGet]
        public Product GetOne(int id)
        {
            return modelList.FirstOrDefault(p => p.Id == id);
        }

        //新增
        [HttpPost]
        public bool PostNew(Product model)
        {
            modelList.Add(model);
            return true;
        }

        //删除
        [HttpDelete]
        public bool Delete(int id)
        {
            return modelList.Remove(modelList.Find(p => p.Id == id));
        }

        //更新
        [HttpPut]
        public bool PutOne(Product model)
        {
            Product editModel = modelList.Find(p => p.Id == model.Id);
            editModel.Name = model.Name;
            editModel.Description = model.Description;
            return true;
        }
    }

    public class Product
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
    }
}
