using Api_comerce.Data;
using Api_comerce.Dtos;
using Api_comerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItemResultDto>> AddItemsToCartAsync(int userId, List<CartItemDto> items)
        {
            if (items == null || !items.Any()) return new List<CartItemResultDto>();

            var productIds = items.Select(i => i.ProductId).ToList();

            var products = await _context.ProductosEmpaque
                .Include(p => p.ImagenProducto)
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var existingCartItems = await _context.CartItems
                .Where(c => c.UserId == userId && productIds.Contains(c.ProductId))
                .ToDictionaryAsync(c => c.ProductId);

            var now = DateTime.UtcNow;
            var result = new List<CartItem>();

            foreach (var item in items)
            {
                if (!products.TryGetValue(item.ProductId, out var product))
                    continue;

                if (existingCartItems.TryGetValue(item.ProductId, out var existing))
                {
                    existing.Quantity += item.Quantity;
                    existing.UpdatedAt = now;
                    result.Add(existing);
                }
                else
                {
                    var newItem = new CartItem
                    {
                        UserId = userId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = (decimal)product.PVenta,
                        CreatedAt = now,
                        UpdatedAt = now,
                        ProductEmpaque = product
                    };

                    _context.CartItems.Add(newItem);
                    result.Add(newItem);
                }
            }

            await _context.SaveChangesAsync();

            // Convertir a DTOs
            var resultDto = result.Select(ci => new CartItemResultDto
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Price,
                ProductEmpaque = new ProductEmpaqueDto
                {
                    Id = ci.ProductEmpaque.Id,
                    Codigo = ci.ProductEmpaque.Codigo,
                    PVenta = (decimal)ci.ProductEmpaque.PVenta,
                    ImagenProducto = ci.ProductEmpaque.ImagenProducto?.Select(img => new ImagenProductoDto
                    {
                        Url = img.Url,
                        Type = img.Type,
                        Label = img.Label
                    }).ToList()
                }
            }).ToList();

            return resultDto;
        }

        public async Task<List<CartItemDto>> GetCartAsync(int userId)
        {
            var items = await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Select(ci => new CartItemDto
            {
                Id = ci.Id,
                UserId = ci.UserId,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Price,
                CreatedAt = ci.CreatedAt,
                UpdatedAt = ci.UpdatedAt,
                User = new AccountsDtocs
                {
                    Id =ci.User.Id,
                  AccountTypeId = ci.User.AccountTypeId,

                    FullName = ci.User.FullName,

                    Email = ci.User.Email,

                  //  Password = "",

                    GoogleId = ci.User.GoogleId,
                    Picture = ci.User.Picture,

                    IsActive = ci.User.IsActive,

                    CreatedAt = ci.User.CreatedAt,

                    UpdatedAt = ci.User.UpdatedAt,

                },
                ProductEmpaque = new ProductoEmpaqueDto
                {
                    Id = ci.ProductEmpaque.Id,
                    ProductoId = ci.ProductEmpaque.ProductoId,
                    EmpaqueId = ci.ProductEmpaque.EmpaqueId,
                    Codigo = ci.ProductEmpaque.Codigo,
                    PCompra = ci.ProductEmpaque.PCompra,
                    PVenta = ci.ProductEmpaque.PVenta,
                    Descuento = (float?)ci.ProductEmpaque.Descuento,
                    Activo = ci.ProductEmpaque.Activo,
                    ImagenProducto = null,
                    Empaque = 
                    new EmpaqueDto
                    {
                        Id = ci.ProductEmpaque.Empaque.Id,
                        Empaque = ci.ProductEmpaque.Empaque.Empaque,
                        Contenido = ci.ProductEmpaque.Empaque.Contenido,
                        Sincronizado = ci.ProductEmpaque.Empaque.Sincronizado,
                        CodigoEmpaque = ci.ProductEmpaque.Empaque.CodigoEmpaque,
                        FechaCreado = ci.ProductEmpaque.Empaque.FechaCreado,
                        UnidadSat = null


                    },
                    Producto = new ProductoDto
                    {
                        Id = ci.ProductEmpaque.Producto.Id,
                        ProductoSatId = ci.ProductEmpaque.Producto.ProductoSatId,
                        Prefijo = ci.ProductEmpaque.Producto.Prefijo,
                        NombreProducto = ci.ProductEmpaque.Producto.NombreProducto,
                        Slug = ci.ProductEmpaque.Producto.Slug,
                        Rating = ci.ProductEmpaque.Producto.Rating,
                        Acumulador = ci.ProductEmpaque.Producto.Acumulador,
                        ProductoIdAcumulador = ci.ProductEmpaque.Producto.ProductoIdAcumulador,
                        Linea = new LineaDto { 
                            Id= (int)ci.ProductEmpaque.Producto.LineaId,
                            Linea= ci.ProductEmpaque.Producto.Linea.Linea,
                            Slug = ci.ProductEmpaque.Producto.Linea.Slug,
                             FechaCreado = ci.ProductEmpaque.Producto.CreatedAt,
                             
                        },
                        MarcaProducto = new MarcaProductoDto { 
                            Id= ci.ProductEmpaque.Producto.MarcaProducto.Id,
                            Marca= ci.ProductEmpaque.Producto.MarcaProducto.Marca,
                            Slug = ci.ProductEmpaque.Producto.MarcaProducto.Slug
                        },
                        //,
                        //LineaId = ci.ProductEmpaque.Producto.LineaId,
                        //MarcaId = ci.ProductEmpaque.Producto.MarcaId,

                    }
                }
            })
            .ToListAsync();

            return   items ;
        }

        public async Task<bool> RemoveItemAsync(int userId, int productId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var items = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> FinalizeOrderAsync(int userId, string metodoPago = "Simulado")
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                throw new InvalidOperationException("El carrito está vacío.");

            var now = DateTime.UtcNow;
            var total = cartItems.Sum(c => c.Quantity * c.Price);

            var order = new Order
            {
                UserId = userId,
                Total = total,
                CreatedAt = now,
                UpdatedAt = now,
                Status = "Procesando",               // estado general de la orden
                PaymentMethod = metodoPago,             // nuevo campo
                PaymentStatus = "Paid",              // lo marcamos como pagado desde el inicio (simulado)
                PaidAt = now,                       
                PaymentReference = $"SIM-{Guid.NewGuid().ToString().Substring(0, 8)}" // ref xxxixixixixixos
            };

            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message;
                throw new Exception("Error al guardar la orden: " + innerMessage, ex);
            }

            var orderItems = cartItems.Select(c => new OrderItem
            {
                OrderId = order.Id,
                ProductEmpaqueId = c.ProductId,
                Quantity = c.Quantity,
                UnitPrice = c.Price
            }).ToList();

            _context.OrderItems.AddRange(orderItems);

            _context.CartItems.RemoveRange(cartItems); // limpiar carrito

            await _context.SaveChangesAsync();

            return order;
        }

    }
}
