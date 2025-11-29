using ApiProject.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProject.WebUI.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;//DI ile httpclientfactory çağırdık

        public CategoriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CategoryList()
        {

            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7100/api/Category");//apiye istek attık
            if (responseMessage.IsSuccessStatusCode)//istek başarılı ise
            {
               var jsonData=await responseMessage.Content.ReadAsStringAsync();//istekte bulunan datayı jsondan string çevirdik
                var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);//string datayı dtoya çevirdik
                return View(values);
            }
            return View();//başarısız ise boş view döndük
        }
    }
}
