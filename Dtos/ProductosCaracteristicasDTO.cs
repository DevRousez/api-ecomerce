namespace Api_comerce.Dtos
{
    public class ProductosCaracteristicasDTO
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public DateTime FechaCreado { get; set; }
    }
}
