using Microsoft.AspNetCore.Mvc;
using Memoria.DataService.IConfiguration;
using AutoMapper;
using Memoria.Entities.DTOs.Incomming;
using Newtonsoft.Json;
using MemoriaMVC.ViewModel.HomePageViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Authentication.Configuration;
using System.Security.Claims;

namespace MemoriaMVC.Controllers
{
    public class NotesController : BaseController<NotesController>
    {
        public NotesController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<NotesController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        // get all the notes 
        [HttpGet]
        public async Task<IActionResult> AllWithOutDraft(string authorId)
        {
            var notes = await _unitOfWork.Notes.AllNotesWithOutDraft(authorId);
            return Json(notes);
        }

        [HttpGet]
        public async Task<IActionResult> RedirectToWrite(string noteId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _unitOfWork.Users.AddActiveNote(userId, noteId);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("GroupEdit", "GroupEditing");
        }



        [HttpGet]
        public async Task<IActionResult> AllSharedNotes(string authorId)
        {

            var authorizedEntities = await _unitOfWork.Authorizations.GetAllAuthorizationsOfUser(authorId);

            var uniqueIds = new HashSet<string>(authorizedEntities.Select(x => x.NoteId));
            var uniqueIdsList = uniqueIds.ToList();

            var sharedNotes = await _unitOfWork.Notes.GetNotesWithIds(uniqueIdsList);

            return Json(sharedNotes);
        }




        [HttpGet]
        public async Task<IActionResult> AllTrashedNotes(string authorId)
        {
            var notes = await _unitOfWork.Notes.AllNotesTrashed(authorId);
            return Json(notes);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string noteId)
        {
            var note = await _unitOfWork.Notes.GetNoteById(noteId);
            return Json(note);
        }

        [HttpGet]
        public async Task<IActionResult> PermanentlyDeleteAnItem(string noteId, string userId)
        {
            var statusOne = await _unitOfWork.Notes.DeleteAnItem(noteId);
            var statusTwo = await _unitOfWork.Attachments.DeleteAllAttachmentOfANote(noteId, userId);
            var staus = statusOne && statusTwo;
            await _unitOfWork.CompleteAsync();
            return Json(staus);
        }
        

        // get partial view        
        [HttpGet]
        public async Task<IActionResult> GetPartialViewAsync(string id)
        {
            // add labels to the viewbag
            var labels = await _unitOfWork.Labels.AllUserLabels(id);
            ViewBag.Labels = labels;
            return PartialView("_NoteCreationModal");
        }

        [HttpGet]
        public async Task<IActionResult> GetPartialViewUpdation(string id)
        {
            // add labels to the viewbag
            var labels = await _unitOfWork.Labels.AllUserLabels(id);
            ViewBag.Labels = labels;
            return PartialView("_NoteUpdationAndDeletion");
        }

        [HttpGet]
        public async Task<IActionResult> GetPartialViewShared(string id)
        {
            // add labels to the viewbag
            var labels = await _unitOfWork.Labels.AllUserLabels(id);
            ViewBag.Labels = labels;
            return PartialView("_NoteSharedModal");
        }




        [HttpGet]
        public async Task<IActionResult> GetPartialViewTrash(string id)
        {
            // add labels to the viewbag
            var labels = await _unitOfWork.Labels.AllUserLabels(id);
            ViewBag.Labels = labels;
            return PartialView("_NoteTrashModal");
        }




        // create draft note from empty note
        [HttpPost]
        public async Task<IActionResult> CreateDraftNote([FromBody] NoteSingleInDTO note)
        {
            var draftNote = await _unitOfWork.Notes.AddDraftNote(note);
            await _unitOfWork.CompleteAsync();
            return Json(draftNote);
        }

        // Save note using updated note
        [HttpPost]
        public async Task<IActionResult> SaveNote([FromBody] NoteSingleInDTO finalNoteDto)
        {
            finalNoteDto.UpdatedDateAndTime = DateTime.UtcNow;
            var finalNote = await _unitOfWork.Notes.AddFinalNote(finalNoteDto);
            await _unitOfWork.CompleteAsync();
            return Json(finalNote);
        }

        [HttpGet]
        public async Task<IActionResult> SearchedByTitleAndDescription([FromQuery] string searchText, [FromQuery] string userId)
        {
            var searchedNotes = await _unitOfWork.Notes.SearchByTitleAndDescription(searchText, userId);
            return Json(searchedNotes);
        }

        [HttpGet]
        public async Task<IActionResult> SearchedByTitleAndDescriptionTrash([FromQuery] string searchText, [FromQuery] string userId)
        {
            var searchedNotes = await _unitOfWork.Notes.SearchByTitleAndDescriptionTrash(searchText, userId);
            return Json(searchedNotes);
        }



        [HttpGet]
        public async Task<IActionResult> Trash()
        {
            ViewBag.Title = "Trash Page";
            var identityId = User.FindFirst("Id")?.Value;
            var user = await _unitOfWork.Users.GetByIdentityId(new Guid(identityId));
            if(user == null)
            {
                RedirectToAction("Login", "Accounts");
            }
            var userViewModel = _mapper.Map<HomeIndexViewModel>(user);
            if(userViewModel == null)
            {
                RedirectToAction("Login", "Accounts");
            }
            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Share()
        {
            ViewBag.Title = "Share Page";
            var identityId = User.FindFirst("Id")?.Value;
            var user = await _unitOfWork.Users.GetByIdentityId(new Guid(identityId));
            var userViewModel = _mapper.Map<HomeIndexViewModel>(user);
            return View(userViewModel);
        }

    }
}
