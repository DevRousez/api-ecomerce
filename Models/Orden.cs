using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
   
        public class Orden
        {
        [Key]
        public int Id { get; set; }

        public string NombreCompleto { get; set; }
        public string Direccion { get; set; } // Esto puede quedar o eliminarse si usas DireccionEnvio
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string MetodoPago { get; set; }
        public string Estado { get; set; } = "Pendiente";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación con DatosFacturacion
        public int? DatosFacturacionId { get; set; }
        public DatosFacturacion DatosFacturacion { get; set; }

        // Relación con Dirección de envío
        public int? AccountsDireccionId { get; set; }
        public AccountsDirecciones AccountsDireccion { get; set; }

        public ICollection<OrdenDetalle> Detalles { get; set; } = new List<OrdenDetalle>();
    }
  }
