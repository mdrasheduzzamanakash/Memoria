using AutoMapper;
using Memoria.DataService.IConfiguration;
using MemoriaMVC.ViewModel.GroupEditing;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using MemoriaMVC.SocketConnections;

namespace MemoriaMVC.Controllers
{
    public class GroupEditingController : BaseController<GroupEditingController>
    {
        private readonly IHubContext<NoteChangeBroadCastHub> _hubContext;
        public GroupEditingController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<GroupEditingController> logger,
            IHubContext<NoteChangeBroadCastHub> hubContext
         ) : base(unitOfWork, mapper, logger)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GroupEdit()
        {
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
