using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Outgoing;
using MemoriaMVC.ViewModel.UserPageViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriaMVC.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HomeController> logger) : base(unitOfWork, mapper, logger)
        {
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            ViewBag.Id = "db950582-5212-4077-9769-3299325acae6";
            return View();
        }

        // GET: Partial View for New Note Creation
        public IActionResult GetPartialView()
        {
            return PartialView("_NoteCreationModal");
        }


        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
