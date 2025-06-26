using Api_comerce.Dtos;

namespace Api_comerce.Services.Checkout
{
    public interface ICheckoutService
    {
        Task<int> CrearOrdenAsync(CheckoutRequest dto);

        Task<OrdenDto> GetOrdenByIdAsync(int ordenId);
        Task<bool> MarcarOrdenComoPagadaAsync(int ordenId);
        
            
    }
}
