﻿namespace APIEntidades.Domain.Dto
{
    public class FiltroVideoJuegoDto
    {
        public int page { get; set; }
        public string? Nombre { get; set; }
        public string? Compania { get; set; }
        public int? Ano { get; set; }
    }
}
