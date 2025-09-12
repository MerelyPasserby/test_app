using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using test_app.Services;
using test_app.ViewModels;

namespace test_app.Controllers
{
    public class TaskCostCalculatorController : Controller
    {
        private readonly DBService _db;

        public TaskCostCalculatorController(DBService db)
        {
            _db = db;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new TaskCostCalculatorViewModel
            {
                AvailableCars = (await _db.GetAllCarsAsync())
                    .Select(c => new SelectListItem(c.DisplayName, c.Id.ToString())).ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> SelectParts(TaskCostCalculatorViewModel model)
        {
            if (model.SelectedCarId == null)
                return RedirectToAction("Index");

            var compatiblePartIds = (await _db.GetAllPartCompatibilitiesAsync())
                .Where(p => p.CarId == model.SelectedCarId)
                .Select(p => p.PartId)
                .ToHashSet();

            var compatibleParts = (await _db.GetAllPartsAsync())
                .Where(p => compatiblePartIds.Contains(p.Id))
                .Select(p => new SelectListItem(p.DisplayName, p.Id.ToString()))
                .ToList();

            var taskTypes = (await _db.GetAllTaskTypesAsync())
                .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
                .ToList();

            model.CompatibleParts = compatibleParts;
            model.AvailableTaskTypes = taskTypes;
           
            model.AvailableCars = (await _db.GetAllCarsAsync())
                .Select(c => new SelectListItem(c.DisplayName, c.Id.ToString())).ToList();

            return View("Index", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Calculate(TaskCostCalculatorViewModel model)
        {
            var parts = await _db.GetAllPartsAsync();
            var taskTypes = await _db.GetAllTaskTypesAsync();

            model.Part1Cost = model.Part1Id.HasValue
                ? parts.FirstOrDefault(p => p.Id == model.Part1Id)?.Price * model.Part1Quantity
                : 0;

            model.Part2Cost = model.Part2Id.HasValue
                ? parts.FirstOrDefault(p => p.Id == model.Part2Id)?.Price * model.Part2Quantity
                : 0;

            model.Part3Cost = model.Part3Id.HasValue
                ? parts.FirstOrDefault(p => p.Id == model.Part3Id)?.Price * model.Part3Quantity
                : 0;

            model.TaskCost = model.SelectedTaskTypeId.HasValue
                ? taskTypes.FirstOrDefault(t => t.Id == model.SelectedTaskTypeId)?.LaborCost
                : 0;

            model.TotalCost = (model.Part1Cost ?? 0) + (model.Part2Cost ?? 0) + (model.Part3Cost ?? 0) + (model.TaskCost ?? 0);

            var partCompatibilities = await _db.GetAllPartCompatibilitiesAsync();

            model.CompatibleParts = parts
                .Where(p => model.SelectedCarId != null &&
                            partCompatibilities.Any(pc => pc.CarId == model.SelectedCarId && pc.PartId == p.Id))
                .Select(p => new SelectListItem(p.DisplayName, p.Id.ToString()))
                .ToList();          

            model.AvailableTaskTypes = taskTypes
                .Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList();

            model.AvailableCars = (await _db.GetAllCarsAsync())
                .Select(c => new SelectListItem(c.DisplayName, c.Id.ToString())).ToList();

            return View("Index", model);
        }
    }
}
