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
    internal class LabelRepository : GenericRepository<Label>, ILabelRepository
    {
        public LabelRepository(AppDbContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<bool> AddNewLabel(LabelSingleInDTO label)
        {
            var labelEntity = _mapper.Map<Label>(label);
            return await base.Add(labelEntity);
        }

        public async Task<IEnumerable<LabelSingleOutDTO>> All()
        {
            var labels = (List<Label>) await base.All();

            var labelsDto = new List<LabelSingleOutDTO>();
            
            foreach (var label in labels)
            {
                labelsDto.Add(_mapper.Map<LabelSingleOutDTO>(label));
            }

            return labelsDto;

        }

        public async Task<IEnumerable<LabelSingleOutDTO>> AllUserLabels(string id)
        {
            var labels = (List<Label>) await _dbSet.Where(x => x.LabelerId == id).ToListAsync();

            var labelsDto = new List<LabelSingleOutDTO>();

            foreach (var label in labels)
            {
                labelsDto.Add(_mapper.Map<LabelSingleOutDTO>(label));
            }
            return labelsDto;
        }

        
    }
}
