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
    public interface IUserRepository
    {

        // Add an entity
        Task<bool> Add(UserSingleInDTO userCreationDTO);
        Task<UserSingleOutDTO> GetById(string id);
        Task<UserSingleOutDTO> getByEmail(string email);
        Task<IEnumerable<UserSingleOutDTO>> All();
        Task<bool> Delete(string id);
        Task<bool> Upsert(UserSingleInDTO userCreationDTO, string userId);
        Task<UserSingleOutDTO> GetByIdentityId(Guid identityId);

        Task<bool> AddActiveNote(string userId,string noteId);
        Task<IEnumerable<UserCollaboratorSearchResultDto>> SearchByEmail(string searchText, string userId);

        Task<List<UserDetailsSingleOutDTO>> GetUsersDetails(List<string> userIds);

        Task<UserDetailsSingleOutDTO> GetSingleUserDetails(string userId);
    }
}
