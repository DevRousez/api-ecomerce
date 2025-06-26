namespace Api_comerce.Dtos
{
    public class OrdenDto
    {
        public int Id { get; set; }

        public AccountDireccionDto Direccion { get; set; }

        public DatosFacturacionDto? DatosFacturacion { get; set; }

        public string MetodoPago { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public List<OrdenDetalleDto> Detalles { get; set; } = new();
    }
}
