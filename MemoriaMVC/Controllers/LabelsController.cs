using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Incomming;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaMVC.Controllers
{
    public class LabelsController : BaseController<LabelsController>
    {
        public LabelsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LabelsController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddNewLabel([FromBody] LabelSingleInDTO labelSingleInDTO)
        {
            var status = await _unitOfWork.Labels.AddNewLabel(labelSingleInDTO);
            await _unitOfWork.CompleteAsync();
            return Json(status);
        }
    }
}
