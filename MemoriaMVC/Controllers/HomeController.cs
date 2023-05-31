using Authentication.Configuration;
using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Outgoing;
using MemoriaMVC.ViewModel.HomePageViewModel;
using MemoriaMVC.ViewModel.UserPageViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace MemoriaMVC.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HomeController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated == true)
            {
                ViewBag.Title = "Home Page";

                var identityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _unitOfWork.Users.GetByIdentityId(new Guid(identityId));
                var userViewModel = _mapper.Map<HomeIndexViewModel>(user);
                return View(userViewModel);
            } 
            else
            {
                string returnUrl = "/Home/Index";
                var routeValue = new RouteValueDictionary(new { retController = "Home", retAction = "Index"});

                return RedirectToAction("RefreshToken", "Accounts", routeValue);
            }
        }

    }
}
