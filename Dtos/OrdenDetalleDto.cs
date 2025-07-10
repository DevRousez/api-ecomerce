namespace Api_comerce.Dtos
{
    public class OrdenDetalleDto
    {
        public int ProductoEmpaqueId { get; set; }
        public float Cantidad { get; set; }
        public decimal Precio { get; set; }

       public ProductoEmpaqueOrdenDto ProductoEmpaque { get; set; }

    }
}
