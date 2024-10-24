using System.ComponentModel.DataAnnotations;

namespace WebEntidadesMVC.Models
{
    public class CreateGameViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La compania es obligatorio.")]
        public string? Compania { get; set; }

        [Required(ErrorMessage = "El ano es obligatorio.")]
        public int? Ano { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal? Precio { get; set; }
    }
}
