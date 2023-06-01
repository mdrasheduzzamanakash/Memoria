using AutoMapper;
using Memoria.DataService.IConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaMVC.Controllers
{
    public class LandingPageController : BaseController<LandingPageController>
    {
        public LandingPageController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LandingPageController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
