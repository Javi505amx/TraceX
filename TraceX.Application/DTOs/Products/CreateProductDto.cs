namespace TraceX.Application.DTOs.Products
{
    public class CreateProductDto
    {

        public required string PartNumber { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
