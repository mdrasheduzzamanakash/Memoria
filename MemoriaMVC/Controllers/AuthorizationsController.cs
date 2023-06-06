using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Incomming;
using MemoriaMVC.SocketConnections;
using MemoriaMVC.SocketConnections.Models.Incomming;
using MemoriaMVC.SocketConnections.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace MemoriaMVC.Controllers
{
    public class AuthorizationsController : BaseController<AuthorizationsController>
    {
        private readonly UserConnectionsService _userConnectionsService;
        private readonly IHubContext<NotificationBroadCastHub> _notificationHubContext;

        public AuthorizationsController(
            UserConnectionsService userConnectionsService,
            IHubContext<NotificationBroadCastHub> notificationHubContext,
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<AuthorizationsController> logger) : base(unitOfWork, mapper, logger)
        {
            _userConnectionsService = userConnectionsService;
            _notificationHubContext = notificationHubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthorization(string noteId, string userId)
        {
            var authorizedEntities = await _unitOfWork.Authorizations.GetAuthorization(noteId, userId);

            return Json(authorizedEntities);
        }

        [HttpGet]
        public async Task<IActionResult> fetchAuthorizedUsersOfANote(string noteId)
        {
            var authorizedUsers = await _unitOfWork.Authorizations.GetAllAuthorizationsOfANote(noteId);
            return Json(authorizedUsers);
        }

        [HttpGet]
        public async Task<IActionResult> SearchCollaboratorsOfANote(string noteId)
        {
            var authorizations = await _unitOfWork.Authorizations.GetAllAuthorizationsOfANote(noteId);
            
            var autorizationsIds = new List<string>();
            foreach(var authorizator in authorizations)
            {
                autorizationsIds.Add(authorizator.AuthorizedUserId);
            }
            var authorizedUserDetails = await _unitOfWork.Users.GetUsersDetails(autorizationsIds);

            return Json(authorizedUserDetails);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetailsById(string userId)
        {
            var user = await _unitOfWork.Users.GetSingleUserDetails(userId);
            return Json(user);
        }


        [HttpPost]
        public async Task<IActionResult> AddAuthorization([FromBody] AuthorizationSingleInDTO authorizationSingleInDTO)
        {
            if (ModelState.IsValid)
            {
                var status = await _unitOfWork.Authorizations.AddAuthorization(authorizationSingleInDTO);
                await _unitOfWork.CompleteAsync();

                var notificationSingleInModel = new NotificationSingleInModel()
                {
                    SenderId = authorizationSingleInDTO.AuthorizerId,
                    ReceiverId = authorizationSingleInDTO.AuthorizedUserId,
                    Content = "A note is shared with you.",
                    IsSent = false,
                    NoticeState = "sent",
                    link = "/Notes/Share"
                };
                var notificationSingleInModelString = JsonConvert.SerializeObject(notificationSingleInModel);

                // Send notification to user connections
                var connections = _userConnectionsService.GetUserConnections(notificationSingleInModel.ReceiverId);
                if (connections != null)
                {
                    foreach (var connectionId in connections)
                    {
                        await _notificationHubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", notificationSingleInModelString);
                    }
                }

                return Json(status);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveAuthorization([FromBody] AuthorizationSingleInDTO authorizationSingleInDTO)
        {
            if(ModelState.IsValid)
            {
                var status = await _unitOfWork.Authorizations.RemoveAuthorization(authorizationSingleInDTO);
                await _unitOfWork.CompleteAsync();
                return Json(status);
            } 
            else
            {
                return BadRequest();
            }
        }

    }
}
