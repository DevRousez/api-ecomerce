namespace Api_comerce.Dtos
{
    public class AdjuntoCorreoDto
    {
        public byte[] Contenido { get; set; } = null!;
        public string NombreArchivo { get; set; } = "archivo.pdf";
        public string TipoMime { get; set; } = "application/pdf";
    }
}
