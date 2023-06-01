using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Incomming;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaMVC.Controllers
{
    public class AuthorizationsController : BaseController<AuthorizationsController>
    {
        public AuthorizationsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthorizationsController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthorization([FromBody] AuthorizationSingleInDTO authorizationSingleInDTO)
        {
            if (ModelState.IsValid)
            {
                var status = await _unitOfWork.Authorizations.AddAuthorization(authorizationSingleInDTO);
                await _unitOfWork.CompleteAsync();
                return Json(status);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAuthorization([FromBody] AuthorizationSingleInDTO authorizationSingleInDTO)
        {
            if(ModelState.IsValid)
            {
                var status = await _unitOfWork.Authorizations.RemoveAuthorization(authorizationSingleInDTO);
                await _unitOfWork.CompleteAsync();
                return Json(status);
            } 
            else
            {
                return BadRequest();
            }
        }
    }
}
