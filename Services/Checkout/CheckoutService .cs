using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using static Api_comerce.Models.Orden;

namespace Api_comerce.Services.Checkout
{
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _context;

        public CheckoutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearOrdenAsync(CheckoutRequest dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("La orden no contiene productos.");

            var orden = new Orden
            {
                NombreCompleto = $"{dto.Nombre} {dto.Apellidos}",
                Direccion = dto.Direccion,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                MetodoPago = dto.MetodoPago.Tipo,
                Estado = "Pendiente",
                FechaCreacion = DateTime.Now
            };

            foreach (var item in dto.Items)
            {
                orden.Detalles.Add(new OrdenDetalle
                {
                    ProductoId = item.ProductEmpaqueId,
                    Cantidad = item.Quantity,
                    Precio = item.Price,
                 
                });
            }

            _context.Orden.Add(orden);
            await _context.SaveChangesAsync();

            return orden.Id;
        }

    }
}
