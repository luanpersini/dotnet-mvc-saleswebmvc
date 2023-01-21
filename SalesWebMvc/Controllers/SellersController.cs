using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly ISellerService _sellerService;
        private readonly IDepartmentService _departmentService;
        private readonly IActionResult _idNotProvided;
        private readonly IActionResult _idNotFound;

        public SellersController(ISellerService sellerService, IDepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
            _idNotProvided = RedirectToAction(nameof(Error), new { message = "Id not provided" }); //TODO Find a better way to reuse this
            _idNotFound = RedirectToAction(nameof(Error), new { message = "Id not provided" });
        }

        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["DepartmentId"] = new SelectList(await _departmentService.FindAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (ModelState.IsValid)
            {
                await _sellerService.InsertAsync(seller);
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(await _departmentService.FindAllAsync(), "Id", "Name", seller.DepartmentId);
            return View();
            
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return _idNotProvided;

            var seller = await _sellerService.FindByIdAsync(id.Value);
            
            if (seller == null) return _idNotFound;

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return _idNotProvided;

            var seller = await _sellerService.FindByIdAsync(id.Value);
            
            return View(seller);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return _idNotProvided;

            var seller = await _sellerService.FindByIdAsync(id.Value);
            
            if (seller == null) return _idNotFound;

            ViewData["DepartmentId"] = new SelectList(await _departmentService.FindAllAsync(), "Id", "Name", seller.DepartmentId);
            return View(seller);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid)
            {
                ViewData["DepartmentId"] = new SelectList(await _departmentService.FindAllAsync(), "Id", "Name", seller.DepartmentId);
                return View(seller);
            }
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

    }
}