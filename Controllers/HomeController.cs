using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebsiteBanHang.Models;
using System.Linq;

namespace WebsiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        // Sửa lỗi: Truyền ApplicationDbContext vào constructor
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Gán context đúng cách
        }

        public IActionResult Index()
        {
            var categories = _context.Categories
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = _context.Products.Count(p => p.CategoryId == c.Id)
                })
                .ToList();

            ViewBag.Categories = categories;

            var products = _context.Products.ToList();
            return View(products);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
