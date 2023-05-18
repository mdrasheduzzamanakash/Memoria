using AutoMapper;
using Memoria.DataService.Data;
using Memoria.DataService.IRepository;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.Repository
{
    internal class NoteRepository : GenericRepository<Note>, INoteRepository
    {
        public NoteRepository(AppDbContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<NoteSingleOutDTO?> AddDraftNote(UserSingleOutDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var note = new Note();
            note.IsRemainderAdded = false;
            var status = await base.Add(note);
            if(status)
            {
                var noteDto = _mapper.Map<NoteSingleOutDTO>(note);
                return noteDto;
            } else
            {
                return null;
            }
        }
        public async Task<bool> Add(NoteSingleInDTO noteDto)
        {
            var note = _mapper.Map<Note>(noteDto);
            await base.Add(note);
            return true;
        }

        public async Task<NoteSingleOutDTO?> AddDraftNote(NoteSingleInDTO noteDto)
        {
            var note = _mapper.Map<Note>(noteDto);
            await base.Add(note);
            var noteOutDto = _mapper.Map<NoteSingleOutDTO>(note);
            return noteOutDto;
        }

        public async Task<NoteSingleOutDTO?> AddFinalNote(NoteSingleInDTO noteDto)
        {
            var finalNote = _mapper.Map<Note>(noteDto);
            await base.Upsert(finalNote, finalNote.Id);
            var finalNoteOutDto = _mapper.Map<NoteSingleOutDTO>(finalNote);
            return finalNoteOutDto;
        }

        public async Task<List<NoteSingleOutDTO>> AllNotesWithOutDraft(string authorId)
        {
            var notes = await _dbSet.Where(x => x.AuthorId == authorId && x.IsDraft == false).ToListAsync();
            var notesDto = new List<NoteSingleOutDTO>();
            foreach (var note in notes)
            {
                var noteDto = _mapper.Map<NoteSingleOutDTO>(note);
                notesDto.Add(noteDto);
            }
            return notesDto;
        }
    }
}
