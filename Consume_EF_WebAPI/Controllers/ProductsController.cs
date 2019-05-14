using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consume_EF_WebAPI;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Microsoft.IdentityModel.Clients.ActiveDirectory; //ADAL client library for getting the access token
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Consume_EF_WebAPI.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public  ActionResult Index()
        {
           
             var products =GetProductsfromApi();
            return View(products);
         }
       

        public ActionResult AddorEditNew(int id =0)
        {
            if(id ==0)
            {
                return View(new ProductEntity());
            }
            else
            {
                var client = new HttpClient();

                HttpResponseMessage response = client.GetAsync("http://localhost:49765/api/Productdetails/"+id.ToString()).Result;
                return View(response.Content.ReadAsAsync<ProductEntity>().Result);
            }

        }

        static string GetUserOAuthToken()
        {
            const string ResourceId = "https://storage.azure.com/";
            const string AuthInstance = "https://login.microsoftonline.com/{0}/";
            const string TenantId = "188285f7-8f1e-4c0d-a0bc-797e3e38c5b3"; // Tenant or directory ID

            // Construct the authority string from the Azure AD OAuth endpoint and the tenant ID. 
            string authority = string.Format(CultureInfo.InvariantCulture, AuthInstance, TenantId);
            AuthenticationContext authContext = new AuthenticationContext(authority);

            // Acquire an access token from Azure AD. 
            AuthenticationResult result = authContext.AcquireTokenAsync(ResourceId,
                                                                        "ecf70f09-f560-400e-b901-6bb87995afdb",
                                                                        new Uri(@"https://wppwebapp.com/auth"),
                                                                        new PlatformParameters(PromptBehavior.Auto)).Result;

            return result.AccessToken;
        }

        [HttpPost]
        public  ActionResult AddorEditNew(ProductEntity prod)
        {
           
            var client = new HttpClient();

            if(prod.id == 0)
            {
                HttpResponseMessage response =client.PostAsJsonAsync("http://localhost:49765/api/Productdetails", prod).Result;

                return RedirectToAction("Index");
            }
            else
            {
                HttpResponseMessage response = client.PutAsJsonAsync("http://localhost:49765/api/Productdetails/"+prod.id, prod).Result;

                return RedirectToAction("Index");
            }

           

        }

        public ActionResult Delete(int id)
        {
            var client = new HttpClient();
            HttpResponseMessage response = client.DeleteAsync("http://localhost:49765/api/Productdetails/" + id.ToString()).Result;
            return RedirectToAction("Index");
        }

        //private async Task<List<ProductEntity>> GetProductsfromApi()
           private  List<ProductEntity> GetProductsfromApi()
        {
            try
            {
                var client = new HttpClient();

                var resultlist = new List<ProductEntity>();

                var getdatatask = client.GetAsync("http://localhost:49765/api/Productdetails")
                    .ContinueWith( response =>
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

                return  resultlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}