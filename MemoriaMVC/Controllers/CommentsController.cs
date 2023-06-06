using AutoMapper;
using Memoria.DataService.IConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaMVC.Controllers
{
    public class CommentsController : BaseController<CommentsController>
    {
        public CommentsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentsController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsOfANote(string noteId)
        {
            var comments = await _unitOfWork.Comments.GetComments(noteId);
            return Json(comments);
        }

    }
}
