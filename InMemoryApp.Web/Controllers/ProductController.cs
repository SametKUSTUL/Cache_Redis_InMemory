using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            #region 1.Yol
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //{
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //} 
            #endregion

            #region 2.Yol

            MemoryCacheEntryOptions options = new();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            options.SlidingExpiration=TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High;
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value}=>sebep:{reason}");

            });



            _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 200 };

            _memoryCache.Set<Product>("product1", product);

            #endregion

            return View();
        }

        public IActionResult Show()
        {
            #region CacheTemizleme
            //_memoryCache.Remove("time"); 
            #endregion


            #region CacheOlusturmaFunc
            //_memoryCache.GetOrCreate<string>("time", entry => 
            //{ 
            //    return DateTime.Now.ToString(); 
            //}); 
            #endregion


            _memoryCache.TryGetValue("time", out string cacheTime);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.zaman = cacheTime ?? $"Cache 'time' key'ine ait veri bulunamadı! ";
            ViewBag.callback = callback ?? $"Callback Yok ";

            ViewBag.product=_memoryCache.Get<Product>("product1");

            return View();

        }
    }
}
