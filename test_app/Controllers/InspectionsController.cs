using System;
using System.Collections.Generic;
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
    public class InspectionsController : Controller
    {
        private readonly DBService _db;

        public InspectionsController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(InspectionViewModel model)
        {
            var inspections = await _db.GetAllInspectionsAsync(); 
            
            if (!string.IsNullOrEmpty(model.SearchByRegNum))
                inspections = inspections.Where(i => i.ClientCar.RegistrationNum.Contains(model.SearchByRegNum, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(model.SearchByClientName))
                inspections = inspections.Where(i => i.ClientCar.Client.FullName.Contains(model.SearchByClientName, StringComparison.OrdinalIgnoreCase)).ToList();
            
            if (!string.IsNullOrEmpty(model.DateFilter))
            {
                var now = DateTime.Now;
                inspections = model.DateFilter switch
                {
                    "week" => inspections.Where(i => (now - i.InspectionDate).TotalDays <= 7).ToList(),
                    "month" => inspections.Where(i => (now - i.InspectionDate).TotalDays > 7 && (now - i.InspectionDate).TotalDays <= 30).ToList(),
                    "3months" => inspections.Where(i => (now - i.InspectionDate).TotalDays > 30 && (now - i.InspectionDate).TotalDays <= 90).ToList(),
                    "6months" => inspections.Where(i => (now - i.InspectionDate).TotalDays > 90 && (now - i.InspectionDate).TotalDays <= 180).ToList(),
                    "older" => inspections.Where(i => (now - i.InspectionDate).TotalDays > 180).ToList(),
                    _ => inspections
                };
            }
            
            inspections = model.SortOrder switch
            {
                "date_asc" => inspections.OrderBy(i => i.InspectionDate).ToList(),
                "date_desc" => inspections.OrderByDescending(i => i.InspectionDate).ToList(),
                _ => inspections
            };
            
            model.AvailableDateFilters = new List<SelectListItem>
            {
                new("За останній тиждень", "week"),
                new("1 тиждень - 1 місяць", "month"),
                new("1 - 3 місяці", "3months"),
                new("3 - 6 місяців", "6months"),
                new("Більше 6 місяців", "older")
            };

            model.SortOptions = new List<SelectListItem>
            {
                new("Дата Up", "date_asc"),
                new("Дата Down", "date_desc")
            };

            model.Inspections = inspections;
            return View(model);
        }


        // GET: Inspections
        //public async Task<IActionResult> Index()
        //{            
        //    return View(await _db.GetAllInspectionsAsync());
        //}

        // GET: Inspections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var inspection = await _db.GetInspectionByIdAsync(id.Value);
            if (inspection == null)
                return NotFound();

            return View(inspection);
        }

        // GET: Inspections/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ClientCarId"] = new SelectList(await _db.GetAllClientCarsAsync(), "Id", "DisplayName");
            return View();
        }

        // POST: Inspections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inspection inspection)
        {
            if (ModelState.IsValid)
            {
                await _db.AddInspectionAsync(inspection);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientCarId"] = new SelectList(await _db.GetAllClientCarsAsync(), "Id", "DisplayName", inspection.ClientCarId);
            return View(inspection);
        }

        // GET: Inspections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var inspection = await _db.GetInspectionByIdAsync(id.Value);
            if (inspection == null)
                return NotFound();

            ViewData["ClientCarId"] = new SelectList(await _db.GetAllClientCarsAsync(), "Id", "DisplayName", inspection.ClientCarId);
            return View(inspection);
        }

        // POST: Inspections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inspection inspection)
        {
            if (id != inspection.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateInspectionAsync(inspection);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.InspectionExistsAsync(inspection.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientCarId"] = new SelectList(await _db.GetAllClientCarsAsync(), "Id", "DisplayName", inspection.ClientCarId);
            return View(inspection);
        }


        // GET: Inspections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var inspection = await _db.GetInspectionByIdAsync(id.Value);
            if (inspection == null)
                return NotFound();

            return View(inspection);
        }

        // POST: Inspections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeleteInspectionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

    //public class InspectionsController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public InspectionsController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: Inspections
    //    public async Task<IActionResult> Index()
    //    {
    //        var autoWorkshopContext = _context.Inspections.Include(i => i.ClientCar);
    //        return View(await autoWorkshopContext.ToListAsync());
    //    }

    //    // GET: Inspections/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.Inspections == null)
    //        {
    //            return NotFound();
    //        }

    //        var inspection = await _context.Inspections
    //            .Include(i => i.ClientCar)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (inspection == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(inspection);
    //    }

    //    // GET: Inspections/Create
    //    public IActionResult Create()
    //    {
    //        ViewData["ClientCarId"] = new SelectList(_context.ClientCars, "Id", "Id");
    //        return View();
    //    }

    //    // POST: Inspections/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,ClientCarId,InspectionDate,Results,Cost,IsPaid")] Inspection inspection)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(inspection);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["ClientCarId"] = new SelectList(_context.ClientCars, "Id", "Id", inspection.ClientCarId);
    //        return View(inspection);
    //    }

    //    // GET: Inspections/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.Inspections == null)
    //        {
    //            return NotFound();
    //        }

    //        var inspection = await _context.Inspections.FindAsync(id);
    //        if (inspection == null)
    //        {
    //            return NotFound();
    //        }
    //        ViewData["ClientCarId"] = new SelectList(_context.ClientCars, "Id", "Id", inspection.ClientCarId);
    //        return View(inspection);
    //    }

    //    // POST: Inspections/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,ClientCarId,InspectionDate,Results,Cost,IsPaid")] Inspection inspection)
    //    {
    //        if (id != inspection.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(inspection);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!InspectionExists(inspection.Id))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["ClientCarId"] = new SelectList(_context.ClientCars, "Id", "Id", inspection.ClientCarId);
    //        return View(inspection);
    //    }

    //    // GET: Inspections/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.Inspections == null)
    //        {
    //            return NotFound();
    //        }

    //        var inspection = await _context.Inspections
    //            .Include(i => i.ClientCar)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (inspection == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(inspection);
    //    }

    //    // POST: Inspections/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.Inspections == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.Inspections'  is null.");
    //        }
    //        var inspection = await _context.Inspections.FindAsync(id);
    //        if (inspection != null)
    //        {
    //            _context.Inspections.Remove(inspection);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool InspectionExists(int id)
    //    {
    //      return (_context.Inspections?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
