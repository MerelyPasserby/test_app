using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class PartSupplierViewModel
    {
        public List<PartSupplier> PartSuppliers { get; set; }

        public string? SearchPartName { get; set; }
        public string? SearchSupplierName { get; set; }

        public string? AvailabilityFilter { get; set; }
        public string? SortOrder { get; set; }

        public List<SelectListItem>? AvailableAvailabilityFilters { get; set; }
        public List<SelectListItem>? AvailableSortOrders { get; set; }
    }
}
