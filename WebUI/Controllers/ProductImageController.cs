using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text.Json;
using WebUI.DTOs.BrandDto;
using WebUI.DTOs.ProductDto;
using WebUI.DTOs.ProductImageDto;

namespace AdminPanel.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductImageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7228/api/Brand");

            if (!response.IsSuccessStatusCode)
            {
                return View();
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BrandResponseDto>(jsonData);
            var brands = result.Brands;

            ViewBag.Brands = brands.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name // Burada Value olaraq BrandName göndəririk, çünki AJAX ondan istifadə edir
            }).ToList();

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetProductsByBrand(string brandName)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7228/api/Product/GetProductsByBrandName?BrandName={brandName}");
            if (!response.IsSuccessStatusCode) return Json(new List<object>());

            var json = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<ProductResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // sadəcə lazım olan məlumatları qaytarırıq (id və name)
            var products = result.Products.Select(p => new
            {
                id = p.Id,
                name = p.Name
            });

            return Json(products);
        }



        [HttpPost]
        public async Task<IActionResult> Upload(UploadProductImageDto model)
        {
            if (model.Files == null || !model.Files.Any())
            {
                ModelState.AddModelError("", "Zəhmət olmasa ən azı 1 şəkil seçin.");
                return View(model);
            }

            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.ProductId.ToString()), "ProductId");

            foreach (var file in model.Files)
            {
                var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(streamContent, "Files", file.FileName);
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("https://localhost:7228/api/ProductImage", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Şəkillər uğurla yükləndi!";
                return RedirectToAction("Upload");
            }

            ModelState.AddModelError("", "Yükləmə zamanı xəta baş verdi.");
            return View(model);
        }

      
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7228/api/ProductImage/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Şəkil uğurla silindi!";
            }
            else
            {
                TempData["Error"] = "Şəkili silmək mümkün olmadı.";
            }

            return RedirectToAction("Upload");
        }
    }
}
