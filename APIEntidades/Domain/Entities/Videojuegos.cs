using System;
using System.Collections.Generic;

namespace APIEntidades.Domain.Entities;

public partial class Videojuegos
{
    public Guid Id { get; set; }

    public string? Nombre { get; set; }

    public string? Compania { get; set; }

    public int? Ano { get; set; }

    public decimal? Precio { get; set; }

    public decimal? Puntaje { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Usuario { get; set; }

    public virtual ICollection<Calificaciones> Calificaciones { get; set; } = new List<Calificaciones>();
}
