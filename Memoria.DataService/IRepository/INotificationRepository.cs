using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.IRepository
{
    public interface INotificationRepository 
    {
        Task<NotificationSingleOutDTO> AddNotification(NotificationSIngleInDTO notificationSIngleInDTO);
        Task<List<NotificationSingleOutDTO>> GetNotification(string userId);
    }
}
