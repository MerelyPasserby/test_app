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
    public class TasksController : Controller
    {
        private readonly DBService _db;

        public TasksController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(TaskViewModel model)
        {
            var tasks = await _db.GetAllTasksAsync();

            if (!string.IsNullOrWhiteSpace(model.SearchCar))
            {
                tasks = tasks.Where(t =>
                    t.Inspection.ClientCar.DisplayName.Contains(model.SearchCar, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(model.SearchTaskType))
            {
                tasks = tasks.Where(t =>
                    t.TaskType.Name.Contains(model.SearchTaskType, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(model.DateRange))
            {
                var now = DateTime.Now;
                tasks = model.DateRange switch
                {
                    "week" => tasks.Where(t => (now - t.CreatedAt).TotalDays <= 7).ToList(),
                    "month" => tasks.Where(t => (now - t.CreatedAt).TotalDays > 7 && (now - t.CreatedAt).TotalDays <= 30).ToList(),
                    "3months" => tasks.Where(t => (now - t.CreatedAt).TotalDays > 30 && (now - t.CreatedAt).TotalDays <= 90).ToList(),
                    "6months" => tasks.Where(t => (now - t.CreatedAt).TotalDays > 90 && (now - t.CreatedAt).TotalDays <= 180).ToList(),
                    "older" => tasks.Where(t => (now - t.CreatedAt).TotalDays > 180).ToList(),
                    _ => tasks
                };
            }

            if (!string.IsNullOrEmpty(model.CompletionFilter))
            {
                tasks = model.CompletionFilter switch
                {
                    "completed" => tasks.Where(t => t.CompletedAt != null).ToList(),
                    "not_completed" => tasks.Where(t => t.CompletedAt == null).ToList(),
                    _ => tasks
                };
            }

            if (!string.IsNullOrEmpty(model.PaymentFilter))
            {
                tasks = model.PaymentFilter switch
                {
                    "paid" => tasks.Where(t => t.IsPaid).ToList(),
                    "unpaid" => tasks.Where(t => !t.IsPaid).ToList(),
                    _ => tasks
                };
            }

            tasks = model.SortOrder switch
            {
                "date_asc" => tasks.OrderBy(t => t.CreatedAt).ToList(),
                "date_desc" => tasks.OrderByDescending(t => t.CreatedAt).ToList(),
                _ => tasks
            };

            model.AvailableDateRanges = new List<SelectListItem>
            {
                new("Останній тиждень", "week"),
                new("Від тижня до місяця", "month"),
                new("Від місяця до 3 міс.", "3months"),
                new("Від 3 до 6 міс.", "6months"),
                new("Старші за 6 міс.", "older")
            };

            model.AvailableCompletionFilters = new List<SelectListItem>
            {
                new("Завершені", "completed"),
                new("Незавершені", "not_completed")
            };

            model.AvailablePaymentFilters = new List<SelectListItem>
            {
                new("Оплачені", "paid"),
                new("Неоплачені", "unpaid")
            };


            model.AvailableSortOrders = new List<SelectListItem>
            {
                new("Дата Up", "date_asc"),
                new("Дата Down", "date_desc")
            };

            model.Tasks = tasks;
            return View(model);
        }


        // GET: Tasks
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.GetAllTasksAsync());
        //}

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var task = await _db.GetTaskByIdAsync(id.Value);
            if (task == null) return NotFound();

            return View(task);
        }

        // GET: Tasks/Create
        public async Task<IActionResult> Create()
        {
            ViewData["InspectionId"] = new SelectList(await _db.GetAllInspectionsAsync(), "Id", "DisplayName");
            ViewData["TaskTypeId"] = new SelectList(await _db.GetAllTaskTypesAsync(), "Id", "DisplayName");
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Task task)
        {
            if (ModelState.IsValid)
            {
                await _db.AddTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }

            ViewData["InspectionId"] = new SelectList(await _db.GetAllInspectionsAsync(), "Id", "DisplayName", task.InspectionId);
            ViewData["TaskTypeId"] = new SelectList(await _db.GetAllTaskTypesAsync(), "Id", "DisplayName", task.TaskTypeId);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _db.GetTaskByIdAsync(id.Value);
            if (task == null) return NotFound();

            ViewData["InspectionId"] = new SelectList(await _db.GetAllInspectionsAsync(), "Id", "DisplayName", task.InspectionId);
            ViewData["TaskTypeId"] = new SelectList(await _db.GetAllTaskTypesAsync(), "Id", "DisplayName", task.TaskTypeId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Task task)
        {
            if (id != task.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateTaskAsync(task);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.TaskExistsAsync(task.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["InspectionId"] = new SelectList(await _db.GetAllInspectionsAsync(), "Id", "DisplayName", task.InspectionId);
            ViewData["TaskTypeId"] = new SelectList(await _db.GetAllTaskTypesAsync(), "Id", "DisplayName", task.TaskTypeId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = await _db.GetTaskByIdAsync(id.Value);
            if (task == null) return NotFound();

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }



    //public class TasksController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public TasksController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: Tasks
    //    public async Task<IActionResult> Index()
    //    {
    //        var autoWorkshopContext = _context.Tasks.Include(t => t.Inspection).Include(t => t.TaskType);
    //        return View(await autoWorkshopContext.ToListAsync());
    //    }

    //    // GET: Tasks/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.Tasks == null)
    //        {
    //            return NotFound();
    //        }

    //        var task = await _context.Tasks
    //            .Include(t => t.Inspection)
    //            .Include(t => t.TaskType)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (task == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(task);
    //    }

    //    // GET: Tasks/Create
    //    public IActionResult Create()
    //    {
    //        ViewData["InspectionId"] = new SelectList(_context.Inspections, "Id", "Id");
    //        ViewData["TaskTypeId"] = new SelectList(_context.TaskTypes, "Id", "Id");
    //        return View();
    //    }

    //    // POST: Tasks/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,InspectionId,TaskTypeId,CreatedAt,CompletedAt,Comment,TotalCost,IsPaid")] Models.Task task)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(task);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["InspectionId"] = new SelectList(_context.Inspections, "Id", "Id", task.InspectionId);
    //        ViewData["TaskTypeId"] = new SelectList(_context.TaskTypes, "Id", "Id", task.TaskTypeId);
    //        return View(task);
    //    }

    //    // GET: Tasks/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.Tasks == null)
    //        {
    //            return NotFound();
    //        }

    //        var task = await _context.Tasks.FindAsync(id);
    //        if (task == null)
    //        {
    //            return NotFound();
    //        }
    //        ViewData["InspectionId"] = new SelectList(_context.Inspections, "Id", "Id", task.InspectionId);
    //        ViewData["TaskTypeId"] = new SelectList(_context.TaskTypes, "Id", "Id", task.TaskTypeId);
    //        return View(task);
    //    }

    //    // POST: Tasks/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,InspectionId,TaskTypeId,CreatedAt,CompletedAt,Comment,TotalCost,IsPaid")] Models.Task task)
    //    {
    //        if (id != task.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(task);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!TaskExists(task.Id))
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
    //        ViewData["InspectionId"] = new SelectList(_context.Inspections, "Id", "Id", task.InspectionId);
    //        ViewData["TaskTypeId"] = new SelectList(_context.TaskTypes, "Id", "Id", task.TaskTypeId);
    //        return View(task);
    //    }

    //    // GET: Tasks/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.Tasks == null)
    //        {
    //            return NotFound();
    //        }

    //        var task = await _context.Tasks
    //            .Include(t => t.Inspection)
    //            .Include(t => t.TaskType)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (task == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(task);
    //    }

    //    // POST: Tasks/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.Tasks == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.Tasks'  is null.");
    //        }
    //        var task = await _context.Tasks.FindAsync(id);
    //        if (task != null)
    //        {
    //            _context.Tasks.Remove(task);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //    public IActionResult Error()
    //    {
    //        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //    }

    //    private bool TaskExists(int id)
    //    {
    //      return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
