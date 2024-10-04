using System;
using System.Collections.Generic;

namespace APIEntidades.Domain.Entities;

public partial class Calificaciones
{
    public Guid Id { get; set; }

    public string? Nickname { get; set; }

    public Guid? Videojuego { get; set; }

    public decimal? Puntaje { get; set; }

    public virtual Videojuegos? VideojuegoNavigation { get; set; }
}
