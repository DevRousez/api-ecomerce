using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
	public class Accounts
	{
        [Key] //Indica que es una llave primaria, cuando el nombre del campo o llave primaria no contiene "Id" al final de la palabra
        public int Id { get; set; }
        public int AccountTypeId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        public string Email { get; set; }

        public string Password { get; set; }

        public string? GoogleId { get; set; }
        public string? Picture { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [ForeignKey("AccountTypeId")]
        public AccountsTypes AccountsTypes { get; set; }


        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

