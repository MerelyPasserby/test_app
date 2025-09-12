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
    public class ClientCarsController : Controller
    {
        private readonly DBService _db;

        public ClientCarsController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(ClientCarViewModel model)
        {
            var clientCars = await _db.GetAllClientCarsAsync();

            if (!string.IsNullOrWhiteSpace(model.SearchByRegNum))
                clientCars = clientCars.Where(c => c.RegistrationNum.Contains(model.SearchByRegNum, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(model.SearchByClientName))
                clientCars = clientCars.Where(c => c.Client.FullName.Contains(model.SearchByClientName, StringComparison.OrdinalIgnoreCase)).ToList();

            model.SortOptions = new List<SelectListItem>
            {
                new("Car (A-Z)", "car_asc"),
                new("Car (Z-A)", "car_desc")
            };

            clientCars = model.SortOrder switch
            {
                "car_asc" => clientCars.OrderBy(c => c.Car.DisplayName).ToList(),
                "car_desc" => clientCars.OrderByDescending(c => c.Car.DisplayName).ToList(),
                _ => clientCars
            };

            model.ClientCars = clientCars;
            return View(model);
        }


        // GET: ClientCars
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.GetAllClientCarsAsync());
        //}

        // GET: ClientCars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var clientCar = await _db.GetClientCarByIdAsync(id.Value);
            if (clientCar == null)
                return NotFound();

            return View(clientCar);
        }

        // GET: ClientCars/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CarId"] = new SelectList(await _db.GetAllCarsAsync(), "Id", "DisplayName");
            ViewData["ClientId"] = new SelectList(await _db.GetAllClientsAsync(), "Id", "DisplayName");            
            return View();
        }

        // POST: ClientCars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCar clientCar)
        {
            if (ModelState.IsValid)
            {
                await _db.AddClientCarAsync(clientCar);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarId"] = new SelectList(await _db.GetAllCarsAsync(), "Id", "DisplayName", clientCar.CarId);
            ViewData["ClientId"] = new SelectList(await _db.GetAllClientsAsync(), "Id", "DisplayName", clientCar.ClientId);
            return View(clientCar);
        }

        // GET: ClientCars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var clientCar = await _db.GetClientCarByIdAsync(id.Value);
            if (clientCar == null)
                return NotFound();

            ViewData["CarId"] = new SelectList(await _db.GetAllCarsAsync(), "Id", "DisplayName", clientCar.CarId);
            ViewData["ClientId"] = new SelectList(await _db.GetAllClientsAsync(), "Id", "DisplayName", clientCar.ClientId);
            return View(clientCar);
        }

        // POST: ClientCars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientCar clientCar)
        {
            if (id != clientCar.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateClientCarAsync(clientCar);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.ClientCarExistsAsync(clientCar.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarId"] = new SelectList(await _db.GetAllCarsAsync(), "Id", "DisplayName", clientCar.CarId);
            ViewData["ClientId"] = new SelectList(await _db.GetAllClientsAsync(), "Id", "DisplayName", clientCar.ClientId);
            return View(clientCar);
        }

        // GET: ClientCars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var clientCar = await _db.GetClientCarByIdAsync(id.Value);
            if (clientCar == null)
                return NotFound();

            return View(clientCar);
        }

        // POST: ClientCars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeleteClientCarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

    //public class ClientCarsController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public ClientCarsController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: ClientCars
    //    public async Task<IActionResult> Index()
    //    {
    //        var autoWorkshopContext = _context.ClientCars.Include(c => c.Car).Include(c => c.Client);
    //        return View(await autoWorkshopContext.ToListAsync());
    //    }

    //    // GET: ClientCars/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.ClientCars == null)
    //        {
    //            return NotFound();
    //        }

    //        var clientCar = await _context.ClientCars
    //            .Include(c => c.Car)
    //            .Include(c => c.Client)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (clientCar == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(clientCar);
    //    }

    //    // GET: ClientCars/Create
    //    public IActionResult Create()
    //    {
    //        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id");
    //        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id");
    //        return View();
    //    }

    //    // POST: ClientCars/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,ClientId,CarId,Vin,RegistrationNum")] ClientCar clientCar)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(clientCar);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", clientCar.CarId);
    //        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", clientCar.ClientId);
    //        return View(clientCar);
    //    }

    //    // GET: ClientCars/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.ClientCars == null)
    //        {
    //            return NotFound();
    //        }

    //        var clientCar = await _context.ClientCars.FindAsync(id);
    //        if (clientCar == null)
    //        {
    //            return NotFound();
    //        }
    //        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", clientCar.CarId);
    //        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", clientCar.ClientId);
    //        return View(clientCar);
    //    }

    //    // POST: ClientCars/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,CarId,Vin,RegistrationNum")] ClientCar clientCar)
    //    {
    //        if (id != clientCar.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(clientCar);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!ClientCarExists(clientCar.Id))
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
    //        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", clientCar.CarId);
    //        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", clientCar.ClientId);
    //        return View(clientCar);
    //    }

    //    // GET: ClientCars/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.ClientCars == null)
    //        {
    //            return NotFound();
    //        }

    //        var clientCar = await _context.ClientCars
    //            .Include(c => c.Car)
    //            .Include(c => c.Client)
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (clientCar == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(clientCar);
    //    }

    //    // POST: ClientCars/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.ClientCars == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.ClientCars'  is null.");
    //        }
    //        var clientCar = await _context.ClientCars.FindAsync(id);
    //        if (clientCar != null)
    //        {
    //            _context.ClientCars.Remove(clientCar);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool ClientCarExists(int id)
    //    {
    //      return (_context.ClientCars?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
