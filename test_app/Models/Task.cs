using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace test_app.Models;

public partial class Task
{
    public int Id { get; set; }

    [Display(Name = "Огляд")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int InspectionId { get; set; }

    [Display(Name = "Тип Завдання")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int TaskTypeId { get; set; }

    [Display(Name = "Створено")]
    [Required(ErrorMessage = "Введіть значення")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Завершено")]    
    public DateTime? CompletedAt { get; set; }

    [Display(Name = "Коментар")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Довжина має бути мін. 1 символ")]
    public string Comment { get; set; } = null!;

    [Display(Name = "Вартість")]
    public decimal TotalCost { get; set; }

    [Display(Name = "Оплачено")]
    public bool IsPaid { get; set; }

    [ValidateNever]
    public virtual Inspection Inspection { get; set; } = null!;

    public virtual ICollection<PartsToTask> PartsToTasks { get; set; } = new List<PartsToTask>();

    [ValidateNever]
    public virtual TaskType TaskType { get; set; } = null!;

    public string DisplayName => $"{String.Join(' ', CreatedAt, TaskType?.Name, Inspection?.ClientCar?.DisplayName)}";

}
