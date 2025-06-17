using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api_comerce.Dtos
{
    public class ProductoDto
    {
        public int ProductId { get; set; }
        public int? ProductoSatId { get; set; }
        public string Prefijo { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionBreve { get; set; }
        public string Slug { get; set; }
        public int? Rating { get; set; }
        public bool? Acumulador { get; set; }
        public int? ProductoIdAcumulador { get; set; }

        public LineaDto? Linea { get; set; }
        public MarcaProductoDto? MarcaProducto { get; set; }

        public ProductoSatDto? ProductoSat { get; set; }
        public UnidadSatDto? UnidadSat { get; set; }
        public string DescripcionProdSat { get; set; }

        public List<ProductoEmpaqueDto> productoEmpaques { get; set; }
    }

    

}
