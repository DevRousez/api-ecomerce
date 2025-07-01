using Api_comerce.Dtos;

namespace Api_comerce.Services.correos
{
    public interface ICorreoSevice

    {
        //void EnviarCorreo(string destino, string asunto, string cuerpo, bool esHtml = false, List<AdjuntoCorreoDto>? adjuntos = null);

        Task EnviarNotaVentaAsync(string destino, byte[] pdfAdjunto, int ordenId);
    }
}
