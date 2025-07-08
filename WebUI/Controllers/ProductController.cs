using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using WebUI.DTOs.BrandDto;
using WebUI.DTOs.CategoryDto;
using WebUI.DTOs.ColorDto;
using WebUI.DTOs.GenderDto;
using WebUI.DTOs.ProductDto;
using WebUI.DTOs.ShapeDto;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(int page = 1, int? categoryId = null)
        {
            var pageSize = 10;
            var client = _httpClientFactory.CreateClient();

            // API url - filter varsa əlavə et
            var requestUrl = $"https://localhost:7228/api/Product/GetAllProductsWith?page={page - 1}&size={pageSize}";
            if (categoryId.HasValue)
            {
                requestUrl += $"&categoryId={categoryId.Value}";  // Kiçik "categoryId"
            }

            var responseMessage = await client.GetAsync(requestUrl);

            // Kategoriyaları da çəkirik
            var categoryResponse = await client.GetAsync("https://localhost:7228/api/Category");
            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
            var categoryData = JsonConvert.DeserializeObject<CategoryResponseDto>(categoryJson);

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductResponseDto>(jsonData);

                // Toplam səhifə sayını hesabla
                var totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

                var model = new ProductListViewModel
                {
                    Products = result.Products,
                    TotalCount = result.TotalCount,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    SelectedCategoryId = categoryId,
                    Categories = categoryData.Categories.Select(c => new CategoryItemDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
                };

                return View(model);
            }

            // Uğursuz olduqda boş model döndür
            return View(new ProductListViewModel
            {
                Products = new List<ResultProductDto>(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = 0,
                TotalPages = 0,
                SelectedCategoryId = categoryId,
                Categories = categoryData.Categories.Select(c => new CategoryItemDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            });
        }


        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var client = _httpClientFactory.CreateClient();

            // Kateqoriyalar
            var categoryResponse = await client.GetAsync("https://localhost:7228/api/Category");
            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
            var categoryData = JsonConvert.DeserializeObject<CategoryResponseDto>(categoryJson); 
            ViewBag.CategoryValues = categoryData.Categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            // Brendlər
            var brandResponse = await client.GetAsync("https://localhost:7228/api/Brand");
            var brandJson = await brandResponse.Content.ReadAsStringAsync();
            var brandData = JsonConvert.DeserializeObject<BrandResponseDto>(brandJson);
            ViewBag.BrandValues = brandData.Brands.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            // Genderlər
            var genderResponse = await client.GetAsync("https://localhost:7228/api/Gender");
            var genderJson = await genderResponse.Content.ReadAsStringAsync();
            var genderData = JsonConvert.DeserializeObject<GenderResponseDto>(genderJson);
            ViewBag.GenderValues = genderData.Genders.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            // Rənglər
            var colorResponse = await client.GetAsync("https://localhost:7228/api/Color");
            var colorJson = await colorResponse.Content.ReadAsStringAsync();
            var colorData = JsonConvert.DeserializeObject<ColorResponseDto>(colorJson);
            ViewBag.ColorValues = colorData.Colors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            // Forması
            var shapeResponse = await client.GetAsync("https://localhost:7228/api/Shape");
            var shapeJson = await shapeResponse.Content.ReadAsStringAsync();
            var shapeData = JsonConvert.DeserializeObject<ShapeResponseDto>(shapeJson);
            ViewBag.ShapeValues = shapeData.Shapes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto model)
        {
            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7228/api/Product", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync($"https://localhost:7228/api/Product/{id}");
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var jsonData = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<UpdateProductDto>(jsonData);

            // Category
            var catResponse = await client.GetAsync("https://localhost:7228/api/Category");
            var catJson = await catResponse.Content.ReadAsStringAsync();
            var catList = JsonConvert.DeserializeObject<CategoryResponseDto>(catJson)?.Categories;
            ViewBag.CategoryValues = catList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            // Brand
            var brandResponse = await client.GetAsync("https://localhost:7228/api/Brand");
            var brandJson = await brandResponse.Content.ReadAsStringAsync();
            var brandList = JsonConvert.DeserializeObject<BrandResponseDto>(brandJson)?.Brands;
            ViewBag.BrandValues = brandList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            // Gender
            var genderResponse = await client.GetAsync("https://localhost:7228/api/Gender");
            var genderJson = await genderResponse.Content.ReadAsStringAsync();
            var genderList = JsonConvert.DeserializeObject<GenderResponseDto>(genderJson)?.Genders;
            ViewBag.GenderValues = genderList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            // Color
            var colorResponse = await client.GetAsync("https://localhost:7228/api/Color");
            var colorJson = await colorResponse.Content.ReadAsStringAsync();
            var colorData = JsonConvert.DeserializeObject<ColorResponseDto>(colorJson);
            ViewBag.ColorValues = colorData.Colors.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = product.ColorIds != null && product.ColorIds.Contains(x.Id)
            }).ToList();

            // Shape
            var shapeResponse = await client.GetAsync("https://localhost:7228/api/Shape");
            var shapeJson = await shapeResponse.Content.ReadAsStringAsync();
            var shapeData = JsonConvert.DeserializeObject<ShapeResponseDto>(shapeJson);
            ViewBag.ShapeValues = shapeData.Shapes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandsByCategoryName(string categoryName)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7228/api/Brand/GetBrandsByCategoryName?categoryName={categoryName}");

            if (!response.IsSuccessStatusCode)
                return Json(new List<SelectListItem>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BrandResponseDto>(jsonData);

            var brandList = result.Brands.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            return Json(brandList);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto model)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("https://localhost:7228/api/Product", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7228/api/Product/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}