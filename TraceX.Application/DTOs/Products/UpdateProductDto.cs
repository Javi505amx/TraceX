namespace TraceX.Application.DTOs.Products
{
    public class UpdateProductDto
    {

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
