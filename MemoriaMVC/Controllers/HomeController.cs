using AutoMapper;
using Memoria.DataService.IConfiguration;
using MemoriaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MemoriaMVC.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HomeController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        public IActionResult Index()
        {
            Console.WriteLine("Hi------fdassssssssssssssssssssssssssssssssssss");
            return View();
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