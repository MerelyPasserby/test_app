using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class InspectionViewModel
    {
        public List<Inspection> Inspections { get; set; }
        
        public string? SearchByRegNum { get; set; }
        public string? SearchByClientName { get; set; }
        
        public string? DateFilter { get; set; }
        public List<SelectListItem>? AvailableDateFilters { get; set; }
       
        public string? SortOrder { get; set; }
        public List<SelectListItem>? SortOptions { get; set; }
    }
}
