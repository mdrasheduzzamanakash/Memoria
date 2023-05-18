using Memoria.DataService.IRepository;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
       
        IUserRepository Users { get; }

        INoteRepository Notes { get; }

        INotificationRepository Notifications { get; }
        ILabelRepository Labels { get; }
        IAttachmentRepository Attachments { get; }
        IAuthorizationRepository Authorizations { get; }
        ICommentRepository Comments { get; }

        Task<string> AddAttachmentAsync(AttachmentSingleInDTO attachment);
        Task CompleteAsync();

    }
}
