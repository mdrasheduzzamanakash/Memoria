using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;
using MemoriaMVC.SocketConnections.Models.Incomming;
using MemoriaMVC.SocketConnections.Models.Outgoing;
using MemoriaMVC.SocketConnections.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace MemoriaMVC.SocketConnections
{
    public class NotificationBroadCastHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserConnectionsService _userConnectionsService;

        public NotificationBroadCastHub(IUnitOfWork unitOfWork, IMapper mapper, UserConnectionsService userConnectionsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userConnectionsService = userConnectionsService;
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.GetHttpContext().Request.Query["userId"];

            _userConnectionsService.AddConnection(userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.GetHttpContext().Request.Query["userId"];

            _userConnectionsService.RemoveConnection(userId, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task<NotificationSingleOutDTO> SaveNotificationToDataBase(NotificationSingleInModel notificationSingleInModel)
        {
            var notificationSingleInDTO = _mapper.Map<NotificationSIngleInDTO>(notificationSingleInModel);
            var notificationDto = await _unitOfWork.Notifications.AddNotification(notificationSingleInDTO);
            await _unitOfWork.CompleteAsync();
            return notificationDto;
        }

        public async Task BroadCastNotification(string notificationSingleInModelString)
        {
            if (notificationSingleInModelString == null)
            {
                return;
            }

            var notificationSingleInModel = JsonConvert.DeserializeObject<NotificationSingleInModel>(notificationSingleInModelString);

            if (notificationSingleInModel == null)
            {
                return;
            }

            var notificationSaved = await SaveNotificationToDataBase(notificationSingleInModel);
            var notificationSingleOutModel = _mapper.Map<NotificationSingleOutModel>(notificationSaved);
            var notificationSingleOutModelString = JsonConvert.SerializeObject(notificationSingleOutModel);

            // Broadcast the notification to all connections of the receiver except the current connection
            var connections = _userConnectionsService.GetUserConnections(notificationSingleInModel.ReceiverId);
            if (connections != null)
            {
                foreach (var connectionId in connections)
                {
                    if (connectionId != Context.ConnectionId)
                    {
                        await Clients.Client(connectionId).SendAsync("ReceiveNotification", notificationSingleOutModelString);
                    }
                }
            }
        }
    }
}
