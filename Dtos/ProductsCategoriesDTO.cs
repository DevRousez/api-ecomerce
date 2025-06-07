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
        // Línea
        public int Id { get; set; } // lineas.Id
        public string Name_Linea { get; set; } // lineas.Linea
        public string Slug_Linea { get; set; } // lineas.Slug
        public DateTime CreatedAtLinea { get; set; } // Lineas.FechaCreado
        public DateTime UpdatedAtLinea { get; set; } // Lineas.FechaCreado

        // Producto
        public int Id_Producto { get; set; } // Productos.Id
        public int? ProductoSatId { get; set; } // Productos.ProductoSatId
        public string Prefijo { get; set; } // productos.Prefijo
        public string NombreProducto { get; set; } // ProductoEmpaque.Codigo + ' ' + productos.NombreProducto
        public double? PVenta { get; set; } // ProductoEmpaque.PVenta
        public double? PCompra { get; set; } // ProductoEmpaque.PCompra
        public double? Descuento { get; set; } // ProductoEmpaque.Descuento
        public string Descripcion { get; set; } // Productos.Descripcion
        public string DescripcionBreve { get; set; } // Productos.DescripcionBreve
        public string SlugProducto { get; set; } // Productos.Slug
        public int? Rating { get; set; } // productos.Rating
        public bool? Acumulador { get; set; } // productos.Acumulador
        public int Is_Stock { get; set; } // 0 as is_stock
        public int? ProductoIdAcumulador { get; set; } // productos.ProductoIdAcumulador
        public DateTime CreatedAtProducto { get; set; } // productos.createdat
        public DateTime UpdatedAtProducto { get; set; } // productos.updatedat

        // Size
        public int? Id_Size { get; set; } // ProductoEmpaque.id
        public string Codigo_Empaque { get; set; } // ProductoEmpaque.codigo
        public double? PCompraPE { get; set; } // ProductoEmpaque.PCompra
        public double? PVentaPE { get; set; } // ProductoEmpaque.PVenta
        public double? DescuentoPE { get; set; } // ProductoEmpaque.Descuento
        public bool? ActivoPE { get; set; } // ProductoEmpaque.Activo

        // Colores (placeholder, ya que lo defines como string fijo)
        public string Colors { get; set; } // 'azul' as colors

        // Marca (Badge)
        public int Id_Marca { get; set; } // MarcasProducto.Id
        public string Marca { get; set; } // MarcasProducto.Marca
        public string SlugMarca { get; set; } // MarcasProducto.Slug

        public int? id_imagen { get; set; }
        public string nameImagen { get; set; }
        public string url_imagen { get; set; }

        public bool? EsImagenPrincipal { get; set; } = false;
    }
}
