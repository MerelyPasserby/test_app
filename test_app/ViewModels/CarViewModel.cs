using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class CarViewModel
    {
        public List<Car> Cars { get; set; }
       
        public string? SearchString { get; set; }

        public string? YearRange { get; set; }
        
        public string? SortOrder { get; set; }
       
        public List<SelectListItem>? AvailableYearRanges { get; set; }
        public List<SelectListItem>? AvailableSortOrders { get; set; }
    }
}
