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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        // Add a user
        public async Task<bool> Add(UserSingleInDTO entityDto)
        {
            // mapping entityDto to native entity
            var user = _mapper.Map<User>(entityDto);
            return await base.Add(user);
        }

        public async Task<UserSingleOutDTO> GetById(string id)
        {
            var user = await base.GetById(id);
            var resultDto = _mapper.Map<UserSingleOutDTO>(user);
            return resultDto;
        }

        public async Task<IEnumerable<UserSingleOutDTO>> All()
        {
            List<User> users = (List<User>) await base.All();
            if (users == null)
            {
                return Enumerable.Empty<UserSingleOutDTO>();
            }

            List<UserSingleOutDTO> resultDtos = new List<UserSingleOutDTO>();
            
            foreach (var user in users)
            {
                var resultDto = _mapper.Map<UserSingleOutDTO>(user);
                resultDtos.Add(resultDto);
            }
            return resultDtos;
        }
        
        public async Task<bool> Upsert(UserSingleInDTO userCreationDTO, string userId)
        {
            if(userId == null || userCreationDTO == null)
            {
                return false;
            }
            var user = _mapper.Map<User>(userCreationDTO);
            var existingUser = await _dbSet.FindAsync(user.Id);

            if (existingUser == null)
            {
                // User does not exist in the database, create one
                user.AddedBy = userId;
                await base.Add(user);
            }
            else
            {
                // User already exists in the database, so update the record
                user.AddedBy = existingUser.AddedBy;
                user.AddedDateAndTime = existingUser.AddedDateAndTime;
                user.UpdatedBy = userId;
                user.UpdatedDateAndTime = DateTime.UtcNow;
                _dbSet.Entry(existingUser).CurrentValues.SetValues(user);
            }
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            return await base.Delete(id);
        }

        public async Task<UserSingleOutDTO> GetByIdentityId(Guid identityId)
        {
            try
            {
                var user = await _dbSet.Where(x => x.IdentityId == identityId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                var userDto = _mapper.Map<UserSingleOutDTO>(user);

                return userDto;
            } 
            catch (Exception ex) { }
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserCollaboratorSearchResultDto>> SearchByEmail(string searchText, string userId)
        {
            try
            {
                var users = await _dbSet.Where(x => x.Email.ToLower().StartsWith(searchText.ToLower()) && x.Id != userId)
                                       .AsNoTracking()
                                       .ToListAsync();

                var resultDtos = new List<UserCollaboratorSearchResultDto>();
                foreach (var user in users)
                {
                    var userDto = _mapper.Map<UserCollaboratorSearchResultDto>(user);
                    resultDtos.Add(userDto);
                }
                return resultDtos;
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                _logger.LogError(ex, "An error occurred while searching users by email.");
                return Enumerable.Empty<UserCollaboratorSearchResultDto>();
            }
        }

        public async Task<bool> AddActiveNote(string userId,string noteId)
        {
            try
            {
                var user = await _dbSet.FirstOrDefaultAsync(x => x.IdentityId.ToString() == userId);
                user.ActiveEditingNote = noteId;
                return true;
            } 
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }
    }
}


