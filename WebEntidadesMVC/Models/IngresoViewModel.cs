namespace WebEntidadesMVC.Models
{
    public class IngresoViewModel
    {
        public string? Correo { get; set; }
        public string? Clave { get; set; }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
    }
}
