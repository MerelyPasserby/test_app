using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace test_app.Models;

public partial class Part
{    
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 50 символів")]
    public string Name { get; set; } = null!;

    [Display(Name = "Артикул")]
    [Required(ErrorMessage = "Введіть значення")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Довжина має бути від 1 до 50 символів")]
    [RegularExpression(@"^(?i)[a-z0-9\-./]+$", ErrorMessage = "Введіть існуючий артикул")]
    public string Article { get; set; } = null!;

    [Display(Name = "К-сть(на складі)")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(0, int.MaxValue, ErrorMessage = "Значення має бути 0 або більше")]
    public int StockQuantity { get; set; }

    [Display(Name = "Ціна")]
    [Required(ErrorMessage = "Введіть значення")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Значення має бути більше 0")]
    public decimal Price { get; set; }

    public virtual ICollection<PartCompatibility> PartCompatibilities { get; set; } = new List<PartCompatibility>();

    public virtual ICollection<PartSupplier> PartSuppliers { get; set; } = new List<PartSupplier>();

    public virtual ICollection<PartsToTask> PartsToTasks { get; set; } = new List<PartsToTask>();

    public string DisplayName => $"{String.Join(' ', Name, Article)}";

}
