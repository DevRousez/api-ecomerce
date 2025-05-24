using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Models
{
    public class ImagenProducto
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? AlternativeText { get; set; }
        public string? Caption { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Hash { get; set; } = string.Empty;
        public string Ext { get; set; } = string.Empty;
        public string Mime { get; set; } = string.Empty;
        public double Size { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? PreviewUrl { get; set; }
        public string Provider { get; set; } = "local";
        public string? ProviderMetadata { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        // Clave foránea compuesta
        public int ProductoId { get; set; }
        public int EmpaqueId { get; set; }

        [ForeignKey(nameof(ProductoId) + "," + nameof(EmpaqueId))]
        public ProductosEmpaque ProductosEmpaque { get; set; } = null!;

    // public ICollection<ImagenFormato> Formats { get; set; } = new List<ImagenFormato>();
    }
}