using Api_comerce.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace Api_comerce.Reports.ventas
{
    public class NotaVentaPdfCorreo
    {
        public static byte[] Generar(Orden orden)
        {
            using var ms = new MemoryStream();
            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontRegular = new XFont("Arial", 12, XFontStyle.Regular);
            var fontBold = new XFont("Arial", 14, XFontStyle.Bold);
            var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);

            int marginLeft = 40;
            int y = 40;
            int lineHeight = 22;

            // Título
            gfx.DrawString($"NOTA DE VENTA #{orden.Id}", fontTitle, XBrushes.Black, new XRect(0, y, page.Width, lineHeight), XStringFormats.TopCenter);
            y += 40;

            // Línea separadora
            gfx.DrawLine(XPens.Black, marginLeft, y, page.Width - marginLeft, y);
            y += 20;

            // Datos cliente y orden
            gfx.DrawString($"Cliente:", fontBold, XBrushes.Black, marginLeft, y);
            gfx.DrawString(orden.NombreCompleto ?? "NO DATO", fontRegular, XBrushes.Black, marginLeft + 80, y);
            y += lineHeight;

            var direccion = orden.AccountsDireccion;
            if (direccion != null)
            {
                gfx.DrawString($"Dirección:", fontBold, XBrushes.Black, marginLeft, y);
                string direccionCompleta = $"{direccion.Calle}, {direccion.Ciudad}, {direccion.Estado}, {direccion.CodigoPostal}, {direccion.Pais}";
                gfx.DrawString(direccionCompleta, fontRegular, XBrushes.Black, marginLeft + 80, y);
                y += lineHeight;
                gfx.DrawString($"Teléfono:", fontBold, XBrushes.Black, marginLeft, y);
                gfx.DrawString(direccion.Telefono ?? "NO DATO", fontRegular, XBrushes.Black, marginLeft + 80, y);
                y += lineHeight;
            }

            gfx.DrawString($"Correo:", fontBold, XBrushes.Black, marginLeft, y);
            gfx.DrawString(orden.Correo ?? "NO DATO", fontRegular, XBrushes.Black, marginLeft + 80, y);
            y += lineHeight;

            gfx.DrawString($"Fecha:", fontBold, XBrushes.Black, marginLeft, y);
            gfx.DrawString($"{orden.FechaCreacion:dd/MM/yyyy}", fontRegular, XBrushes.Black, marginLeft + 80, y);
            y += lineHeight;

            gfx.DrawString($"Método de pago:", fontBold, XBrushes.Black, marginLeft, y);
            gfx.DrawString(orden.MetodoPago ?? "NO DATO", fontRegular, XBrushes.Black, marginLeft + 120, y);
            y += lineHeight + 10;

            // Línea separadora
            gfx.DrawLine(XPens.Black, marginLeft, y, page.Width - marginLeft, y);
            y += 10;

            // Tabla de productos: encabezado
            int colProducto = marginLeft;
            int colCantidad = 300;
            int colPrecioUnit = 370;
            int colTotal = 460;

            gfx.DrawString("Producto", fontBold, XBrushes.Black, colProducto, y);
            gfx.DrawString("Cantidad", fontBold, XBrushes.Black, colCantidad, y);
            gfx.DrawString("Precio Unitario", fontBold, XBrushes.Black, colPrecioUnit, y);
            gfx.DrawString("Total", fontBold, XBrushes.Black, colTotal, y);
            y += lineHeight;

            gfx.DrawLine(XPens.Black, marginLeft, y, page.Width - marginLeft, y);
            y += 5;

            decimal subtotal = 0m;

            foreach (var item in orden.Detalles)
            {
                string nombreProd = item.ProductoEmpaque.Codigo + "-"+ item.ProductoEmpaque?.Producto?.NombreProducto ?? "NO DATO" + "-"+ item.ProductoEmpaque.Empaque.Empaque?? "no dato";
                int cantidad = item.Cantidad;
                decimal precioUnit = item.Precio;
                decimal total = precioUnit * cantidad;

                gfx.DrawString(nombreProd, fontRegular, XBrushes.Black, colProducto, y);
                gfx.DrawString(cantidad.ToString(), fontRegular, XBrushes.Black, colCantidad, y);
                gfx.DrawString(precioUnit.ToString("C2"), fontRegular, XBrushes.Black, colPrecioUnit, y);
                gfx.DrawString(total.ToString("C2"), fontRegular, XBrushes.Black, colTotal, y);

                y += lineHeight;
                subtotal += total;
            }

            y += 10;
            gfx.DrawLine(XPens.Black, marginLeft, y, page.Width - marginLeft, y);
            y += lineHeight;

            // Impuestos (por ejemplo 16%)
            decimal impuesto = subtotal * 0.16m;
            decimal totalGeneral = subtotal + impuesto;

            gfx.DrawString("Subtotal:", fontBold, XBrushes.Black, colTotal - 100, y);
            gfx.DrawString(subtotal.ToString("C2"), fontRegular, XBrushes.Black, colTotal, y);
            y += lineHeight;

            gfx.DrawString("IVA (16%):", fontBold, XBrushes.Black, colTotal - 100, y);
            gfx.DrawString(impuesto.ToString("C2"), fontRegular, XBrushes.Black, colTotal, y);
            y += lineHeight;

            gfx.DrawString("TOTAL:", fontBold, XBrushes.Black, colTotal - 100, y);
            gfx.DrawString(totalGeneral.ToString("C2"), fontRegular, XBrushes.Black, colTotal, y);
            y += lineHeight + 20;

            // Pie de página
            gfx.DrawString("Gracias por su compra!", fontBold, XBrushes.Black, new XRect(0, page.Height - 40, page.Width, lineHeight), XStringFormats.Center);

            document.Save(ms);
            return ms.ToArray();
        }
    }
}
