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
    public class SuppliersController : Controller
    {
        private readonly DBService _db;

        public SuppliersController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(SupplierViewModel model)
        {
            var suppliers = await _db.GetAllSuppliersAsync();

            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                suppliers = suppliers.Where(s =>
                    s.Name.Contains(model.SearchName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(model.SearchAddress))
            {
                suppliers = suppliers.Where(s =>
                    s.Address.Contains(model.SearchAddress, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            suppliers = model.SortOrder switch
            {
                "name_asc" => suppliers.OrderBy(s => s.Name).ToList(),
                "name_desc" => suppliers.OrderByDescending(s => s.Name).ToList(),
                _ => suppliers
            };

            model.AvailableSortOrders = new List<SelectListItem>
            {
                new("Õ‡Á‚‡ AñZ", "name_asc"),
                new("Õ‡Á‚‡ ZñA", "name_desc")
            };

            model.Suppliers = suppliers;
            return View(model);
        }


        // GET: Suppliers
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.GetAllSuppliersAsync());
        //}

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await _db.GetSupplierByIdAsync(id.Value);
            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                await _db.AddSupplierAsync(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await _db.GetSupplierByIdAsync(id.Value);
            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateSupplierAsync(supplier);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.SupplierExistsAsync(supplier.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await _db.GetSupplierByIdAsync(id.Value);
            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeleteSupplierAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }



    //public class SuppliersController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public SuppliersController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: Suppliers
    //    public async Task<IActionResult> Index()
    //    {
    //          return _context.Suppliers != null ? 
    //                      View(await _context.Suppliers.ToListAsync()) :
    //                      Problem("Entity set 'AutoWorkshopContext.Suppliers'  is null.");
    //    }

    //    // GET: Suppliers/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.Suppliers == null)
    //        {
    //            return NotFound();
    //        }

    //        var supplier = await _context.Suppliers
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (supplier == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(supplier);
    //    }

    //    // GET: Suppliers/Create
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Suppliers/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,Name,Phone,Email,Address,Website")] Supplier supplier)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(supplier);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(supplier);
    //    }

    //    // GET: Suppliers/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.Suppliers == null)
    //        {
    //            return NotFound();
    //        }

    //        var supplier = await _context.Suppliers.FindAsync(id);
    //        if (supplier == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(supplier);
    //    }

    //    // POST: Suppliers/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,Email,Address,Website")] Supplier supplier)
    //    {
    //        if (id != supplier.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(supplier);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!SupplierExists(supplier.Id))
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
    //        return View(supplier);
    //    }

    //    // GET: Suppliers/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.Suppliers == null)
    //        {
    //            return NotFound();
    //        }

    //        var supplier = await _context.Suppliers
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (supplier == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(supplier);
    //    }

    //    // POST: Suppliers/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.Suppliers == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.Suppliers'  is null.");
    //        }
    //        var supplier = await _context.Suppliers.FindAsync(id);
    //        if (supplier != null)
    //        {
    //            _context.Suppliers.Remove(supplier);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool SupplierExists(int id)
    //    {
    //      return (_context.Suppliers?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
