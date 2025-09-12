using Microsoft.AspNetCore.Mvc.Rendering;

namespace test_app.ViewModels
{
    public class TaskCostCalculatorViewModel
    {
        public int? SelectedCarId { get; set; }
        public List<SelectListItem>? AvailableCars { get; set; }
        
        public List<SelectListItem>? CompatibleParts { get; set; }
        
        public int? Part1Id { get; set; }
        public int Part1Quantity { get; set; }

        public int? Part2Id { get; set; }
        public int Part2Quantity { get; set; }

        public int? Part3Id { get; set; }
        public int Part3Quantity { get; set; }
        
        public int? SelectedTaskTypeId { get; set; }
        public List<SelectListItem>? AvailableTaskTypes { get; set; }
        
        public decimal? Part1Cost { get; set; }
        public decimal? Part2Cost { get; set; }
        public decimal? Part3Cost { get; set; }
        public decimal? TaskCost { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
