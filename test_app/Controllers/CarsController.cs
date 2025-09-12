using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test_app.Models;
using test_app.Services;
using test_app.ViewModels;

namespace test_app.Controllers 
{
    public class CarsController : Controller
    {
        private readonly DBService _db;
        public CarsController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(CarViewModel model)
        {
            var cars = await _db.GetAllCarsAsync();
            
            if (!string.IsNullOrWhiteSpace(model.SearchString))
            {
                cars = cars.Where(c =>
                    c.Brand.Contains(model.SearchString, StringComparison.OrdinalIgnoreCase) ||
                    c.Model.Contains(model.SearchString, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(model.YearRange))
            {
                cars = model.YearRange switch
                {
                    "before2000" => cars.Where(c => c.YearFrom < 2000).ToList(),
                    "2000to2010" => cars.Where(c => c.YearFrom >= 2000 && c.YearFrom <= 2010).ToList(),
                    "2011to2015" => cars.Where(c => c.YearFrom >= 2011 && c.YearFrom <= 2015).ToList(),
                    "2016to2020" => cars.Where(c => c.YearFrom >= 2016 && c.YearFrom <= 2020).ToList(),
                    "2021plus" => cars.Where(c => c.YearFrom >= 2021).ToList(),
                    _ => cars
                };
            }
            
            cars = model.SortOrder switch
            {
                "year_asc" => cars.OrderBy(c => c.YearFrom).ToList(),
                "year_desc" => cars.OrderByDescending(c => c.YearFrom).ToList(),
                "brand_asc" => cars.OrderBy(c => c.Brand).ToList(),
                "brand_desc" => cars.OrderByDescending(c => c.Brand).ToList(),
                _ => cars
            };
            
            model.AvailableYearRanges = new List<SelectListItem>
            {
                new("Before 2000", "before2000"),
                new("2000–2010", "2000to2010"),
                new("2011–2015", "2011to2015"),
                new("2016–2020", "2016to2020"),
                new("2021+", "2021plus")
            };

            model.AvailableSortOrders = new List<SelectListItem>
            {
                new("Year Up", "year_asc"),
                new("Year Down", "year_desc"),
                new("Brand A–Z", "brand_asc"),
                new("Brand Z–A", "brand_desc")
            };

            model.Cars = cars;
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) 
                return NotFound();

            var car = await _db.GetCarByIdAsync(id.Value);
            if (car == null) 
                return NotFound();

            return View(car);
        }
       
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                await _db.AddCarAsync(car);
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.GetCarByIdAsync(id.Value);
            if (car == null)
                return NotFound();

            return View(car);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateCarAsync(car);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.CarExistsAsync(car.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.GetCarByIdAsync(id.Value);
            if (car == null)
                return NotFound();

            return View(car);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeleteCarAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}



// GET: Cars
//public async Task<IActionResult> Index()
//{
//      return _context.Cars != null ? 
//                  View(await _context.Cars.ToListAsync()) :
//                  Problem("Entity set 'AutoWorkshopContext.Cars'  is null.");
//}
// GET: Cars/Details/5
//public async Task<IActionResult> Details(int? id)
//{
//    if (id == null || _context.Cars == null)
//    {
//        return NotFound();
//    }

//    var car = await _context.Cars
//        .FirstOrDefaultAsync(m => m.Id == id);
//    if (car == null)
//    {
//        return NotFound();
//    }

//    return View(car);
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Create([Bind("Id,Brand,Model,YearFrom,YearTo,Engine,Transmission")] Car car)
//{
//    if (ModelState.IsValid)
//    {
//        _context.Add(car);
//        await _context.SaveChangesAsync();
//        return RedirectToAction(nameof(Index));
//    }
//    return View(car);
//}
// GET: Cars/Edit/5
//public async Task<IActionResult> Edit(int? id)
//{
//    if (id == null || _context.Cars == null)
//    {
//        return NotFound();
//    }

//    var car = await _context.Cars.FindAsync(id);
//    if (car == null)
//    {
//        return NotFound();
//    }
//    return View(car);
//}

//// POST: Cars/Edit/5
//// To protect from overposting attacks, enable the specific properties you want to bind to.
//// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,YearFrom,YearTo,Engine,Transmission")] Car car)
//{
//    if (id != car.Id)
//    {
//        return NotFound();
//    }

//    if (ModelState.IsValid)
//    {
//        try
//        {
//            _context.Update(car);
//            await _context.SaveChangesAsync();
//        }
//        catch (DbUpdateConcurrencyException)
//        {
//            if (!CarExists(car.Id))
//            {
//                return NotFound();
//            }
//            else
//            {
//                throw;
//            }
//        }
//        return RedirectToAction(nameof(Index));
//    }
//    return View(car);
//}
// GET: Cars/Delete/5
//public async Task<IActionResult> Delete(int? id)
//{
//    if (id == null || _context.Cars == null)
//    {
//        return NotFound();
//    }

//    var car = await _context.Cars
//        .FirstOrDefaultAsync(m => m.Id == id);
//    if (car == null)
//    {
//        return NotFound();
//    }

//    return View(car);
//}

//// POST: Cars/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> DeleteConfirmed(int id)
//{
//    if (_context.Cars == null)
//    {
//        return Problem("Entity set 'AutoWorkshopContext.Cars'  is null.");
//    }
//    var car = await _context.Cars.FindAsync(id);
//    if (car != null)
//    {
//        _context.Cars.Remove(car);
//    }

//    await _context.SaveChangesAsync();
//    return RedirectToAction(nameof(Index));
//}

//private bool CarExists(int id)
//{
//    return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
//}