using TraceX.Domain.Common;

namespace TraceX.Domain.Entities
{
    public class Product : BaseEntity
    {
        //public int Id { get; set; }

        public string PartNumber { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        //public bool IsActive { get; set; } = true ;

    }
}
