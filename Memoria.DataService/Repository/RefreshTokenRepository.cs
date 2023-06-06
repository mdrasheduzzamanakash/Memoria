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
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<bool> Add(RefreshTokenSingleInDTO refreshTokenSingleDTO)
        {
            try
            {
                var refreshToken = _mapper.Map<RefreshToken>(refreshTokenSingleDTO);
                await base.Add(refreshToken);
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<RefreshTokenSingleOutDTO> GetByRefreshToken(string refreshToken)
        {
            try
            {
                var token = await _dbSet.Where(x => x.Token.ToLower() == refreshToken.ToLower())
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                return _mapper.Map<RefreshTokenSingleOutDTO>(token);
            } 
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> MarkRefreshTokenAsUsed(RefreshTokenSingleInDTO refreshToken)
        {
            try
            {
                var token = await _dbSet.Where(x => x.Token.ToLower() == refreshToken.Token.ToLower())
                    .FirstOrDefaultAsync();
                if(token == null)
                {
                    return false;
                }

                token.IsUsed = refreshToken.IsUsed;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
