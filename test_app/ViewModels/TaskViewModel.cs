using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class TaskViewModel
    {
        public List<Models.Task> Tasks { get; set; }

        public string? SearchCar { get; set; }
        public string? SearchTaskType { get; set; }

        public string? DateRange { get; set; }
        public string? CompletionFilter { get; set; }
        public string? PaymentFilter { get; set; }

        public string? SortOrder { get; set; }

        public List<SelectListItem>? AvailableDateRanges { get; set; }
        public List<SelectListItem>? AvailableCompletionFilters { get; set; }
        public List<SelectListItem>? AvailablePaymentFilters { get; set; }
        public List<SelectListItem>? AvailableSortOrders { get; set; }
    }
}
