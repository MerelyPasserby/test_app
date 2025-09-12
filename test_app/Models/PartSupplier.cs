using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace test_app.Models;

public partial class PartSupplier
{
    public int Id { get; set; }

    [Display(Name = "Деталь")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int PartId { get; set; }

    [Display(Name = "Постачальник")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int SupplierId { get; set; }

    [Display(Name = "Ціна")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public decimal Price { get; set; }

    [Display(Name = "Наявність")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 30 символів")]
    public string Availability { get; set; } = null!;

    [Display(Name = "Час доставки")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int DeliveryTimeDays { get; set; }

    [ValidateNever]
    public virtual Part Part { get; set; } = null!;

    [ValidateNever]
    public virtual Supplier Supplier { get; set; } = null!;
}
