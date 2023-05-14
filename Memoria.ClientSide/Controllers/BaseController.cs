using AutoMapper;
using Memoria.DataService.IConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace Memoria.ClientSide.Controllers
{
    public class BaseController : Controller
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        public ILogger _logger;
        public BaseController(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
