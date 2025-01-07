using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private ApplicationDbContext _context;

        public VillaNumberController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var villaNumbers = _context.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM vm = new VillaNumberVM(); 
            vm.VillaList = _context.Villas.ToList().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool isRoomExist = _context.VillaNumbers.Where(x => x.Villa_Number == obj.VillaNumber.Villa_Number).Any();
            if (ModelState.IsValid && !isRoomExist)
            {
                _context.VillaNumbers.Add(obj.VillaNumber);
                _context.SaveChanges();
                TempData["Success"] = "The villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (isRoomExist)
            {
                TempData["error"] = "The Villa number already exist.";
            }
            obj.VillaList = _context.Villas.ToList().Select(c => new SelectListItem
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
                VillaList = _context.Villas.ToList().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId),
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
                _context.VillaNumbers.Update(obj.VillaNumber);
                _context.SaveChanges();
                TempData["Success"] = "The villa Number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            obj.VillaList = _context.Villas.ToList().Select(c => new SelectListItem
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
                VillaList = _context.Villas.ToList().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId),
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
            VillaNumber? existVillaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == obj.VillaNumber.Villa_Number);
            if (existVillaNumber is not null)
            {
                _context.VillaNumbers.Remove(existVillaNumber);
                _context.SaveChanges();
                TempData["Success"] = "The villa Number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa Number could not be deleted.";
            return View();
        }
    }
}
