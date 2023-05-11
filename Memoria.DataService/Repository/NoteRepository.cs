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
    internal class NoteRepository : GenericRepository<Note>, INoteRepository
    {
        public NoteRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
