using Microsoft.AspNetCore.Mvc;
using Memoria.DataService.IConfiguration;
using AutoMapper;
using Memoria.Entities.DTOs.Incomming;
using Newtonsoft.Json;
using MemoriaMVC.ViewModel.HomePageViewModel;

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
            var user = await _unitOfWork.Users.GetById("1ff4e1cd-6081-450d-abef-5c1667daf7f7");
            var userViewModel = _mapper.Map<HomeIndexViewModel>(user);
            return View(userViewModel);
        }

    }
}
