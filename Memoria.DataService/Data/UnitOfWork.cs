using Memoria.DataService.IConfiguration;
using Memoria.DataService.IRepository;
using Memoria.DataService.Repository;
using Memoria.Entities.DbSet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable 
    {
        private readonly AppDbContext _context;
        
        private readonly ILogger _logger;

        public IUserRepository Users { get; private set; }

        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("db_logs");
            Users = new UserRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        // memory optimization
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
