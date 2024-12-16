using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;
using WebPicture.Models;

namespace WebPicture.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly DBmanager _dbManager;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            ViewData["img"] = "~/image/cc.jpg";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Picture()
        {
            var filePath = Path.Combine(_env.WebRootPath, "imageData.json");
            if (!System.IO.File.Exists(filePath))
            {
                return Json(new { message = "File not found" });
            }
            var jsonString = System.IO.File.ReadAllText(filePath);
            var items = JsonSerializer.Deserialize<List<Item>>(jsonString);
            ViewData["img"] = items;
            return View();
        }

        [HttpPost]
        public IActionResult addAccount(string jsonItems)
        {
            var items = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(jsonItems);
            DBmanager dbmanager = new DBmanager();
            try
            {
                foreach (var item in items)
                {
                    dbmanager.newAccount(item);
                }
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
            }
            return RedirectToAction("Picture");
        }

        public IActionResult ImageLook()
        {
            var filePath = Path.Combine(_env.WebRootPath, "imageData.json");
            if (!System.IO.File.Exists(filePath))
            {
                return Json(new { message = "File not found" });
            }
            var jsonString = System.IO.File.ReadAllText(filePath);
            var items = JsonSerializer.Deserialize<List<Item>>(jsonString);
            ViewData["img"] = items;
            //var data = items.Skip(page * pageSize).Take(pageSize).ToList();
            return View();
        }

        //public IActionResult ImageLook(Object test)
        //{
        //    return View();
        //}

        public JsonResult GetItem(int page, int pageSize)
        {
            var filePath = Path.Combine(_env.WebRootPath, "imageData.json");
            if (!System.IO.File.Exists(filePath)){
                return Json(new { message = "File not found" });
            }
            var jsonString = System.IO.File.ReadAllText(filePath);
            var items = JsonSerializer.Deserialize<List<Item>>(jsonString);
            ViewData["img"] = items;
            var data = items.Skip(page * pageSize).Take(pageSize).ToList();
            return Json (data);

        }

        //[HttpPost]
        //[Route("items/import")]
        //public IActionResult ImportItem()
        //{
        //    var filePath = Path.Combine(_env.WebRootPath, "imageData.json");
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return Json(new { message = "File not found" });
        //    }
        //    try
        //    {
        //        var jsonString = System.IO.File.ReadAllText(filePath);
        //        var items = JsonSerializer.Deserialize<List<Item>>(jsonString);

        //        // 將資料存入資料庫
        //        _dbManager.newAccount(items);

        //        return Json(new { message = "Data saved successfully" });
        //    }catch(Exception e)
        //    {
        //        // 返回錯誤訊息
        //        return Json(new { message = "Error saving data", error = e.Message });
        //    }
        //}



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
