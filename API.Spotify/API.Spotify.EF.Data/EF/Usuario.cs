using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Spotify.EF.Data.EF;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "El e-mail es obligatorio")]
    public string Email { get; set; } = null!;
}

