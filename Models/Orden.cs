using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
   
        public class Orden
        {
            [Key]
            public int Id { get; set; }

            public string NombreCompleto { get; set; }
            public string Direccion { get; set; }
            public string Correo { get; set; }
            public string Telefono { get; set; }
            public string MetodoPago { get; set; }
            public string Estado { get; set; } = "Pendiente";

            public DateTime FechaCreacion { get; set; } = DateTime.Now;

            public ICollection<OrdenDetalle> Detalles { get; set; } = new List<OrdenDetalle>();
        }
  }
