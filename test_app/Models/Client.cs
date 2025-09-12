using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace test_app.Models;

public partial class Client
{
    public int Id { get; set; }

    [Display(Name = "ПІБ")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 100 символів")]
    [RegularExpression(@"^(?i)[a-zа-яіїєґ .,-]+$", ErrorMessage = "Допускаються тільки літери")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Телефон")]
    [Required(ErrorMessage = "Введіть значення")]
    [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Введіть існуючий телефон +380*********")]
    public string Phone { get; set; } = null!;

    [Display(Name = "Пошта")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 100 символів")]
    [RegularExpression(@"^(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$", ErrorMessage = "Введіть існуючу пошту")]
    public string Email { get; set; } = null!;

    public virtual ICollection<ClientCar> ClientCars { get; set; } = new List<ClientCar>();
    public string DisplayName => $"{FullName}";
}
