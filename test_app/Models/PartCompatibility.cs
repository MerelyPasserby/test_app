using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace test_app.Models;

public partial class PartCompatibility
{    
    public int Id { get; set; }

    [Display(Name = "Деталь")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int PartId { get; set; }

    [Display(Name = "Авто")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int CarId { get; set; }

    [ValidateNever]
    public virtual Car Car { get; set; } = null!;

    [ValidateNever]
    public virtual Part Part { get; set; } = null!;
}
