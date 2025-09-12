using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class SupplierViewModel
    {
        public List<Supplier> Suppliers { get; set; }

        public string? SearchName { get; set; }
        public string? SearchAddress { get; set; }

        public string? SortOrder { get; set; }

        public List<SelectListItem>? AvailableSortOrders { get; set; }
    }
}
