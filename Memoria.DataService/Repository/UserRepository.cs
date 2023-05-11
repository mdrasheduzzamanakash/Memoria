using Memoria.DataService.Data;
using Memoria.DataService.IRepository;
using Memoria.Entities.DbSet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
