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
            if(_unitOfWork.Users == null)
            {
                _logger.LogError("User entity does not Exists.");
                return BadRequest();
            }
            var users = await _unitOfWork.Users.All();
            return View(users);
        }
        

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {

            if(id == null)
            {
                return NotFound();
            }

            if(_unitOfWork.Users == null)
            {
                _logger.LogError("User entity does not Exists.");
                return NotFound();
            }

            var user = await _unitOfWork.Users.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); 
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
            await Console.Out.WriteLineAsync("Hi i am here");
            if (ModelState.IsValid)
            {
                // mapping to the DTO
                var userCreationDTO = _mapper.Map<UserCreationDTO>(userCreationViewModel);
                
                //_unitOfWork.Users.Add(userCreationDTO);
                //await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userCreationViewModel);
        }

        /*
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _unitOfWork.Users == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Email,Password,Image,Id,Status,AddedDateAndTime,UpdatedDateAndTime,UpdatedBy,AddedBy")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Update(user);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _unitOfWork.Users == null)
            {
                return NotFound();
            }

            var user = await _unitOfWork.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
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
            var user = await _unitOfWork.Users.FindAsync(id);
            if (user != null)
            {
                _unitOfWork.Users.Remove(user);
            }
            
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
          return (_unitOfWork.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        } */
    }
}
