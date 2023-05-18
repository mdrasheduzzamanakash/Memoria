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
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            } catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
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

        public virtual async Task<bool> Upsert(T entity, string id)
        {
            try
            {
                var existingEntity = await _dbSet.FindAsync(id);
                if (existingEntity != null)
                {
                    _dbSet.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    await _dbSet.AddAsync(entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

    }
}
