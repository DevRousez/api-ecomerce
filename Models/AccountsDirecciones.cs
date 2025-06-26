namespace Api_comerce.Models
{
    public class AccountsDirecciones
    {
       
            public int Id { get; set; }

            public int AccountId { get; set; }
            public virtual Accounts Account { get; set; }

            public string Calle { get; set; }
            public string Ciudad { get; set; }
            public string Estado { get; set; }
            public string CodigoPostal { get; set; }
            public string Pais { get; set; }
            public string Telefono { get; set; }
             public string Tipo { get; set; }

            public bool EsPredeterminada { get; set; } = false;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
