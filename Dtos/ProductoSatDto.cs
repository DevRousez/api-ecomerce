namespace Api_comerce.Dtos
{
    public class ProductoSatDto
    {
        public int Id { get; set; }
        public string ClaveProd { get; set; } = "NO DATO";
        public string Descripcion { get; set; } = "NO DATO";
        public DateTime? FechaCreado { get; set; }
    }
}
