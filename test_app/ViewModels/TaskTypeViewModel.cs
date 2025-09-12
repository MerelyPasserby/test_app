using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class TaskTypeViewModel
    {
        public List<TaskType> TaskTypes { get; set; }

        public string? SearchName { get; set; }
        public string? NormHoursFilter { get; set; }
        public string? SortOrder { get; set; }

        public List<SelectListItem>? AvailableNormHoursFilters { get; set; }
        public List<SelectListItem>? AvailableSortOrders { get; set; }
    }
}
