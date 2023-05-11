using Memoria.DataService.IConfiguration;
using Memoria.DataService.IRepository;
using Memoria.DataService.Repository;
using Memoria.Entities.DbSet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.DataService.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable 
    {
        private readonly AppDbContext _context;
        
        private readonly ILogger _logger;

        public IUserRepository Users { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public ILabelRepository Labels { get; private set; }
        public IAttachmentRepository Attachments { get; private set; }
        public IAuthorizationRepository Authorizations { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public INoteRepository Notes { get; private set; }
        public INoteLabelRepository NoteLabels { get; private set; }
        public ITrashRepository Trashes { get; private set; }
        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("db_logs");
            Users = new UserRepository(context, _logger);
            Notifications = new NotificationRepository(context, _logger);
            Labels = new LabelRepository(context, _logger);
            Attachments = new AttachmentRepository(context, _logger);
            Authorizations = new AuthorizationRepository(context, _logger);
            Comments = new CommentRepository(context, _logger);
            Notes = new NoteRepository(context, _logger);
            NoteLabels = new NoteLabelRepository(context, _logger);
            Trashes = new TrashRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        // memory optimization
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
