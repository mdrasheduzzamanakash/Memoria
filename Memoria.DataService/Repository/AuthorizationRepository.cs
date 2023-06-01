using AutoMapper;
using Memoria.DataService.Data;
using Memoria.DataService.IRepository;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
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
