using Api_comerce.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Dtos
{
    public class EmpaqueDto
    {
        public int Id { get; set; }
        public string Empaque { get; set; }
        public double? Contenido { get; set; }
        public bool? Sincronizado { get; set; }
        public string CodigoEmpaque { get; set; }
        public DateTime? FechaCreado { get; set; }
        public UnidadSatDto UnidadSat { get; set; }
        public string Codigo { get; set; }
        public decimal? PCompra { get; set; }
        public decimal? PVenta { get; set; }
        public double? Descuento { get; set; }
        public bool? Activo { get; set; }
    }

    public class sizeEmpaqueDto
    {
        public int Id { get; set; }

      
        public string? Empaque { get; set; }

        public double? Contenido { get; set; }

        public bool? Sincronizado { get; set; }

       
        public string? CodigoEmpaque { get; set; }

        public DateTime? FechaCreado { get; set; }

        public int? UnidadId { get; set; }

     
        public virtual UnidadSAT? UnidadSAT { get; set; }
    }
}
