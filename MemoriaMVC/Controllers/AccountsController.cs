using Authentication.Configuration;
using AutoMapper;
using Memoria.DataService.IConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MemoriaMVC.Controllers
{
    public class AccountsController : BaseController<AccountsController>
    {
        public AccountsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AccountsController> logger, UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionMonitor) : base(unitOfWork, mapper, logger, userManager, optionMonitor)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
