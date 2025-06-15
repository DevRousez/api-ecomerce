namespace Api_comerce.Dtos
{
    public class CheckoutRequest
    {
        // Datos del cliente
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        // Método de pago seleccionado (puede ser "efectivo", "tarjeta", etc.)
        public MetodoPagoDto MetodoPago { get; set; }

        // Productos en el carrito
        public List<CartItemDto> Items { get; set; } = new();
    }

    public class MetodoPagoDto
    {
        public string Tipo { get; set; } // ejemplo: "efectivo", "tarjeta", "transferencia"
    }
}
