using Memoria.DataService.Data;
using Memoria.DataService.IRepository;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
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
        public UserRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {

        }

        // GET all the users
        public override async Task<IEnumerable<User>> All()
        {
            try
            {
                return await _dbSet.Where(x => x.Status == 1)
                    .AsNoTracking()
                    .ToListAsync();
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All", typeof(UserRepository));
                return new List<User>();
            }
        }

        // Add a user

    }
}
