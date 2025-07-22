using Api_comerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_comerce.Dtos
{
    public class catLineaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<catProductoDTO> Products { get; set; } = new();
    }
    public class catProductoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Featured { get; set; } = false;
        public double Price { get; set; }
        public double? SalePrice { get; set; } = null;
        public bool OnSale { get; set; } = false;
        public string Slug { get; set; }
        public bool IsStock { get; set; }
        public int RatingCount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<cSizeDTO> Sizes { get; set; } = new();
        public List<string> Colors { get; set; } = new();
        public List<string> Badges { get; set; } = new();
        public List<cImageDTO> Images { get; set; } = new();
        public cImageDTO Thumbnail { get; set; }
        public cImageDTO ThumbnailBack { get; set; }
    }


    public class cSizeDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public double? PCompra { get; set; }
        public double? PVenta { get; set; }
        public double? Descuento { get; set; }
        public bool? Activo { get; set; }
    }

    public class cImageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public cImageFormatDTO Formats { get; set; }
    }

    public class cImageFormatDTO
    {
        public cImageSizeDTO Thumbnail { get; set; }
        public cImageSizeDTO Small { get; set; }
        public cImageSizeDTO Medium { get; set; }
        public cImageSizeDTO Large { get; set; }
    }
    public class cImageSizeDTO
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Ext { get; set; }
        public string Mime { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double Size { get; set; }
        public string Url { get; set; }
    }



    [Keyless]
    public class ProductoPlano
    {
        public int ProductoEmpaqueId { get; set; }

        public string NombreProductoCodigo { get; set; }
        public int productId { get; set; }

        public bool Featured { get; set; }

        public decimal Price { get; set; }
        public decimal PCompra { get; set; }
        public decimal Descuento { get; set; }

        public decimal SalePrice { get; set; }

        public bool OnSale { get; set; }

        public string Slug { get; set; }
        public string Codigo { get; set; }

        public bool IsStock { get; set; }
        public bool Activo { get; set; }

        public int ProductoSatId { get; set; }

        public int RatingCount { get; set; }


        public int ProductoIdAcumulador { get; set; }
        public bool Acumulador { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string CategoriaTipo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Prefijo { get; set; }

        // Este campo debe ser deserializado manualmente si estás trayendo Sizes como JSON
        public string Sizes { get; set; }
        public string ProductosCaracteristicas { get; set; }

        public string ProductoImagenes { get; set; }

        public int Id_Marca { get; set; }

        public string Marca { get; set; }
        public int lineaID { get; set; }
        public string linea { get; set; }
        public string slug_linea { get; set; }

        public DateTime fecha_creado_linea { get; set; }

        public string Slug_Marca { get; set; }



        public string thumbnail { get; set; }
        public string thumbnailback { get; set; }


        public int empaqueId { get; set; }
        public string empaque { get; set; }
        public float Contenido { get; set; }

        public bool Sincronizado { get; set; }

        public string CodigoEmpaque { get; set; }
    }

    }
