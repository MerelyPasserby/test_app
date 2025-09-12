using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace test_app.Models;

public partial class Car
{
    public int Id { get; set; }

    [Display(Name = "Марка")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 30 символів")]
    public string Brand { get; set; } = null!;

    [Display(Name = "Модель")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 30 символів")]
    public string Model { get; set; } = null!;

    [Display(Name = "Рік(початок)")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1960, 2026, ErrorMessage = "Рік має бути від 1960 до 2026")]
    public int YearFrom { get; set; }

    [Display(Name = "Рік(кінець)")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1960, 2026, ErrorMessage = "Рік має бути від 1960 до 2026")]
    public int YearTo { get; set; }

    [Display(Name = "Двигун")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 50 символів")]
    public string Engine { get; set; } = null!;

    [Display(Name = "Коробка передач")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 50 символів")]
    public string Transmission { get; set; } = null!;

    public virtual ICollection<ClientCar> ClientCars { get; set; } = new List<ClientCar>();

    public virtual ICollection<PartCompatibility> PartCompatibilities { get; set; } = new List<PartCompatibility>();

    public string DisplayName => $"{String.Join(' ', Brand, Model, YearFrom.ToString() + '-' + YearTo.ToString(), Engine, Transmission)}";
}
