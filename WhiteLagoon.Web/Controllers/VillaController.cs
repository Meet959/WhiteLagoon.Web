using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villas.GetAll().ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if(villa.Name == villa.Description)
            {
                ModelState.AddModelError("description", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                if (villa.Image != null)
                {
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                    string imageFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                    using var fileStream = new FileStream(Path.Combine(imageFolderPath, FileName), FileMode.Create);
                    villa.Image.CopyTo(fileStream);
                    villa.ImageUrl = @"\images\VillaImages\"+FileName;
                }
                else
                {
                    villa.ImageUrl = "https://placehold.co/600x400";
                }

                villa.Created_Date = DateTime.Now;
                _unitOfWork.Villas.Add(villa);
                _unitOfWork.Save();
                TempData["Success"] = "The villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? villa = _unitOfWork.Villas.Get(x => x.Id == villaId);
            if(villa is null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (villa.Name == villa.Description)
            {
                ModelState.AddModelError("description", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid && villa.Id > 0)
            {
                if (villa.Image != null)
                {
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                    string imageFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                    if (!string.IsNullOrEmpty(villa.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imageFolderPath, FileName), FileMode.Create);
                    villa.Image.CopyTo(fileStream);
                    villa.ImageUrl = @"\images\VillaImages\" + FileName;
                }

                var existVilla = _unitOfWork.Villas.Get(x => x.Id == villa.Id);
                if (existVilla is not null)
                {
                    existVilla.Name = villa.Name;
                    existVilla.Description = villa.Description;
                    existVilla.Price = villa.Price;
                    existVilla.Occupancy = villa.Occupancy;
                    existVilla.ImageUrl = villa.ImageUrl;
                    existVilla.Sqft = villa.Sqft;
                    existVilla.Updated_Date = DateTime.Now;
                    _unitOfWork.Villas.Update(existVilla);
                    _unitOfWork.Save();
                    TempData["Success"] = "The villa has been updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(villa);
        }

        public IActionResult Delete(int villaId)
        {
            Villa? villa = _unitOfWork.Villas.Get(x => x.Id == villaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            } 
            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? existVilla = _unitOfWork.Villas.Get(x => x.Id == villa.Id);
            if (existVilla is not null)
            {
                _unitOfWork.Villas.Remove(existVilla);
                _unitOfWork.Save();
                TempData["Success"] = "The villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
