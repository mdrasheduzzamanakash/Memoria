using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.IRepository
{
    public interface INoteRepository
    {
        Task<NoteSingleOutDTO?> AddDraftNote(NoteSingleInDTO noteDto);

        Task<NoteSingleOutDTO?> AddFinalNote(NoteSingleInDTO noteDto);

        Task<bool> Add(NoteSingleInDTO noteDto);

        Task<List<NoteSingleOutDTO>> AllNotesWithOutDraft(string id);

        Task<NoteSingleOutDTO> GetNoteById(string id);
    }
}
