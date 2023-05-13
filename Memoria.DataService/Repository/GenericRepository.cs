using AutoMapper;
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

        protected readonly IMapper _mapper;

        internal DbSet<T> _dbSet;


        public GenericRepository(AppDbContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _dbSet = context.Set<T>();
        }


        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(string id)
        {
            var entityToDelete = await _dbSet.FindAsync(id);
            if(entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
                return true;
            }
            return false;
        }
       

        public virtual async Task<IEnumerable<T>> All()
        {
            return await _dbSet.ToListAsync(); 
        }

        public virtual async Task<T?> GetById(string id)
        {
            var result = await _dbSet.FindAsync(id);
            if(result != null)
            {
                return result;
            }
            _logger.LogError("NotFound in _dbSet");
            return null;
        }

        public virtual async Task<bool> Upsert(T entity)
        {
            try
            {
                // get the primary key property name of type T
                var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();
                
                // get the Primary key
                var keyValue = entity.GetType().GetProperty(keyName).GetValue(entity, null);

                // Find the existing entry 
                var existingEntry = await _dbSet.FindAsync(keyValue);

                if(existingEntry != null)
                {
                    _dbSet.Entry(existingEntry).CurrentValues.SetValues(entity);
                }
                else
                {
                    await _dbSet.AddAsync(entity);
                }
                return true;
            }
            catch
            {
                _logger.LogError("Error in Upsert method _dbSet");
                return false;
            }
        }
    }
}
