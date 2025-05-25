namespace Api_comerce.Dtos
{
    public class ProductoEmpaqueDto
    {
        public int ProductoId { get; set; }
        public int EmpaqueId { get; set; }
        public string? Codigo { get; set; }
        public decimal? PCompra { get; set; }
        public decimal? PVenta { get; set; }
        public float? Descuento { get; set; }
        public bool? Activo { get; set; }
    }
}
