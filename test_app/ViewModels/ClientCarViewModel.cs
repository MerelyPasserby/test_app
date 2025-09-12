using Microsoft.AspNetCore.Mvc.Rendering;
using test_app.Models;

namespace test_app.ViewModels
{
    public class ClientCarViewModel
    {
        public List<ClientCar> ClientCars { get; set; }

        public string? SearchByRegNum { get; set; }
        public string? SearchByClientName { get; set; }
        public string? SortOrder { get; set; }

        public List<SelectListItem>? SortOptions { get; set; }
    }
}
