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
    public class ClientsController : Controller
    {
        private readonly DBService _db;

        public ClientsController(DBService db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(ClientViewModel model)
        {
            var clients = await _db.GetAllClientsAsync();
            
            if (!string.IsNullOrWhiteSpace(model.SearchName))
            {
                clients = clients.Where(c => c.FullName.Contains(model.SearchName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

                        
            clients = model.SortOrder switch
            {
                "name_asc" => clients.OrderBy(c => c.FullName).ToList(),
                "name_desc" => clients.OrderByDescending(c => c.FullName).ToList(),
                _ => clients
            };
            
            model.SortOptions = new List<SelectListItem>
            {
                new("Ім’я: від А до Я", "name_asc", model.SortOrder == "name_asc"),
                new("Ім’я: від Я до А", "name_desc", model.SortOrder == "name_desc")
            };

            model.Clients = clients;
            return View(model);
        }


        // GET: Clients
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _db.GetAllClientsAsync());
        //}

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _db.GetClientByIdAsync(id.Value);
            if (client == null)
                return NotFound();

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                await _db.AddClientAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _db.GetClientByIdAsync(id.Value);
            if (client == null)
                return NotFound();

            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _db.UpdateClientAsync(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.ClientExistsAsync(client.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var client = await _db.GetClientByIdAsync(id.Value);
            if (client == null)
                return NotFound();

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }



    //public class ClientsController : Controller
    //{
    //    private readonly AutoWorkshopContext _context;

    //    public ClientsController(AutoWorkshopContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: Clients
    //    public async Task<IActionResult> Index()
    //    {
    //          return _context.Clients != null ? 
    //                      View(await _context.Clients.ToListAsync()) :
    //                      Problem("Entity set 'AutoWorkshopContext.Clients'  is null.");
    //    }

    //    // GET: Clients/Details/5
    //    public async Task<IActionResult> Details(int? id)
    //    {
    //        if (id == null || _context.Clients == null)
    //        {
    //            return NotFound();
    //        }

    //        var client = await _context.Clients
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (client == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(client);
    //    }

    //    // GET: Clients/Create
    //    public IActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Clients/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,FullName,Phone,Email")] Client client)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(client);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(client);
    //    }

    //    // GET: Clients/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null || _context.Clients == null)
    //        {
    //            return NotFound();
    //        }

    //        var client = await _context.Clients.FindAsync(id);
    //        if (client == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(client);
    //    }

    //    // POST: Clients/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Phone,Email")] Client client)
    //    {
    //        if (id != client.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(client);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!ClientExists(client.Id))
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
    //        return View(client);
    //    }

    //    // GET: Clients/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null || _context.Clients == null)
    //        {
    //            return NotFound();
    //        }

    //        var client = await _context.Clients
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (client == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(client);
    //    }

    //    // POST: Clients/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        if (_context.Clients == null)
    //        {
    //            return Problem("Entity set 'AutoWorkshopContext.Clients'  is null.");
    //        }
    //        var client = await _context.Clients.FindAsync(id);
    //        if (client != null)
    //        {
    //            _context.Clients.Remove(client);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool ClientExists(int id)
    //    {
    //      return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
