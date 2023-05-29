using Memoria.Entities.DTOs.Incomming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.IRepository
{
    public interface IRefreshTokenRepository
    {
        Task<bool> Add(RefreshTokenSingleInDTO refreshTokenSingleDTO);
    }
}
