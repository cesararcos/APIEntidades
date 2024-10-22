﻿namespace WebEntidadesMVC.Models
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
    }

    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
