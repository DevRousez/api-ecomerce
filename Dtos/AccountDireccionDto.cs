namespace Api_comerce.Dtos
{
    public class AccountDireccionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Calle { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
        public string Tipo { get; set; } = "Casa"; // Casa, Oficina, etc.
        public bool EsPredeterminada { get; set; } = false;
    }
}
