using System.Net;
using System.Net.Mail;
using System.Text;


namespace Api_comerce.Services.correos
{
    public class CorreoSevice : ICorreoSevice

    {
        private readonly string smtpHost = "smtp.gmail.com"; 
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "sayerventas9@gmail.com";
        private readonly string smtpPass = "$@yer2000.";
        private readonly string smtpAppPass = "nomdbwjbkjfruxvq";
        public async Task EnviarNotaVentaAsync(string destino, byte[] pdfBytes, int ordenId)
        {
            var remitente = new MailAddress(smtpUser, "Tu Empresa");
            var destinatario = new MailAddress(destino);

            var mensaje = new MailMessage(remitente, destinatario)
            {
                Subject = $"Tu nota de venta #{ordenId}",
                Body = "Gracias por tu compra. Adjuntamos tu nota de venta en PDF.",
                IsBodyHtml = false,
                BodyEncoding = Encoding.UTF8
            };

            // Adjuntar el PDF
            var pdfAttachment = new Attachment(new MemoryStream(pdfBytes), $"nota-venta-{ordenId}.pdf", "application/pdf");
            mensaje.Attachments.Add(pdfAttachment);

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpAppPass)
            };

            smtp.Send(mensaje);
        }
    }
}
