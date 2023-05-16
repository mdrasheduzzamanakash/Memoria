using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Memoria.DataService.Data;
using Memoria.Entities.DbSet;
using Memoria.DataService.IConfiguration;
using AutoMapper;
using Memoria.Entities.DTOs.Outgoing;
using Memoria.Entities.DTOs.Incomming;

namespace MemoriaMVC.Controllers
{
    public class NotesController : BaseController<NotesController>
    {
        public NotesController(IUnitOfWork unitOfWork ,IMapper mapper, ILogger<NotesController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        // GET: Notes
        [HttpGet]
        public async Task<IActionResult> GetPartialViewAsync(string id)
        {
            // add labels to the viewbag
            var labels = await _unitOfWork.Labels.AllUserLabels(id);
            ViewBag.Labels = labels;

            foreach (var label in labels)
            {
                await Console.Out.WriteLineAsync(label.Content);
            }
            return PartialView("_NoteCreationModal");
        }

        [HttpGet]
        public async Task<IActionResult> CreateDraftNote(NoteSingleInDTO note)
        {
            var draftNote = await _unitOfWork.Notes.AddDraftNote(note);
            await _unitOfWork.CompleteAsync();
            return Json(draftNote);
        }

        [HttpGet]
        public async Task<IActionResult> SaveNote(NoteSingleInDTO note)
        {
            await _unitOfWork.Notes.Add(note);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
        /*
        // GET: Notes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _unitOfWork.Notes == null)
            {
                return NotFound();
            }

            var note = await _unitOfWork.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Title,Description,Todos,TrashingDate,BgColor,IsHidden,IsTrashed,IsArchived,IsPinned,IsMarked,IsRemainderAdded,RemainderDateTime,Id,Status,AddedDateAndTime,UpdatedDateAndTime,UpdatedBy,AddedBy,FileFormat")] Note note)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Add(note);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _unitOfWork.Notes == null)
            {
                return NotFound();
            }

            var note = await _unitOfWork.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Type,Title,Description,Todos,TrashingDate,BgColor,IsHidden,IsTrashed,IsArchived,IsPinned,IsMarked,IsRemainderAdded,RemainderDateTime,Id,Status,AddedDateAndTime,UpdatedDateAndTime,UpdatedBy,AddedBy,FileFormat")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Update(note);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _unitOfWork.Notes == null)
            {
                return NotFound();
            }

            var note = await _unitOfWork.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_unitOfWork.Notes == null)
            {
                return Problem("Entity set 'AppDbContext.Notes'  is null.");
            }
            var note = await _unitOfWork.Notes.FindAsync(id);
            if (note != null)
            {
                _unitOfWork.Notes.Remove(note);
            }
            
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(string id)
        {
          return (_unitOfWork.Notes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        */
    }
}
