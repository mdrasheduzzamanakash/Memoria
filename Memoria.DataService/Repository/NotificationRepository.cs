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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.Repository
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<NotificationSingleOutDTO> AddNotification(NotificationSIngleInDTO notificationSIngleInDTO)
        {
            var entity = _mapper.Map<Notification>(notificationSIngleInDTO);
            await _dbSet.AddAsync(entity);
            return _mapper.Map<NotificationSingleOutDTO>(entity);
        }

        public async Task<List<NotificationSingleOutDTO>> GetNotification(string userId)
        {
            var notifications = await _dbSet.Where(x => x.ReceiverId == userId).ToListAsync();
            var notificationsDtos = new List<NotificationSingleOutDTO>();
            foreach(var notification in notifications)
            {
                notificationsDtos.Add(_mapper.Map<NotificationSingleOutDTO>(notification));
            }
            return notificationsDtos;
        }
    }
}
