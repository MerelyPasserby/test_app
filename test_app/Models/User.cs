using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace test_app.Models;

public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email")]
    [Display(Name = "Пошта")]
    [Required(ErrorMessage = "Введіть пошту")]
    [RegularExpression(@"^(?i)[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$", ErrorMessage = "Введіть існуючу пошту")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Введіть пароль")]
    [StringLength(255)]
    public string Password { get; set; } = null!;

    public int Attempts { get; set; } = 0;

    public DateTime TimeLocked { get; set; } = new DateTime(2001, 1, 1);
}
