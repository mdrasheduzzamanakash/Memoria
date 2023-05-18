using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Memoria.DataService.Data;
using Memoria.Entities.DbSet;
using Microsoft.AspNetCore.Authentication;
using Memoria.DataService.IConfiguration;
using AutoMapper;
using MemoriaMVC.ViewModel.UserPageViewModel;
using Memoria.Entities.DTOs.Incomming;

namespace MemoriaMVC.Controllers
{
    public class UsersController : BaseController<UsersController>
    {
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UsersController> logger) : base(unitOfWork, mapper, logger)
        {
        }


        // GET: Users
        public async Task<IActionResult> Index()
        {
            ViewBag.UserId = "1ff4e1cd-6081-450d-abef-5c1667daf7f7"; 
            if (_unitOfWork.Users == null)
            {
                _logger.LogError("User entity does not Exists.");
                return BadRequest();
            }
            var users = await _unitOfWork.Users.All();
            List<UserDetailsViewModel> usersViewModel = new List<UserDetailsViewModel>();
            foreach (var user in users)
            {
                usersViewModel.Add(_mapper.Map<UserDetailsViewModel>(user));
            }
            return View(usersViewModel);
        }


        //GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (_unitOfWork.Users == null)
            {
                _logger.LogError("User entity does not Exists.");
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userViewModel = _mapper.Map<UserDetailsViewModel>(user);
            return View(userViewModel);
        }

        public async Task<IActionResult> GetById(string id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            var userJson = Json(user);
            return userJson;
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreationViewModel userCreationViewModel)
        {
            if (ModelState.IsValid)
            {
                // mapping to the DTO
                var userCreationDTO = _mapper.Map<UserSingleInDTO>(userCreationViewModel);
                var result = await _unitOfWork.Users.Add(userCreationDTO);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userCreationViewModel);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _unitOfWork.Users == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            var userViewModel = _mapper.Map<UserEditViewModel>(user);
            return View(userViewModel);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditViewModel userEditViewModel)
        {
            if (id != userEditViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userDTO = _mapper.Map<UserSingleInDTO>(userEditViewModel);
                    await _unitOfWork.Users.Upsert(userDTO, id);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userEditViewModel);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _unitOfWork.Users == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDeletetionViewMode>(user);
            return View(userDto);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_unitOfWork.Users == null)
            {
                return Problem("Entity set 'AppDbContext.Users'  is null.");
            }
            var user = await _unitOfWork.Users.GetById(id);
            if (user != null)
            {
                await _unitOfWork.Users.Delete(id);
            }

            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
