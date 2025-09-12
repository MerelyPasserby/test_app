using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class PartsToTaskViewModel
    {
        public List<PartsToTask> PartsToTasks { get; set; }

        public string? SearchPartName { get; set; }
        public string? SearchCar { get; set; }

        public string? DateRange { get; set; }
        public string? SortOrder { get; set; }

        public List<SelectListItem>? AvailableDateRanges { get; set; }
        public List<SelectListItem>? AvailableSortOrders { get; set; }
    }
}
