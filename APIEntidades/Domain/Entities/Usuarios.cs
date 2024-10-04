using System;
using System.Collections.Generic;

namespace APIEntidades.Domain.Entities;

public partial class Usuarios
{
    public Guid Id { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public string? Usuario { get; set; }
}
