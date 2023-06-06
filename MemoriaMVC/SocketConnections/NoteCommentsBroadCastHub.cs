using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Incomming;
using MemoriaMVC.SocketConnections.Models.Incomming;
using MemoriaMVC.SocketConnections.Models.Outgoing;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace MemoriaMVC.SocketConnections
{
    public class NoteCommentsBroadCastHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NoteCommentsBroadCastHub(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task JoinNoteGroup(string noteId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, noteId);
        }

        public async Task LeaveNoteGroup(string noteId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, noteId);
        }

        public async Task<bool> SaveCommentToDataBase(NoteCommentSingleInModel noteCommentSingleInModel)
        {
            var noteCommentSingleInDto = _mapper.Map<CommentSingleInDTO>(noteCommentSingleInModel);
            await _unitOfWork.Comments.AddComment(noteCommentSingleInDto);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task BroadCastComment(string noteCommentSingleInModelString)
        {
            if(noteCommentSingleInModelString == null)
            {
                return;
            }

            var noteCommentSingleInModel = JsonConvert.DeserializeObject<NoteCommentSingleInModel>(noteCommentSingleInModelString);

            if(noteCommentSingleInModel == null)
            {
                return;
            }
            await SaveCommentToDataBase(noteCommentSingleInModel);
            var noteCommentSingleOutModel = _mapper.Map<NoteCommentSingleOutModel>(noteCommentSingleInModel);
            var noteCommentSingleOutModelString = JsonConvert.SerializeObject(noteCommentSingleOutModel);
            await Clients.Group(noteCommentSingleInModel.NoteId).SendAsync("ReceiveNoteComment", noteCommentSingleOutModelString);
        }
    }
}
