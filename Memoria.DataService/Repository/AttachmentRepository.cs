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
    internal class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(AppDbContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<bool> Add(Attachment attachment)
        {
            return await base.Add(attachment);
        }

        public async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
