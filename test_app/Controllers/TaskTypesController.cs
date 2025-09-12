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
    public class TaskTypesController : Controller
    {
        private readonly DBService _db;

        public TaskTypesController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(TaskTypeViewModel model)
        {
            var taskTypes = await _db.GetAllTaskTypesAsync();

            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                taskTypes = taskTypes.Where(t =>
                    t.Name.Contains(model.SearchName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(model.NormHoursFilter))
            {
                taskTypes = model.NormHoursFilter switch
                {
                    "under1" => taskTypes.Where(t => t.NormTime < 1).ToList(),
                    "1to2" => taskTypes.Where(t => t.NormTime >= 1 && t.NormTime <= 2).ToList(),
                    "2to4" => taskTypes.Where(t => t.NormTime > 2 && t.NormTime <= 4).ToList(),
                    "over4" => taskTypes.Where(t => t.NormTime > 4).ToList(),
                    _ => taskTypes
                };
            }

            taskTypes = model.SortOrder switch
            {
                "price_asc" => taskTypes.OrderBy(t => t.LaborCost).ToList(),
                "price_desc" => taskTypes.OrderByDescending(t => t.LaborCost).ToList(),
                _ => taskTypes
            };

            model.AvailableNormHoursFilters = new List<SelectListItem>
            {
                new("ּוםרו 1 דמה", "under1"),
                new("1–2 דמה", "1to2"),
                new("2–4 דמה", "2to4"),
                new("ֱ³כüרו 4 דמה", "over4")
            };

            model.AvailableSortOrders = new List<SelectListItem>
            {
                new("ײ³םא Up", "price_asc"),
                new("ײ³םא Dowm", "price_desc")
            };

            model.TaskTypes = taskTypes;
            return View(model);
        }


        // GET: TaskTypes
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.GetAllTaskTypesAsync());
        //}

        // GET: TaskTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var taskType = await _db.GetTaskTypeByIdAsync(id.Value);
            if (taskType == null)
                return NotFound();

            return View(taskType);
        }

        // GET: TaskTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskType taskType)
        {
            if (ModelState.IsValid)
            {
                await _db.AddTaskTypeAsync(taskType);
                return RedirectToAction(nameof(Index));
            }
            return View(taskType);
        }

        // GET: TaskTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var taskType = await _db.GetTaskTypeByIdAsync(id.Value);
            if (taskType == null)
                return NotFound();

            return View(taskType);
        }

        // POST: TaskTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskType taskType)
        {
            if (id != taskType.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateTaskTypeAsync(taskType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.TaskTypeExistsAsync(taskType.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskType);
        }

        // GET: TaskTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var taskType = await _db.GetTaskTypeByIdAsync(id.Value);
            if (taskType == null)
                return NotFound();

            return View(taskType);
        }

        // POST: TaskTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeleteTaskTypeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

    //public class TaskTypesController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public TaskTypesController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: TaskTypes
    //    public async Task<IActionResult> Index()
    //    {
    //          return _context.TaskTypes != null ? 
    //                      View(await _context.TaskTypes.ToListAsync()) :
    //                      Problem("Entity set 'AutoWorkshopContext.TaskTypes'  is null.");
    //    }

    //    // GET: TaskTypes/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.TaskTypes == null)
    //        {
    //            return NotFound();
    //        }

    //        var taskType = await _context.TaskTypes
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (taskType == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(taskType);
    //    }

    //    // GET: TaskTypes/Create
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: TaskTypes/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,Name,NormTime,LaborCost")] TaskType taskType)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(taskType);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(taskType);
    //    }

    //    // GET: TaskTypes/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.TaskTypes == null)
    //        {
    //            return NotFound();
    //        }

    //        var taskType = await _context.TaskTypes.FindAsync(id);
    //        if (taskType == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(taskType);
    //    }

    //    // POST: TaskTypes/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NormTime,LaborCost")] TaskType taskType)
    //    {
    //        if (id != taskType.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(taskType);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!TaskTypeExists(taskType.Id))
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
    //        return View(taskType);
    //    }

    //    // GET: TaskTypes/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.TaskTypes == null)
    //        {
    //            return NotFound();
    //        }

    //        var taskType = await _context.TaskTypes
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (taskType == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(taskType);
    //    }

    //    // POST: TaskTypes/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.TaskTypes == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.TaskTypes'  is null.");
    //        }
    //        var taskType = await _context.TaskTypes.FindAsync(id);
    //        if (taskType != null)
    //        {
    //            _context.TaskTypes.Remove(taskType);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool TaskTypeExists(int id)
    //    {
    //      return (_context.TaskTypes?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
