namespace MyProtocolsAPI_JosephGF.ModelsDTOs
{
    public class UserRoleDTO
    {
        //un DTO data transfer object sirve para 2 funciones
        //1 simplificar la estructura de los json que se envian y llegan a los end point de los controllers
        //quitando composiciones inecesarias que solo harian que los json sean muy pesados o que muestren
        //informacion que no se desea ver (puede ser por seguridad)
        //2 ocultar la estructura real de los modelos y por tanto de las tablas nases de datos a los programadores
        //de las apps, paginas web o aplicaciones de escritorio.

        //tomando en cuenta el segundo criterio y solo a manera de ejemplo este DTO tendra los nombres de propiedades en espanol

        public int IDRolUsuario { get; set; }
        public string DescripcionRol { get; set; } = null!;
    }
}
