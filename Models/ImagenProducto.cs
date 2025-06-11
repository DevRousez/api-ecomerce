using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Models
{
    public class ImagenProducto
    {
        [Key]
        public int Id { get; set; }

        public int ProductEmpaqueId { get; set; }

        public string Type { get; set; } = "original"; // original, thumbnail, small, etc.

        public string Label { get; set; } // front, back, etc.

        public string Url { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public decimal? SizeMb { get; set; }

        public string MimeType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ProductosEmpaque ProductEmpaque { get; set; }
    }
}