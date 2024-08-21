namespace Answers_API_Examen_KeirynSandi.ModelsDTOs
{
    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }

        public string? Correo { get; set; }

        public string? Nombre { get; set; }

        public string? Telefono { get; set; }

        public string? Contrasennia { get; set; }

        public int RolID { get; set; }

        public string? RolDescripcion { get; set; }
    }
}
