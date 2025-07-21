namespace Api_comerce.Dtos
{
    public class LineaDto
    {
        public int Id { get; set; }
        public string? Linea { get; set; }
        public string? Slug { get; set; }
        public DateTime? FechaCreado { get; set; }

        public List<ProductoEmpaqueDto> ProductoEmpaque { get; set; } = new();
    }



}
