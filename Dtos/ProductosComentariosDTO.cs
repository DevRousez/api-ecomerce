using Api_comerce.Models;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Dtos
{
    public class ProductosComentariosDTO
    {
       
           
            public int Id { get; set; }

            public int ProductoId { get; set; }
            public Productos Producto { get; set; }

            public int AccountId { get; set; }
            public Accounts Account { get; set; }

            public string Comentario { get; set; }
            public int Calificacion { get; set; } 
            public DateTime FechaCreado { get; set; } = DateTime.Now;
        
    }
}
