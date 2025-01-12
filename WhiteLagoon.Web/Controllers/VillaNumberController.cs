using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Repository;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork context)
        {
            _unitOfWork = context;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumbers.GetAll(includeProperties : "Villa").ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM vm = new VillaNumberVM(); 
            vm.VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool isRoomExist = _unitOfWork.VillaNumbers.GetAll(x => x.Villa_Number == obj.VillaNumber.Villa_Number).Any();
            if (ModelState.IsValid && !isRoomExist)
            {
                _unitOfWork.VillaNumbers.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["Success"] = "The villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (isRoomExist)
            {
                TempData["error"] = "The Villa number already exist.";
            }
            obj.VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM VillaNumberVM = new()
            {
                VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumbers.Get(x => x.Villa_Number == villaNumberId),
            };
            if(VillaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(VillaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumbers.Update(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["Success"] = "The villa Number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            obj.VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM VillaNumberVM = new()
            {
                VillaList = _unitOfWork.Villas.GetAll().ToList().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumbers.Get(x => x.Villa_Number == villaNumberId),
            };
            if (VillaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(VillaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM obj)
        {
            VillaNumber? existVillaNumber = _unitOfWork.VillaNumbers.Get(x => x.Villa_Number == obj.VillaNumber.Villa_Number);
            if (existVillaNumber is not null)
            {
                _unitOfWork.VillaNumbers.Remove(existVillaNumber);
                _unitOfWork.Save();
                TempData["Success"] = "The villa Number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa Number could not be deleted.";
            return View();
        }
    }
}
