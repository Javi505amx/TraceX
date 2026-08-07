namespace TraceX.Application.DTOs.Products
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string PartNumber { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
