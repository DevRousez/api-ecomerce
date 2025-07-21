using System.ComponentModel.DataAnnotations;
using Api_comerce.Dtos;
using Api_comerce.Models;

namespace Api_comerce.Dtos
{
    public class ProductoEcommerceDto
    {
        public int ProductoEmpaqueId { get; set; }
        public string Name { get; set; }
        public bool Featured { get; set; } = false;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public bool OnSale { get; set; } = false;
        public string Slug { get; set; }
        public bool IsStock { get; set; } = true;
        public int RatingCount { get; set; } = 0;
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public string CategoriaTipo { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<EmpaqueDto> Sizes { get; set; }
        //public sizeEmpaqueDto SizesEmpaque { get; set; }
        public List<string> Colors { get; set; }
        public List<MarcaProductoDto> Badges { get; set; }
        public List<ImageDto> Images { get; set; } = new();
        public ImageDto Thumbnail { get; set; }
        public ImageDto ThumbnailBack { get; set; }
        public List<MarcaProductoDto > ProductBrands { get; set; } 
        public List<LineaDto> ProductCategories { get; set; }
        public List<ProductosCaracteristicasDTO> ProductosCaracteristicas { get; set; }
    }
    public class ImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public FormatDto Formats { get; set; }
    }
    public class spImageDto
    {
        public int Id { get; set; }
            public string Name { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<spFormatDto> Formats  { get; set; }
    }
    public class spFormatDto
    {
        public string Name { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string Hash { get; set; }
        public string Ext { get; set; }
        public string Mime { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double Size { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
    }
    public class FormatDto
    {
        public FormatItemDto Thumbnail { get; set; }
        public FormatItemDto Small { get; set; }
        public FormatItemDto Medium { get; set; }
        public FormatItemDto Large { get; set; } 
    }
  

    public class SimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }
    public class FormatItemDto
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }

    public class spProductoEcommerceDto
    {
        public int ProductoEmpaqueId { get; set; }
        public string Name { get; set; }
        public bool Featured { get; set; } = false;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public bool OnSale { get; set; } = false;
        public string Slug { get; set; }
        public bool IsStock { get; set; } = true;
        public int RatingCount { get; set; } = 0;
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public string CategoriaTipo { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<spEmpaqueDto> Sizes { get; set; }
        //public sizeEmpaqueDto SizesEmpaque { get; set; }
        public List<string> Colors { get; set; }
        public List<MarcaProductoDto> Badges { get; set; }
        public List<spImageDto> Images { get; set; } = new();
        public spImagenProductoDto Thumbnail { get; set; }
        public spImagenProductoDto ThumbnailBack { get; set; }
        public List<MarcaProductoDto> ProductBrands { get; set; }
        public List<LineaDto> ProductCategories { get; set; }
        public List<ProductosCaracteristicasDTO> ProductosCaracteristicas { get; set; }
    }

    public class spImagenProductoDto
    {

        public int Id { get; set; }
        public int ProductEmpaqueId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // "thumbnail"
        public string Label { get; set; } // "front", "back", etc.
        public string Hash { get; set; }
        public string Ext { get; set; }
        public string Mime { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public decimal Size { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }


    }
}
