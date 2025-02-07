using JollyAdminClient.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using JollyAPI.Models.Entity;
using JollyAPI.Models.DTOS;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Newtonsoft.Json;
using JollyAPI.Services;
using Microsoft.EntityFrameworkCore;
using JollyAPI.Helper;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JollyAdminClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client;
        private string productApiUrl;
        private string categoryApiUrl;

        public ProductController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            productApiUrl = AppConstant.API_URL + "/products";
            categoryApiUrl = AppConstant.API_URL + "/categories";
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(productApiUrl + "/get-all");
            List<Product> products = new List<Product>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                products = response.Content.ReadFromJsonAsync<List<Product>>().Result;
            }
            ViewBag.products = products;
            ViewBag.pageTitle = "Quản lý sản phẩm";
            ViewBag.JsFiles = new List<string> { "list.js", "product.js" };

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync(productApiUrl + "/get-product/" + id);
            Product product = new Product();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                product = response.Content.ReadFromJsonAsync<Product>().Result;
            }
            HttpResponseMessage response2 = await client.GetAsync(productApiUrl + "/get-color/" + id);
            List<Color> color = new List<Color>();
            if (response2.StatusCode == System.Net.HttpStatusCode.OK)
            {
                color = response2.Content.ReadFromJsonAsync<List<Color>>().Result;
            }

            ViewBag.colors = color;
            ViewData["pageTitle"] = "Xem chi tiết sản phẩm";
            return View(product);
        }

        public async Task<IActionResult> Reviews(int id, int? size = null, int? page = null)
        {
            HttpResponseMessage response = await client.GetAsync(productApiUrl + "/" + id + "/reviews");
            List<Rating> ratings = new List<Rating>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ratings = response.Content.ReadFromJsonAsync<List<Rating>>().Result;
            }
            ViewBag.ratings = ratings;
            ViewData["pageTitle"] = "Xem đánh giá sản phẩm";
            ViewBag.JsFiles = new List<string> { "list.js" };

            return View(ratings);
        }

        public async Task<IActionResult> Create()
        {
            HttpResponseMessage response = await client.GetAsync(categoryApiUrl + "/get-all");
            List<Category>? categories = new List<Category>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                categories = response.Content.ReadFromJsonAsync<List<Category>>().Result;
            }
            HttpResponseMessage colorResponse = await client.GetAsync(productApiUrl + "/get-all-color");
            List<Color>? colors = new List<Color>();
            if (colorResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                colors = colorResponse.Content.ReadFromJsonAsync<List<Color>>().Result;
            }
            List<SelectListItem> genderList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Men", Text = "Men" },
                new SelectListItem { Value = "Women", Text = "Women" },
                new SelectListItem { Value = "Children", Text = "Children" }
            };
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Colors = new SelectList(colors, "Id", "Name");
            ViewBag.Genders = new SelectList(genderList, "Value", "Text");
            ViewBag.pageTitle = "Thêm mới sản phẩm";
            ViewBag.JsFiles = new List<string> { "product.js" };


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDTO product, List<IFormFile> imageFiles)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var file in imageFiles)
                    {
                        if (!(file.ContentType == "image/png" ||
                              file.ContentType == "image/jpeg" ||
                              file.ContentType == "image/gif"))
                        {
                            return RedirectToAction("Create");
                        }
                    }

                    

                    var formData = new MultipartFormDataContent();

                    foreach (var property in typeof(ProductCreateDTO).GetProperties())
                    {
                        formData.Add(new StringContent(property.GetValue(product)?.ToString()), property.Name);
                    }

                    foreach (var file in imageFiles)
                    {
                        formData.Add(new StreamContent(file.OpenReadStream())
                        {
                            Headers =
                    {
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "imageFiles",
                            FileName = file.FileName
                        }
                    }
                        }, "imageFiles");
                    }

                    HttpResponseMessage productResponse = await client.PostAsync(productApiUrl + "/create", formData);

                    if (productResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Create");

                    }

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Create");
                }
            }
            ViewBag.JsFiles = new List<string> { "product.js" };

            return RedirectToAction("Create");

        }


        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage productDetailResponse = await client.GetAsync(productApiUrl + "/get-product/" + id);
            ProductUpdateDTO product = new ProductUpdateDTO();
            if (productDetailResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                product = productDetailResponse.Content.ReadFromJsonAsync<ProductUpdateDTO>().Result;
            }
            HttpResponseMessage response = await client.GetAsync(categoryApiUrl + "/get-all");
            List<Category>? categories = new List<Category>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                categories = response.Content.ReadFromJsonAsync<List<Category>>().Result;
            }
            HttpResponseMessage colorResponse = await client.GetAsync(productApiUrl + "/get-all-color");
            List<Color>? colors = new List<Color>();
            if (colorResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                colors = colorResponse.Content.ReadFromJsonAsync<List<Color>>().Result;
            }

            HttpResponseMessage imageResponse = await client.GetAsync(productApiUrl + "/get-image/" + id);
            List<Image> images = new List<Image>();
            if (imageResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                images = imageResponse.Content.ReadFromJsonAsync<List<Image>>().Result;
            }
            List<SelectListItem> genderList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Men", Text = "Men" },
                new SelectListItem { Value = "Women", Text = "Women" },
                new SelectListItem { Value = "Children", Text = "Children" }
            };
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Colors = new SelectList(colors, "Id", "Name");
            ViewBag.Genders = new SelectList(genderList, "Value", "Text");
            ViewBag.Images = images;
            ViewData["pageTitle"] = "Sửa sản phẩm";
            ViewBag.JsFiles = new List<string> { "product.js" };


            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductUpdateDTO p, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var file in imageFiles)
                    {
                        if (!(file.ContentType == "image/png" ||
                              file.ContentType == "image/jpeg" ||
                              file.ContentType == "image/gif"))
                        {
                            return RedirectToAction("Create?fail");
                        }
                    }

                    var formData = new MultipartFormDataContent();

                    foreach (var property in typeof(ProductUpdateDTO).GetProperties())
                    {
                        formData.Add(new StringContent(property.GetValue(p)?.ToString()), property.Name);
                    }

                    foreach (var file in imageFiles)
                    {
                        formData.Add(new StreamContent(file.OpenReadStream())
                        {
                            Headers =
                    {
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "imageFiles",
                            FileName = file.FileName
                        }
                    }
                        }, "imageFiles");
                    }

                    HttpResponseMessage productResponse = await client.PutAsync(productApiUrl + "/edit/" + id, formData);

                    if (productResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Edit", new { id = id });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error occurred while processing the request. " + ex.Message;
                }
            }
            ViewBag.JsFiles = new List<string> { "product.js" };

            return View(p);
        }


        public async Task<IActionResult> Delete(int id)
        {

            await client.DeleteAsync(productApiUrl + "/delete/" + id);

            return RedirectToAction("Index");

        }
    }
}
