using System.ComponentModel.DataAnnotations;

namespace WebEntidadesMVC.Models
{
    public class IngresoViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo es inválido.")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatorio.")]
        [DataType(DataType.Password)]
        public string? Clave { get; set; }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
    }
}
