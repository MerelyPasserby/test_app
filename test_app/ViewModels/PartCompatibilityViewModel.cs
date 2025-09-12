using test_app.Models;

namespace test_app.ViewModels
{
    public class PartCompatibilityViewModel
    {
        public List<PartCompatibility> PartCompatibilities { get; set; }

        public string? SearchPartName { get; set; }
        public string? SearchCarName { get; set; }
    }
}
