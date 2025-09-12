using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace test_app.Models;

public partial class TaskType
{
    public int Id { get; set; }

    [Display(Name = "Тип")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 50 символів")]
    [RegularExpression(@"^(?i)[a-zа-яіїєґ .,-]+$", ErrorMessage = "Допускаються тільки літери")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Норм час")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public double NormTime { get; set; }

    [Required(ErrorMessage = "Ціна роботи")]
    [Range(1, int.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public int LaborCost { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public string DisplayName => $"{Name}";

}
