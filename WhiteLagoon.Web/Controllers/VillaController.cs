using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var villas = _context.Villas.ToList();
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
                _context.Villas.Add(villa);
                _context.SaveChanges();
                TempData["Success"] = "The villa has been created successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
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
                var existVilla = _context.Villas.FirstOrDefault(x => x.Id == villa.Id);
                if (existVilla is not null)
                {
                    existVilla.Name = villa.Name;
                    existVilla.Description = villa.Description;
                    existVilla.Price = villa.Price;
                    existVilla.Occupancy = villa.Occupancy;
                    existVilla.Sqft = villa.Sqft;
                    existVilla.Updated_Date = DateTime.Now;
                    _context.Villas.Update(existVilla);
                    _context.SaveChanges();
                    TempData["Success"] = "The villa has been updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(villa);
        }

        public IActionResult Delete(int villaId)
        {
            Villa? villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            } 
            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa existVilla = _context.Villas.FirstOrDefault(x => x.Id == villa.Id);
            if (existVilla is not null)
            {
                _context.Villas.Remove(existVilla);
                _context.SaveChanges();
                TempData["Success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
