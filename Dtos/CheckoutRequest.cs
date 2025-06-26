namespace Api_comerce.Dtos
{
    public class CheckoutRequest
    {
        public int AccountsDireccionId { get; set; }  // Obligatorio
        public int? DatosFacturacionId { get; set; }  // Opcional

        public string NombreCompleto { get; set; } // defaulkt de user
        public string Correo { get; set; }
        public MetodoPagoDto MetodoPago { get; set; }  // Ej: { "Tipo": "Tarjeta" }

        public List<CartItemDto> Items { get; set; } = new();
    }

    public class MetodoPagoDto
    {
        public string Tipo { get; set; } // ejemplo: "efectivo", "tarjeta", "transferencia"
    }
}
