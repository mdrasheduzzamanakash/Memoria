using AutoMapper;
using Memoria.DataService.IConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaMVC.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController<ProfileController>
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ProfileController(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<ProfileController> logger,
            UserManager<IdentityUser> userManager) : base(unitOfWork, mapper, logger)
        { 
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var loggedInUser = await _userManager.GetUserAsync(HttpContext.User);
            if(loggedInUser == null)
            {
                return NotFound();
            }
            var identityId = new Guid(loggedInUser.Id);
            var profile = await _unitOfWork.Users.GetByIdentityId(identityId);

            if(profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
    }
}
