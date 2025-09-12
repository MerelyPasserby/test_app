using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace test_app.Models;

public partial class Inspection
{
    public int Id { get; set; }

    [Display(Name = "Клінтське Авто")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int ClientCarId { get; set; }

    [Display(Name = "Дата Огляду")]
    [Required(ErrorMessage = "Введіть значення")]
    public DateTime InspectionDate { get; set; }

    [Display(Name = "Результати")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Довжина має бути мін. 1 символ")]
    public string Results { get; set; } = null!;

    [Display(Name = "Вартість")]    
    public decimal Cost { get; set; } = 0;

    [Display(Name = "Оплачено")]   
    public bool IsPaid { get; set; }

    [ValidateNever]
    public virtual ClientCar ClientCar { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public string DisplayName => $"{String.Join(' ', InspectionDate, ClientCar?.DisplayName)}"; // Не совсем понял почему, надо разобраться 

}
