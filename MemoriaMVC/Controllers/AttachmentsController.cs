using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Memoria.DataService.Data;
using Memoria.Entities.DbSet;
using Memoria.DataService.IConfiguration;
using AutoMapper;
using Newtonsoft.Json;
using Memoria.Entities.DTOs.Outgoing;
using Memoria.Entities.DTOs.Incomming;
using MemoriaMVC.ViewModel.Attachment;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Authentication.Configuration;

namespace MemoriaMVC.Controllers
{
    public class AttachmentsController : BaseController<AttachmentsController>
    {
        public AttachmentsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AttachmentsController> logger, UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionMonitor) : base(unitOfWork, mapper, logger, userManager, optionMonitor)
        {
        }

        [HttpGet]
        public async Task<IActionResult> AllAttachmentPreview([FromQuery] string noteIds)
        {
            var noteIdsArray = JsonConvert.DeserializeObject<string[]>(noteIds);
            var attchmentPreviews = await _unitOfWork.Attachments.GetFirstOneByIds(noteIdsArray);
            return Json(attchmentPreviews);
        }


        [HttpGet]
        public async Task<IActionResult> AttachmentAllForANote([FromQuery] string noteId) 
        {
              var allAttachments = await _unitOfWork.Attachments.GetAllAttachmentForANote(noteId);
            return Json(allAttachments);
        }


        [HttpPost]
        public async Task<IActionResult> AddAttachment()
        {
            var file = Request.Form.Files[0];
            string attachMentMetaDataString = Request.Form["attachmentMetaData"];

            if(file == null)
            {
                throw new ArgumentNullException(nameof(file));
            } 
            if(attachMentMetaDataString == null)
            {
                throw new ArgumentNullException(attachMentMetaDataString, nameof(attachMentMetaDataString));
            }

            var attachmentMetaData = JsonConvert.DeserializeObject<AttachmentViewModel>(attachMentMetaDataString);

            var attchmentDto = _mapper.Map<AttachmentSingleInDTO>(attachmentMetaData);

            // auto clearing memory via disposable object
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                attachmentMetaData.file = memoryStream.ToArray();
            }

            var attachmentMetaDataDto = _mapper.Map<AttachmentSingleInDTO>(attachmentMetaData);
            // now add to the database
            string attachmentId = await _unitOfWork.AddAttachmentAsync(attachmentMetaDataDto);

            return Json(attachmentId);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.Attachments.Delete(id);
            return Json(null);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAllAttachmentOfANote([FromQuery]string noteId, [FromQuery] string userId)
        {
            return Json(await _unitOfWork.Attachments.DeleteAllAttachmentOfANote(noteId, userId));
        }
    }
}
