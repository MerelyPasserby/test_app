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
    public class PartCompatibilitiesController : Controller
    {
        private readonly DBService _db;

        public PartCompatibilitiesController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(PartCompatibilityViewModel model)
        {
            var list = await _db.GetAllPartCompatibilitiesAsync();

            
            if (!string.IsNullOrWhiteSpace(model.SearchPartName))
            {
                list = list.Where(pc => pc.Part.DisplayName.Contains(model.SearchPartName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            
            if (!string.IsNullOrWhiteSpace(model.SearchCarName))
            {
                list = list.Where(pc => pc.Car.DisplayName.Contains(model.SearchCarName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            model.PartCompatibilities = list;

            return View(model);
        }

        // GET: PartCompatibilities
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.GetAllPartCompatibilitiesAsync());
        //}

        // GET: PartCompatibilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var pc = await _db.GetPartCompatibilityByIdAsync(id.Value);
            if (pc == null)
                return NotFound();

            return View(pc);
        }

        // GET: PartCompatibilities/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CarId"] = new SelectList(await _db.GetAllCarsAsync(), "Id", "DisplayName");
            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName");
            return View();
        }

        // POST: PartCompatibilities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartCompatibility partCompatibility)
        {
            if (ModelState.IsValid)
            {
                await _db.AddPartCompatibilityAsync(partCompatibility);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarId"] = new SelectList(await _db.GetAllCarsAsync(), "Id", "DisplayName", partCompatibility.CarId);
            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName", partCompatibility.PartId);
            return View(partCompatibility);
        }

        // GET: PartCompatibilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var pc = await _db.GetPartCompatibilityByIdAsync(id.Value);
            if (pc == null)
                return NotFound();

            ViewData["CarId"] = new SelectList(await _db.GetAllCarsAsync(), "Id", "DisplayName", pc.CarId);
            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName", pc.PartId);
            return View(pc);
        }

        // POST: PartCompatibilities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PartCompatibility partCompatibility)
        {
            if (id != partCompatibility.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdatePartCompatibilityAsync(partCompatibility);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.PartCompatibilityExistsAsync(partCompatibility.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarId"] = new SelectList(await _db.GetAllCarsAsync(), "Id", "DisplayName", partCompatibility.CarId);
            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName", partCompatibility.PartId);
            return View(partCompatibility);
        }

        // GET: PartCompatibilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var pc = await _db.GetPartCompatibilityByIdAsync(id.Value);
            if (pc == null)
                return NotFound();

            return View(pc);
        }

        // POST: PartCompatibilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeletePartCompatibilityAsync(id);
            return RedirectToAction(nameof(Index));
        }
       
    }



    //public class PartCompatibilitiesController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public PartCompatibilitiesController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: PartCompatibilities
    //    public async Task<IActionResult> Index()
    //    {
    //        var autoWorkshopContext = _context.PartCompatibilities.Include(p => p.Car).Include(p => p.Part);
    //        return View(await autoWorkshopContext.ToListAsync());
    //    }

    //    // GET: PartCompatibilities/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.PartCompatibilities == null)
    //        {
    //            return NotFound();
    //        }

    //        var partCompatibility = await _context.PartCompatibilities
    //            .Include(p => p.Car)
    //            .Include(p => p.Part)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (partCompatibility == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(partCompatibility);
    //    }

    //    // GET: PartCompatibilities/Create
    //    public IActionResult Create()
    //    {
    //        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id");
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id");
    //        return View();
    //    }

    //    // POST: PartCompatibilities/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,PartId,CarId")] PartCompatibility partCompatibility)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(partCompatibility);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", partCompatibility.CarId);
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", partCompatibility.PartId);
    //        return View(partCompatibility);
    //    }

    //    // GET: PartCompatibilities/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.PartCompatibilities == null)
    //        {
    //            return NotFound();
    //        }

    //        var partCompatibility = await _context.PartCompatibilities.FindAsync(id);
    //        if (partCompatibility == null)
    //        {
    //            return NotFound();
    //        }
    //        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", partCompatibility.CarId);
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", partCompatibility.PartId);
    //        return View(partCompatibility);
    //    }

    //    // POST: PartCompatibilities/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,PartId,CarId")] PartCompatibility partCompatibility)
    //    {
    //        if (id != partCompatibility.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(partCompatibility);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!PartCompatibilityExists(partCompatibility.Id))
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
    //        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", partCompatibility.CarId);
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", partCompatibility.PartId);
    //        return View(partCompatibility);
    //    }

    //    // GET: PartCompatibilities/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.PartCompatibilities == null)
    //        {
    //            return NotFound();
    //        }

    //        var partCompatibility = await _context.PartCompatibilities
    //            .Include(p => p.Car)
    //            .Include(p => p.Part)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (partCompatibility == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(partCompatibility);
    //    }

    //    // POST: PartCompatibilities/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.PartCompatibilities == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.PartCompatibilities'  is null.");
    //        }
    //        var partCompatibility = await _context.PartCompatibilities.FindAsync(id);
    //        if (partCompatibility != null)
    //        {
    //            _context.PartCompatibilities.Remove(partCompatibility);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool PartCompatibilityExists(int id)
    //    {
    //      return (_context.PartCompatibilities?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
