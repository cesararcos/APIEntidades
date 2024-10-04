namespace APIEntidades.Domain.Dto
{
    public class VideoJuegosDto
    {
        public string? Nombre { get; set; }
        public string? Compania { get; set; }
        public int? Ano { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Puntaje { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? Usuario { get; set; }
    }
}
