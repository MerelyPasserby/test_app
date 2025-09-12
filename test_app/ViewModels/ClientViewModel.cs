using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class ClientViewModel
    {
        public List<Client> Clients { get; set; }
        
        public string? SearchName { get; set; }
      
        public string? SortOrder { get; set; }

        public List<SelectListItem>? SortOptions { get; set; }
    }
}
