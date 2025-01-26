using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Repository;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork context)
        {
            _unitOfWork = context;
        }
        public IActionResult Index()
        {
            var Amenities = _unitOfWork.Amenities.GetAll(includeProperties: "Villa").ToList();
            return View(Amenities);
        }

        public IActionResult Create()
        {
            AmenityVM vm = new AmenityVM();
            vm.VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            bool isRoomExist = _unitOfWork.Amenities.GetAll(x => x.Id == obj.Amenity.Id).Any();
            if (ModelState.IsValid && !isRoomExist)
            {
                _unitOfWork.Amenities.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["Success"] = "The amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (isRoomExist)
            {
                TempData["error"] = "The amenity already exist.";
            }
            obj.VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int AmenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenities.Get(x => x.Id == AmenityId),
            };
            if (AmenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenities.Update(obj.Amenity);
                _unitOfWork.Save();
                TempData["Success"] = "The amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            obj.VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Delete(int AmenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenities.Get(x => x.Id == AmenityId),
            };
            if (AmenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM obj)
        {
            Amenity? existAmenity = _unitOfWork.Amenities.Get(x => x.Id == obj.Amenity.Id);
            if (existAmenity is not null)
            {
                _unitOfWork.Amenities.Remove(existAmenity);
                _unitOfWork.Save();
                TempData["Success"] = "The amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity could not be deleted.";
            return View();
        }
    }
}
