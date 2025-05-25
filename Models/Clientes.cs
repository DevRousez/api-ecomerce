using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models
{
    public class Clientes{
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string RazonSocial { get; set; }

        [StringLength(16)]
        public string Rfc { get; set; }
        public string Calle { get; set; }
        public string Exterior { get; set; }
        public string Interior { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string CodPostal { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 

        [ForeignKey("AccountId")]
        public virtual Accounts? Accounts { get; set; }
    }
}