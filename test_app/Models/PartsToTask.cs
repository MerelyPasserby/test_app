using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace test_app.Models;

public partial class PartsToTask
{
    public int Id { get; set; }

    [Display(Name = "Завдання")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int TaskId { get; set; }

    [Display(Name = "Деталь")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int PartId { get; set; }

    [Display(Name = "К-сть")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int Quantity { get; set; }

    [Display(Name = "Ціна(момент використання)")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public decimal PriceAtUse { get; set; }

    [ValidateNever]
    public virtual Part Part { get; set; } = null!;

    [ValidateNever]
    public virtual Task Task { get; set; } = null!;
}
