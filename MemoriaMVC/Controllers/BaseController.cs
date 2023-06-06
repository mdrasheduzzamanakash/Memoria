
using Authentication.Configuration;
using AutoMapper;
using Memoria.DataService.IConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MemoriaMVC.Controllers
{
    public class BaseController<T> : Controller
    {
        public IUnitOfWork _unitOfWork;
        public IMapper _mapper;
        protected readonly ILogger<T> _logger;
        public BaseController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<T> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
