using Microsoft.AspNetCore.Mvc;
using Memoria.DataService.IConfiguration;
using AutoMapper;
using Memoria.Entities.DTOs.Incomming;
using Newtonsoft.Json;

namespace MemoriaMVC.Controllers
{
    public class NotesController : BaseController<NotesController>
    {
        public NotesController(IUnitOfWork unitOfWork ,IMapper mapper, ILogger<NotesController> logger) : base(unitOfWork, mapper, logger)
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
        public async Task<IActionResult> GetById(string noteId)
        {
            var note = await _unitOfWork.Notes.GetNoteById(noteId);
            return Json(note);
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
            var finalNote = await _unitOfWork.Notes.AddFinalNote(finalNoteDto);
            await _unitOfWork.CompleteAsync();
            return Json(finalNote);
        }

        [HttpGet]
        public async Task<IActionResult> SearchedByTitleAndDescription([FromQuery]string searchText, [FromQuery] string userId)
        {
            var searchedNotes = await _unitOfWork.Notes.SearchByTitleAndDescription(searchText, userId);
            return Json(searchedNotes);
        }
    }
}
