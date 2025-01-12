using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private UnitOfWork _unitOfWork;

        public VillaController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                var existVilla = _unitOfWork.Villas.Get(x => x.Id == villa.Id);
                if (existVilla is not null)
                {
                    existVilla.Name = villa.Name;
                    existVilla.Description = villa.Description;
                    existVilla.Price = villa.Price;
                    existVilla.Occupancy = villa.Occupancy;
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
            Villa existVilla = _unitOfWork.Villas.Get(x => x.Id == villa.Id);
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
