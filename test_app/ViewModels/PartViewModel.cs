using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class PartViewModel
    {
        public List<Part> Parts { get; set; }
        
        public string? SearchByName { get; set; }
        public string? SearchByArticle { get; set; }
        
        public string? PriceFilter { get; set; }
        public List<SelectListItem>? AvailablePriceFilters { get; set; }

        public string? SortOrder { get; set; }
        public List<SelectListItem>? SortOptions { get; set; }
    }
}
