using Api_comerce.Models;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Dtos
{
    public class ProductosComentariosDTO
    {
           
            public int Id { get; set; }
        public bool canEdit { get; set; }   = false;
        public string Comentario { get; set; }
        public int Calificacion { get; set; }
        public DateTime FechaCreado { get; set; } = DateTime.Now;
        public int AccountId { get; set; }
            public AccountDto Account { get; set; }
        public ProductoEmpaqueDto ProductoEmpaque { get; set; }
       
        
    }
    public class ProductoConComentariosDTO
    {
        public int ProductoEmpaqueId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;

        public int ProductoId { get; set; } 

        public List<ProductosComentariosDTO> Comentarios { get; set; } = new();
    }

    public class CrearComentarioDTO
    {
        public int ProductoId { get; set; }
        public int AccountId { get; set; }
        public string Comentario { get; set; }
        public int Calificacion { get; set; }
    }
}
