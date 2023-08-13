using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions? cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            Product? product = new Product { Id = 2, Name = "pencil", Price = 1160 };


            string? jsonproduct = JsonConvert.SerializeObject(product);


            Byte[]? byteproduct = Encoding.UTF8.GetBytes(jsonproduct);

            _distributedCache.Set("product:1", byteproduct);

            //await _distributedCache.SetStringAsync("product:2", jsonproduct, cacheEntryOptions);

            // _distributedCache.SetString("name", "Ruveyda", cacheEntryOptions);

            //await _distributedCache.SetStringAsync("surname", "Akçınar");
            return View();
        }


        public IActionResult Details()
        {

            Byte[]? byteProduct = _distributedCache.Get("product:1");

            string? jsonproduct = Encoding.UTF8.GetString(byteProduct);
            Product? p = JsonConvert.DeserializeObject<Product>(jsonproduct);

            //string? name = _distributedCache.GetString("name");
            //ViewBag.name = name;
            ViewBag.product = p;
            
            return View();
        }

        public IActionResult Remove() 
        {
            _distributedCache.Remove("name");
            return View(); 
        }

        public IActionResult imageUrl()
        {
            byte[]? imageByte = _distributedCache.Get("image");

            return File(imageByte,"image/png");
        }



        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.png");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image",imageByte);
            return View();
        }

    }

}
