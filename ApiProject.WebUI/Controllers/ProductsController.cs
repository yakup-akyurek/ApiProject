using ApiProject.WebUI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace ApiProject.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ProductList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7100/api/Product");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(values);
            }
            //var error = await responseMessage.Content.ReadAsStringAsync();
            //ModelState.AddModelError("", $"API hata döndü: {responseMessage.StatusCode} - {error}");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var client = _httpClientFactory.CreateClient();//httpclientfactory ile client oluşturduk
            var responseMessage = await client.GetAsync("https://localhost:7100/api/Category");//apiye istek attık
            var jsonData = await responseMessage.Content.ReadAsStringAsync();//istekte bulunan datayı jsondan string çevirdik
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);//string datayı dtoya çevirdik
            List<SelectListItem> values2 = (from x in values//dto dan gelen values
                                            select new SelectListItem//selectlistitem oluşturduk
                                            {
                                                Text = x.categoryName,//görünen kısım
                                                Value = x.categoryId.ToString()//seçilen kısım
                                            }).ToList();//dto datayı selectlistitem a çevirdik

            ViewBag.v = values2;//viewbag ile view a gönderdik
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            var client = _httpClientFactory.CreateClient();//httpclientfactory ile client oluşturduk
            var jsonData = JsonConvert.SerializeObject(createProductDto);//dto yu json a çevirdik
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");//json datayı stringcontent e çevirdik
            var responseMessage = await client.PostAsync(
                "https://localhost:7100/api/Product",
                stringContent);//apiye post isteği attık

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductList");
            }
            else
            {
                var error = await responseMessage.Content.ReadAsStringAsync();//hata mesajını okuduk
                ModelState.AddModelError("", $"API hata döndü: {responseMessage.StatusCode} - {error}");//modelstate e hata ekledik
                return View(createProductDto);//başarısız ise aynı view e dto ile geri döndük

            }
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync("https://localhost:7100/api/Product?id=" + id);//apiye delete isteği attık
            if (responseMessage.IsSuccessStatusCode)//istek başarılı ise
            {
                return RedirectToAction("ProductList");
            }
            else
            {
                var error = await responseMessage.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"API hata döndü: {responseMessage.StatusCode} - {error}");
                return RedirectToAction("ProductList");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // 1) Kategorileri al
            var categoryResponse = await client.GetAsync("https://localhost:7100/api/Category");
            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
            var categories = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(categoryJson);

            ViewBag.v = categories
                .Select(x => new SelectListItem
                {
                    Text = x.categoryName,
                    Value = x.categoryId.ToString()
                })
                .ToList();

            // 2) Ürünü al
            var productResponse = await client.GetAsync($"https://localhost:7100/api/Product/{id}");

            if (!productResponse.IsSuccessStatusCode)
            {
                var error = await productResponse.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"API hata döndü: {productResponse.StatusCode} - {error}");
                return View(); // Boş sayfa, üstte hata gözükecek
            }

            var productJson = await productResponse.Content.ReadAsStringAsync();
            Console.WriteLine(productJson); // debug için

            var updatedValue = JsonConvert.DeserializeObject<UpdateProduct>(productJson);

            return View(updatedValue);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProduct updateProduct)
        {
            var client = _httpClientFactory.CreateClient();

            // Burada updateProduct.ProductId dolu olmalı
            var responseMessage = await client.PutAsJsonAsync(
                "https://localhost:7100/api/Product",
                updateProduct);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductList");
            }

            // Hata olursa kategorileri tekrar doldur
            var categoryResponse = await client.GetAsync("https://localhost:7100/api/Category");
            if (categoryResponse.IsSuccessStatusCode)
            {
                var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(categoryJson);

                ViewBag.v = categories
                    .Select(x => new SelectListItem
                    {
                        Text = x.categoryName,
                        Value = x.categoryId.ToString()
                    })
                    .ToList();
            }

            var error = await responseMessage.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"API hata döndü: {responseMessage.StatusCode} - {error}");

            return View(updateProduct);
        }

    }
}
