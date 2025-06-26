namespace Api_comerce.Models
{
    public class DatosFacturacion
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public virtual Accounts Account { get; set; }

        public string RazonSocial { get; set; }
        public string RFC { get; set; }
        public string RegimenFiscal { get; set; }
        public string MetodoPago { get; set; }
        public string UsoCFDI { get; set; }
        public string Calle { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
