using AutoMapper;
using Memoria.DataService.Data;
using Memoria.DataService.IRepository;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;
using Microsoft.EntityFrameworkCore;
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

        public override async Task<bool> Add(Attachment attachment)
        {
            return await base.Add(attachment);
        }

        public override async Task<bool> Delete(string id)
        {
            return await base.Delete(id);
        }

        public async Task<bool> DeleteAllAttachmentOfANote(string noteId, string userId)
        {
            try
            {
                var attachments = await _dbSet.Where(x => x.NoteId == noteId && x.OwnerId == userId).ToListAsync();

                await Console.Out.WriteLineAsync('h');
                foreach (var attachment in attachments)
                {
                    await base.Delete(attachment.Id); 
                }

                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting attachments: {ex.Message}");
                return false;
            }
        }


        public async Task<List<AttachmentSingleOutDTO>> GetAllAttachmentForANote(string noteId)
        {
            var attachments = await _dbSet.Where(x => x.NoteId == noteId).AsNoTracking().ToListAsync();
            var attachmentDtos = attachments.Select(_mapper.Map<AttachmentSingleOutDTO>).ToList();
            return attachmentDtos;
        }

        public async Task<List<AttachmentSingleOutDTO>> GetFirstOneByIds(string[] noteIds)
        {
            var attachments = await _dbSet
                .Where(x => noteIds.Contains(x.NoteId))
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToListAsync();

            var attachmentDtos = attachments.Select(_mapper.Map<AttachmentSingleOutDTO>).ToList();

            return attachmentDtos;
        }

    }
}
