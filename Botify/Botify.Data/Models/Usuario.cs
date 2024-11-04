using System;
using System.Collections.Generic;

namespace Botify.Data.Models;

public partial class Usuario
{
    public string Nombre { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Id { get; set; }
}
