using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Editora.MVC.ApiClientHelper
{
    public class ApiClient
    {
        public HttpClient Client { get; set; }

        public ApiClient()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:44337/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
