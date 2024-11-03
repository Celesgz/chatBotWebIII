using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Botify.Data.EF;
public partial class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [RegularExpression(@"^[a-zA-Z0-9_.-]+$", ErrorMessage = "El nombre de usuario solo puede contener letras, números, guiones bajos (_), puntos (.) y guiones (-).")]
    [DisplayName("Nombre de usuario")]
    [MaxLength(20)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [MaxLength(20, ErrorMessage = "La contraseña no puede tener más de 20 caracteres.")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*])[A-Za-z\\d!@#$%^&*]{8,}$",
                   ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula, una minúscula, un dígito y un carácter especial.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "El e-mail es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
    [MaxLength(100, ErrorMessage = "El e-mail debe tener hasta 100 caracteres")]
    [DisplayName("E-mail")]
    public string Email { get; set; } = null!;
}

