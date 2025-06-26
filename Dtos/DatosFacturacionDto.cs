namespace Api_comerce.Dtos
{
    public class DatosFacturacionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string RazonSocial { get; set; }
        public string RFC { get; set; }
        public string UsoCFDI { get; set; }
        public string RegimenFiscal { get; set; }
        public string Correo { get; set; }
    }
}
