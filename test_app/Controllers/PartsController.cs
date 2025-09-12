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
    public class PartsController : Controller
    {
        private readonly DBService _db;

        public PartsController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(PartViewModel model)
        {
            var parts = await _db.GetAllPartsAsync();
            
            if (!string.IsNullOrEmpty(model.SearchByName))
                parts = parts.Where(p => p.Name.Contains(model.SearchByName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(model.SearchByArticle))
                parts = parts.Where(p => p.Article.Contains(model.SearchByArticle, StringComparison.OrdinalIgnoreCase)).ToList();
            
            if (!string.IsNullOrEmpty(model.PriceFilter))
            {
                parts = model.PriceFilter switch
                {
                    "low" => parts.Where(p => p.Price <= 300).ToList(),
                    "mid1" => parts.Where(p => p.Price > 300 && p.Price <= 1000).ToList(),
                    "mid2" => parts.Where(p => p.Price > 1000 && p.Price <= 10000).ToList(),
                    "mid3" => parts.Where(p => p.Price > 10000 && p.Price <= 50000).ToList(),
                    "high" => parts.Where(p => p.Price > 50000).ToList(),
                    _ => parts
                };
            }
            
            parts = model.SortOrder switch
            {
                "name_asc" => parts.OrderBy(p => p.Name).ToList(),
                "name_desc" => parts.OrderByDescending(p => p.Name).ToList(),
                "quantity_asc" => parts.OrderBy(p => p.StockQuantity).ToList(),
                "quantity_desc" => parts.OrderByDescending(p => p.StockQuantity).ToList(),
                "price_asc" => parts.OrderBy(p => p.Price).ToList(),
                "price_desc" => parts.OrderByDescending(p => p.Price).ToList(),
                _ => parts
            };
            
            model.AvailablePriceFilters = new List<SelectListItem>
            {
                new("До 300", "low"),
                new("301 – 1000", "mid1"),
                new("1001 – 10000", "mid2"),
                new("10001 – 50000", "mid3"),
                new("Більше 50000", "high"),
            };

            model.SortOptions = new List<SelectListItem>
            {
                new("Назва Up", "name_asc"),
                new("Назва Down", "name_desc"),
                new("К-сть на складі Up", "quantity_asc"),
                new("К-сть на складі Down", "quantity_desc"),
                new("Ціна Up", "price_asc"),
                new("Ціна Down", "price_desc"),
            };

            model.Parts = parts;
            return View(model);
        }


        // GET: Parts
        //public async Task<IActionResult> Index()
        //{            
        //    return View(await _db.GetAllPartsAsync());
        //}

        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var part = await _db.GetPartByIdAsync(id.Value);
            if (part == null)
                return NotFound();

            return View(part);
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Part part)
        {
            if (ModelState.IsValid)
            {
                await _db.AddPartAsync(part);
                return RedirectToAction(nameof(Index));
            }
            return View(part);
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var part = await _db.GetPartByIdAsync(id.Value);
            if (part == null)
                return NotFound();

            return View(part);
        }

        // POST: Parts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Part part)
        {
            if (id != part.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdatePartAsync(part);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.PartExistsAsync(part.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(part);
        }

        // GET: Parts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var part = await _db.GetPartByIdAsync(id.Value);
            if (part == null)
                return NotFound();

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeletePartAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }



    //public class PartsController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public PartsController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: Parts
    //    public async Task<IActionResult> Index()
    //    {
    //          return _context.Parts != null ? 
    //                      View(await _context.Parts.ToListAsync()) :
    //                      Problem("Entity set 'AutoWorkshopContext.Parts'  is null.");
    //    }

    //    // GET: Parts/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.Parts == null)
    //        {
    //            return NotFound();
    //        }

    //        var part = await _context.Parts
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (part == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(part);
    //    }

    //    // GET: Parts/Create
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Parts/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,Name,Article,StockQuantity,Price")] Part part)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(part);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(part);
    //    }

    //    // GET: Parts/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.Parts == null)
    //        {
    //            return NotFound();
    //        }

    //        var part = await _context.Parts.FindAsync(id);
    //        if (part == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(part);
    //    }

    //    // POST: Parts/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Article,StockQuantity,Price")] Part part)
    //    {
    //        if (id != part.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(part);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!PartExists(part.Id))
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
    //        return View(part);
    //    }

    //    // GET: Parts/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.Parts == null)
    //        {
    //            return NotFound();
    //        }

    //        var part = await _context.Parts
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (part == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(part);
    //    }

    //    // POST: Parts/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.Parts == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.Parts'  is null.");
    //        }
    //        var part = await _context.Parts.FindAsync(id);
    //        if (part != null)
    //        {
    //            _context.Parts.Remove(part);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool PartExists(int id)
    //    {
    //      return (_context.Parts?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
