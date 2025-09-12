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
    public class PartsToTasksController : Controller
    {
        private readonly DBService _db;

        public PartsToTasksController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(PartsToTaskViewModel model)
        {
            var items = await _db.GetAllPartsToTasksAsync();

            if (!string.IsNullOrWhiteSpace(model.SearchPartName))
            {
                items = items.Where(p =>
                    p.Part.DisplayName.Contains(model.SearchPartName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(model.SearchCar))
            {
                items = items.Where(p =>
                    p.Task.Inspection.ClientCar.DisplayName.Contains(model.SearchCar, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(model.DateRange))
            {
                var now = DateTime.Now;
                items = model.DateRange switch
                {
                    "week" => items.Where(p => (now - p.Task.CreatedAt).TotalDays <= 7).ToList(),
                    "month" => items.Where(p => (now - p.Task.CreatedAt).TotalDays > 7 && (now - p.Task.CreatedAt).TotalDays <= 30).ToList(),
                    "3months" => items.Where(p => (now - p.Task.CreatedAt).TotalDays > 30 && (now - p.Task.CreatedAt).TotalDays <= 90).ToList(),
                    "6months" => items.Where(p => (now - p.Task.CreatedAt).TotalDays > 90 && (now - p.Task.CreatedAt).TotalDays <= 180).ToList(),
                    "older" => items.Where(p => (now - p.Task.CreatedAt).TotalDays > 180).ToList(),
                    _ => items
                };
            }

            items = model.SortOrder switch
            {
                "date_asc" => items.OrderBy(p => p.Task.CreatedAt).ToList(),
                "date_desc" => items.OrderByDescending(p => p.Task.CreatedAt).ToList(),
                _ => items
            };

            model.AvailableDateRanges = new List<SelectListItem>
            {
                new("Останній тиждень", "week"),
                new("Від тижня до місяця", "month"),
                new("Від місяця до 3 міс.", "3months"),
                new("Від 3 до 6 міс.", "6months"),
                new("Старші за 6 міс.", "older")
            };

            model.AvailableSortOrders = new List<SelectListItem>
            {
                new("Дата завдання Up", "date_asc"),
                new("Дата завдання Down", "date_desc")
            };

            model.PartsToTasks = items;
            return View(model);
        }


        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.GetAllPartsToTasksAsync());
        //}

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var item = await _db.GetPartToTaskAsync(id.Value);
            if (item == null)
                return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName");
            ViewData["TaskId"] = new SelectList(await _db.GetAllTasksAsync(), "Id", "DisplayName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartsToTask partsToTask)
        {
            if (ModelState.IsValid)
            {
                await _db.AddPartToTaskAsync(partsToTask);
                return RedirectToAction(nameof(Index));
            }

            ViewData["PartId"] = new SelectList(await _db.GetAllPartsAsync(), "Id", "DisplayName", partsToTask.PartId);
            ViewData["TaskId"] = new SelectList(await _db.GetAllTasksAsync(), "Id", "DisplayName", partsToTask.TaskId);
            return View(partsToTask);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _db.GetPartToTaskAsync(id.Value);
            if (item == null)
                return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeletePartFromTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

    //public class PartsToTasksController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public PartsToTasksController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: PartsToTasks
    //    public async Task<IActionResult> Index()
    //    {
    //        var autoWorkshopContext = _context.PartsToTasks.Include(p => p.Part).Include(p => p.Task);
    //        return View(await autoWorkshopContext.ToListAsync());
    //    }

    //    // GET: PartsToTasks/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.PartsToTasks == null)
    //        {
    //            return NotFound();
    //        }

    //        var partsToTask = await _context.PartsToTasks
    //            .Include(p => p.Part)
    //            .Include(p => p.Task)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (partsToTask == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(partsToTask);
    //    }

    //    // GET: PartsToTasks/Create
    //    public IActionResult Create()
    //    {
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Article");
    //        ViewData["TaskId"] = new SelectList(_context.Tasks, "Id", "Comment");
    //        return View();
    //    }

    //    // POST: PartsToTasks/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,TaskId,PartId,Quantity,PriceAtUse")] PartsToTask partsToTask)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(partsToTask);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Article", partsToTask.PartId);
    //        ViewData["TaskId"] = new SelectList(_context.Tasks, "Id", "Comment", partsToTask.TaskId);
    //        return View(partsToTask);
    //    }

    //    // GET: PartsToTasks/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.PartsToTasks == null)
    //        {
    //            return NotFound();
    //        }

    //        var partsToTask = await _context.PartsToTasks.FindAsync(id);
    //        if (partsToTask == null)
    //        {
    //            return NotFound();
    //        }
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Article", partsToTask.PartId);
    //        ViewData["TaskId"] = new SelectList(_context.Tasks, "Id", "Comment", partsToTask.TaskId);
    //        return View(partsToTask);
    //    }

    //    // POST: PartsToTasks/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,TaskId,PartId,Quantity,PriceAtUse")] PartsToTask partsToTask)
    //    {
    //        if (id != partsToTask.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(partsToTask);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!PartsToTaskExists(partsToTask.Id))
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
    //        ViewData["PartId"] = new SelectList(_context.Parts, "Id", "Article", partsToTask.PartId);
    //        ViewData["TaskId"] = new SelectList(_context.Tasks, "Id", "Comment", partsToTask.TaskId);
    //        return View(partsToTask);
    //    }

    //    // GET: PartsToTasks/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.PartsToTasks == null)
    //        {
    //            return NotFound();
    //        }

    //        var partsToTask = await _context.PartsToTasks
    //            .Include(p => p.Part)
    //            .Include(p => p.Task)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (partsToTask == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(partsToTask);
    //    }

    //    // POST: PartsToTasks/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.PartsToTasks == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.PartsToTasks'  is null.");
    //        }
    //        var partsToTask = await _context.PartsToTasks.FindAsync(id);
    //        if (partsToTask != null)
    //        {
    //            _context.PartsToTasks.Remove(partsToTask);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool PartsToTaskExists(int id)
    //    {
    //      return (_context.PartsToTasks?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
