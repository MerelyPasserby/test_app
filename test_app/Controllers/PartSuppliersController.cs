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
    public class PartSuppliersController : Controller
    {
        private readonly DBService _db;

        public PartSuppliersController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(PartSupplierViewModel model)
        {
            var items = await _db.GetAllPartSuppliersAsync();

            if (!string.IsNullOrWhiteSpace(model.SearchPartName))
            {
                items = items.Where(i =>
                    i.Part.DisplayName.Contains(model.SearchPartName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(model.SearchSupplierName))
            {
                items = items.Where(i =>
                    i.Supplier.Name.Contains(model.SearchSupplierName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(model.AvailabilityFilter))
            {
                items = model.AvailabilityFilter switch
                {
                    "available" => items.Where(i => i.Availability == "Наявні").ToList(),
                    "not_available" => items.Where(i => i.Availability == "Нема в наявності").ToList(),
                    "in_transit" => items.Where(i => i.Availability == "В дорозі").ToList(),
                    "not_supplied" => items.Where(i => i.Availability == "Не поставляють на даний момент").ToList(),
                    _ => items
                };
            }

            items = model.SortOrder switch
            {
                "price_asc" => items.OrderBy(i => i.Price).ToList(),
                "price_desc" => items.OrderByDescending(i => i.Price).ToList(),
                _ => items
            };

            model.AvailableAvailabilityFilters = new List<SelectListItem>
            {
                new("Наявні", "available"),
                new("Нема в наявності", "not_available"),
                new("В дорозі", "in_transit"),
                new("Не поставляють на даний момент", "not_supplied")
            };

            model.AvailableSortOrders = new List<SelectListItem>
            {
                new("Ціна Up", "price_asc"),
                new("Ціна Down", "price_desc")
            };

            model.PartSuppliers = items;
            return View(model);
        }


        // GET: PartSuppliers
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.GetAllPartSuppliersAsync());
        //}

        // GET: PartSuppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var ps = await _db.GetPartSupplierByIdAsync(id.Value);
            if (ps == null)
                return NotFound();

            return View(ps);
        }

        // GET: PartSuppliers/Create
        public async Task<IActionResult> Create()
        {
            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName");
            ViewData["SupplierId"] = new SelectList(await _db.GetAllSuppliersAsync(), "Id", "DisplayName");
            ViewData["AvailabilityOptions"] = new SelectList(new[]
            {
                "Наявні", "Нема в наявності", "В дорозі", "Не поставляють на даний момент"
            });

            return View();
        }

        // POST: PartSuppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartSupplier partSupplier)
        {
            if (ModelState.IsValid)
            {
                await _db.AddPartSupplierAsync(partSupplier);
                return RedirectToAction(nameof(Index));
            }

            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName", partSupplier.PartId);
            ViewData["SupplierId"] = new SelectList(await _db.GetAllSuppliersAsync(), "Id", "DisplayName", partSupplier.SupplierId);
            ViewData["AvailabilityOptions"] = new SelectList(new[]
            {
                "Наявні", "Нема в наявності", "В дорозі", "Не поставляють на даний момент"
            });
            return View(partSupplier);
        }

        // GET: PartSuppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var ps = await _db.GetPartSupplierByIdAsync(id.Value);
            if (ps == null)
                return NotFound();

            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName", ps.PartId);
            ViewData["SupplierId"] = new SelectList(await _db.GetAllSuppliersAsync(), "Id", "DisplayName", ps.SupplierId);
            ViewData["AvailabilityOptions"] = new SelectList(new[]
            {
                "Наявні", "Нема в наявності", "В дорозі", "Не поставляють на даний момент"
            });
            return View(ps);
        }

        // POST: PartSuppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PartSupplier partSupplier)
        {
            if (id != partSupplier.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdatePartSupplierAsync(partSupplier);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.PartSupplierExistsAsync(partSupplier.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName", partSupplier.PartId);
            ViewData["SupplierId"] = new SelectList(await _db.GetAllSuppliersAsync(), "Id", "DisplayName", partSupplier.SupplierId);
            ViewData["AvailabilityOptions"] = new SelectList(new[]
            {
                "Наявні", "Нема в наявності", "В дорозі", "Не поставляють на даний момент"
            });
            return View(partSupplier);
        }

        // GET: PartSuppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var ps = await _db.GetPartSupplierByIdAsync(id.Value);
            if (ps == null)
                return NotFound();

            return View(ps);
        }

        // POST: PartSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeletePartSupplierAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }



    //public class PartSuppliersController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public PartSuppliersController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: PartSuppliers
    //    public async Task<IActionResult> Index()
    //    {
    //        var autoWorkshopContext = _context.PartSuppliers.Include(p => p.Part).Include(p => p.Supplier);
    //        return View(await autoWorkshopContext.ToListAsync());
    //    }

    //    // GET: PartSuppliers/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.PartSuppliers == null)
    //        {
    //            return NotFound();
    //        }

    //        var partSupplier = await _context.PartSuppliers
    //            .Include(p => p.Part)
    //            .Include(p => p.Supplier)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (partSupplier == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(partSupplier);
    //    }

    //    // GET: PartSuppliers/Create
    //    public IActionResult Create()
    //    {
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id");
    //        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Id");
    //        return View();
    //    }

    //    // POST: PartSuppliers/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,PartId,SupplierId,Price,Availability,DeliveryTimeDays")] PartSupplier partSupplier)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(partSupplier);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", partSupplier.PartId);
    //        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Id", partSupplier.SupplierId);
    //        return View(partSupplier);
    //    }

    //    // GET: PartSuppliers/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.PartSuppliers == null)
    //        {
    //            return NotFound();
    //        }

    //        var partSupplier = await _context.PartSuppliers.FindAsync(id);
    //        if (partSupplier == null)
    //        {
    //            return NotFound();
    //        }
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", partSupplier.PartId);
    //        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Id", partSupplier.SupplierId);
    //        return View(partSupplier);
    //    }

    //    // POST: PartSuppliers/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,PartId,SupplierId,Price,Availability,DeliveryTimeDays")] PartSupplier partSupplier)
    //    {
    //        if (id != partSupplier.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(partSupplier);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!PartSupplierExists(partSupplier.Id))
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
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Id", partSupplier.PartId);
    //        ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Id", partSupplier.SupplierId);
    //        return View(partSupplier);
    //    }

    //    // GET: PartSuppliers/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.PartSuppliers == null)
    //        {
    //            return NotFound();
    //        }

    //        var partSupplier = await _context.PartSuppliers
    //            .Include(p => p.Part)
    //            .Include(p => p.Supplier)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (partSupplier == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(partSupplier);
    //    }

    //    // POST: PartSuppliers/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.PartSuppliers == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.PartSuppliers'  is null.");
    //        }
    //        var partSupplier = await _context.PartSuppliers.FindAsync(id);
    //        if (partSupplier != null)
    //        {
    //            _context.PartSuppliers.Remove(partSupplier);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool PartSupplierExists(int id)
    //    {
    //      return (_context.PartSuppliers?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
