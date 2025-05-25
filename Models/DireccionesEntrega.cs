using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_comerce.Models{
    public class DireccionesEntrega{

        [Key]
        public int Id {get;set;}
        public int AccountId { get; set; }
        public string Etiqueta {get;set;} // Ej. Casa de mamá, Sucursal Tabasco
        public string Calle { get; set; }
        public string Exterior { get; set; }
        public string Interior { get; set; }
        public string Colonia { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string CodPostal { get; set; } 

        public string NombreDestinatario { get; set; } 
        public string Telefono { get; set; } 

        //Datos opcionales
        public string ? Edificio {get;set;}
        public string ? Apartamento {get;set;}
        public string ? OtrasReferencias {get;set;}

        [ForeignKey("AccountId")]
        public virtual Accounts? Accounts{get;set;}

    }
}