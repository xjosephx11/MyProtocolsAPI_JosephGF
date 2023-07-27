namespace MyProtocolsAPI_JosephGF.ModelsDTOs
{
    public class UserDTO
    {
        //a modo de ejemplo los atributos del modelo estaran en espanol
        //un escenario como este puede sevir para que un equipo de trabajo del app
        //que solo sepa espanol, entienda de que trata

        public int IDUsuario { get; set; }
        public string Correo { get; set; } = null!;
        public string Contrasenia { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string CorreoRespaldo{ get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string? Direccion { get; set; }
        public bool? Activo { get; set; }
        public bool? EstaBloqueado { get; set; }
        public int IdRol { get; set; }
        public string DescripcionRol { get; set; } = null!;    
    }
}
