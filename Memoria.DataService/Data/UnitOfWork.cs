using AutoMapper;
using Elfie.Serialization;
using Memoria.DataService.IConfiguration;
using Memoria.DataService.IRepository;
using Memoria.DataService.Mapper;
using Memoria.DataService.Repository;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
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
            IMapper mapper = AutoMapperConfig.Configure();

            Users = new UserRepository(context, _logger, mapper);
            Notifications = new NotificationRepository(context, _logger, mapper);
            Labels = new LabelRepository(context, _logger, mapper);
            Attachments = new AttachmentRepository(context, _logger, mapper);
            Authorizations = new AuthorizationRepository(context, _logger, mapper);
            Comments = new CommentRepository(context, _logger, mapper);
            Notes = new NoteRepository(context, _logger, mapper);
            NoteLabels = new NoteLabelRepository(context, _logger, mapper);
            Trashes = new TrashRepository(context, _logger, mapper);
        }




        // some custom methods that is inconvenient to implement inner side
        // here i am implementing them for avoiding parallel execution of trackign of entities and avoiding possible errors
        // specially those methods who requires immediate data retrival after being added should be implemented here 
        public async Task<string> AddAttachmentAsync(AttachmentSingleInDTO attachmentDto)
        {
            IMapper mapper = AutoMapperConfig.Configure();
            var attachment = mapper.Map<Attachment>(attachmentDto);
            await Attachments.Add(attachment);
            await CompleteAsync(); // must save to db and then return id
            return attachment.Id; // immediate data retrival from entity
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
