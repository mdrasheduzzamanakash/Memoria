using AutoMapper;
using Memoria.DataService.IConfiguration;
using MemoriaMVC.SocketConnections.Models.Incomming;
using MemoriaMVC.SocketConnections.Models.Outgoing;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace MemoriaMVC.SocketConnections
{
    public class NoteChangeBroadCastHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NoteChangeBroadCastHub(IUnitOfWork unitOfWork, IMapper mapper)
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

        public async Task<bool> ChangeTitleOfTheNote(NoteChangeSingleInModel noteChangeSingleInModel)
        {
            await _unitOfWork.Notes.ModifyTitleOrDescription(
                noteChangeSingleInModel.NoteId,
                noteChangeSingleInModel.TitleChanges,
                "",
                true);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<bool> ChangeDescriptionOfTheNote(NoteChangeSingleInModel noteChangeSingleInModel)
        {
            await _unitOfWork.Notes.ModifyTitleOrDescription(
                noteChangeSingleInModel.NoteId,
                "",
                noteChangeSingleInModel.DescriptionChanges,
                false);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task BroadCastNoteChange(string noteChangeSingleInModelString)
        {
            var noteChangeSingleInModel = JsonConvert.DeserializeObject<NoteChangeSingleInModel>(noteChangeSingleInModelString);
            // write to the database
            if(noteChangeSingleInModel.IsTitleChanged)
            {
                await ChangeTitleOfTheNote(noteChangeSingleInModel);
            } else
            {
                
            }

            var noteChangeSingleOutModel = _mapper.Map<NoteChangeSingleOutModel>(noteChangeSingleInModel);
            var noteChnageSingleOutModelString = JsonConvert.SerializeObject(noteChangeSingleOutModel);
            await Clients.GroupExcept(noteChangeSingleInModel.NoteId, Context.ConnectionId).SendAsync("ReceiveNoteChanges", noteChnageSingleOutModelString);
        }
    }
}
