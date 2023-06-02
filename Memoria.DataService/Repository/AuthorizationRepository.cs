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
    internal class AuthorizationRepository : GenericRepository<Authorization>, IAuthorizationRepository
    {
        public AuthorizationRepository(AppDbContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<bool> AddAuthorization(AuthorizationSingleInDTO authorizationSingleInDTO)
        {
            var entity = _mapper.Map<Authorization>(authorizationSingleInDTO);
            return await base.Add(entity);
        }

        public async Task<List<AuthorizationSingleOutDTO>> GetAllAuthorizationsOfANote(string noteId)
        {
            var allAuthorizations = await _dbSet.Where(x => x.NoteId == noteId).ToListAsync();
            var allAuthorizationsDtos = new List<AuthorizationSingleOutDTO>();
            foreach (var authorization in allAuthorizations)
            {
                var authorizationDto = _mapper.Map<AuthorizationSingleOutDTO>(authorization);
                allAuthorizationsDtos.Add(authorizationDto);
            }
            return allAuthorizationsDtos;
        }

        public async Task<List<AuthorizationSingleOutDTO>> GetAllAuthorizationsOfUser(string authorId)
        {
            var authorizations = await _dbSet
                        .Where(x => x.AuthorizedUserId == authorId)
                        .ToListAsync();

            var authorizationsDto = new List<AuthorizationSingleOutDTO>();
            foreach (var authorization in authorizations)
            {
                var authorizationDto = _mapper.Map<AuthorizationSingleOutDTO>(authorization);
                authorizationsDto.Add(authorizationDto);
            }
            return authorizationsDto;
        }

        public async Task<AuthorizationSingleOutDTO> GetAuthorization(string noteId, string userId)
        {
            var authorization = await _dbSet.FirstOrDefaultAsync(x => x.NoteId == noteId && x.AuthorizedUserId == userId);

            var authorizationDto = _mapper.Map<AuthorizationSingleOutDTO>(authorization);

            return authorizationDto;
        }

        public async Task<bool> RemoveAuthorization(AuthorizationSingleInDTO authorizationSingleInDTO)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.AuthorizerId == authorizationSingleInDTO.AuthorizerId &&
                                                      x.AuthorizedUserId == authorizationSingleInDTO.AuthorizedUserId &&
                                                      x.NoteId == authorizationSingleInDTO.NoteId);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return true;
            }

            return false;
        }

    }
}
