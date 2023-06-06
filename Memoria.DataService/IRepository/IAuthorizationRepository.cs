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
    public interface IAuthorizationRepository
    {
        Task<bool> AddAuthorization(AuthorizationSingleInDTO authorizationSingleInDTO);

        Task<bool> RemoveAuthorization(AuthorizationSingleInDTO authorizationSingleInDTO);

        Task<List<AuthorizationSingleOutDTO>> GetAllAuthorizationsOfUser(string authorId);

        Task<AuthorizationSingleOutDTO> GetAuthorization(string noteId, string userId);

        Task<List<AuthorizationSingleOutDTO>> GetAllAuthorizationsOfANote(string noteId);
    }
}
