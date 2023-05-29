using AutoMapper;
using Memoria.DataService.Data;
using Memoria.DataService.IRepository;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
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
    }
}
