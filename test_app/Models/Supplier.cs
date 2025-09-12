using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace test_app.Models;

public partial class Supplier
{    
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 50 символів")]
    public string Name { get; set; } = null!;

    [Display(Name = "Номер")]
    [Required(ErrorMessage = "Введіть значення")]
    [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Введіть існуючий телефон +380*********")]
    public string Phone { get; set; } = null!;

    [Display(Name = "Пошта")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 50 символів")]
    [RegularExpression(@"^(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$", ErrorMessage = "Введіть існуючу пошту")]
    public string Email { get; set; } = null!;

    [Display(Name = "Адреса")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 100 символів")]
    public string Address { get; set; } = null!;

    [Display(Name = "Сайт")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 100 символів")]
    [RegularExpression(@"^https?:\/\/[a-z0-9\-\.]+\.[a-z]{2,}(/.*)?$", ErrorMessage = "Введіть існуючий вебсайт")]
    public string Website { get; set; } = null!;

    public virtual ICollection<PartSupplier> PartSuppliers { get; set; } = new List<PartSupplier>();

    public string DisplayName => $"{Name}";

}
