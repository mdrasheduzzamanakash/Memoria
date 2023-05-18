using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DbSet;
using Memoria.Entities.DTOs.Incomming;
using Microsoft.AspNetCore.Mvc;

namespace Memoria.WebApi.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
       
        // GET: get all the users
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAll();
            return Ok(users);
        }

        // POST: Add a user
        [HttpPost]
        public async Task<IActionResult> AddUser(UserDto user)
        {
            var _user = _mapper.Map<User>(user);
            await
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDto user)
        {
            var _user = _mapper.Map<User>(user);
            _user.Status = 1;
            await _unitOfWork.Users.Add(_user);
            await _unitOfWork.CompleteAsync();
            var userDto = _mapper.Map<UserDto>(_user);
            return CreatedAtRoute("GetUser", new { id = _user.Id }, userDto);
        }



        // GET; get a specific user by id
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            return Ok(user);
        }

    }
}
