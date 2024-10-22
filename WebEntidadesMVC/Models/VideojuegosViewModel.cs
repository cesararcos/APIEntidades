namespace WebEntidadesMVC.Models
{
    public class VideojuegosViewModel
    {
        public string? Nombre { get; set; }
        public string? Compania { get; set; }
        public int? Ano { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Puntaje { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? Usuario { get; set; }
        public int? TotalPaginas { get; set; }
    }

    public class PaginacionViewModel<T>
    {   
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanoPagina { get; set; }
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
        public List<T> Elementos { get; set; }
    }
}
