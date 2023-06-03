using AutoMapper;
using Memoria.DataService.IConfiguration;
using MemoriaMVC.ViewModel.GroupEditing;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MemoriaMVC.Controllers
{
    public class GroupEditingController : BaseController<GroupEditingController>
    {
        public GroupEditingController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GroupEditingController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GroupEdit() { 
            var writerIdentityId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.Users.GetByIdentityId(new Guid(writerIdentityId));

            var viewModel = new GroupEditViewModel()
            {
                noteId = user.ActiveEditingNote,
                writerId = user.Id
            };
            return View(viewModel);
        }

    }
}
