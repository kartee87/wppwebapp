using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consume_EF_WebAPI;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace Consume_EF_WebAPI.Controllers
{
    public class ProductDetailsController : Controller
    {
        // GET: ProductDetails
        public ActionResult Index()
        {
            var products = GetProductsfromApi();
            return View(products);
        }

        private List<ProductEntity> GetProductsfromApi()
        {
            try
            {
                var client = new HttpClient();

                var resultlist = new List<ProductEntity>();

                var getdatatask = client.GetAsync("http://localhost:49765/api/Productdetails")
                    .ContinueWith(response =>
                    {
                        var result = response.Result;

                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var readresult = result.Content.ReadAsAsync<List<ProductEntity>>();

                            readresult.Wait();
                            resultlist = readresult.Result;
                        }
                    });

                getdatatask.Wait();

                return resultlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}