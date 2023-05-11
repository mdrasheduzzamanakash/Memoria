using Memoria.DataService.Data;
using Memoria.DataService.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;

        protected readonly ILogger _logger;

        internal DbSet<T> _dbSet;


        public GenericRepository(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = context.Set<T>();
        }


        public Task<bool> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
