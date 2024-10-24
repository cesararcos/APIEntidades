using System.ComponentModel.DataAnnotations;

namespace WebEntidadesMVC.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo es inválido.")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatorio.")]
        public string? Clave { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string? Usuario { get; set; }
    }
}
