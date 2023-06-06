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
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<bool> AddComment(CommentSingleInDTO commentSingleInDTO)
        {
            var entity = _mapper.Map<Comment>(commentSingleInDTO);
            return await base.Add(entity);
        }

        public async Task<List<CommentSingleOutDTO>> GetComments(string noteId)
        {
            var comments = await _dbSet.Where(x => x.NoteId == noteId).ToListAsync();
            var commentDtos = new List<CommentSingleOutDTO>();
            foreach (var comment in comments)
            {
                var commentDto = _mapper.Map<CommentSingleOutDTO>(comment);
                commentDtos.Add(commentDto);
            }
            return commentDtos;
        }
    }
}
