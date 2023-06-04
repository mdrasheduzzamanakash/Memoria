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

        Task<List<NoteSingleOutDTO>> AllNotesTrashed(string id);

        Task<NoteSingleOutDTO> GetNoteById(string id);

        Task<List<NoteSingleOutDTO>> GetNotesWithIds(List<string> ids);

        Task<List<NoteSingleOutDTO>> SearchByTitleAndDescription(string searchText, string userId);

        Task<List<NoteSingleOutDTO>> SearchByTitleAndDescriptionTrash(string searchText, string userId);

        Task<bool> DeleteAnItem(string noteId);

        Task<List<NoteSingleOutDTO>> TrashedNotesOlderThan30Days();

        Task RemoveRange(List<NoteSingleOutDTO> notes);

        Task<bool> ModifyTitleOrDescription(string noteId, string title, string description, bool isTitle);
    }
}
