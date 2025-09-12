using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace test_app.Models;

public partial class ClientCar
{
    public int Id { get; set; }

    [Display(Name = "Клієнт")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int ClientId { get; set; }

    [Display(Name = "Авто")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int CarId { get; set; }

    [Display(Name = "VIN")]
    [Required(ErrorMessage = "Введіть значення")]
    [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "Введіть існуючий VIN")]
    public string Vin { get; set; } = null!;

    [Display(Name = "Гос Номер")]
    [Required(ErrorMessage = "Введіть значення")]
    [RegularExpression(@"^[АВСЕНІКМОРТХ]{2}\d{4}[АВСЕНІКМОРТХ]{2}$", ErrorMessage = "Введіть існуючий госномер")]
    public string RegistrationNum { get; set; } = null!;

    [ValidateNever]
    public virtual Car Car { get; set; } = null!;

    [ValidateNever]
    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();

    public string DisplayName => $"{String.Join(' ', Car?.DisplayName, RegistrationNum)}";
}
