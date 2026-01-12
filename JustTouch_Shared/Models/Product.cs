using Microsoft.AspNetCore.Http;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace JustTouch_Shared.Models
{
    [Table("Product")]
    public class Product : BaseModel
    {
        [PrimaryKey("IdProduct")]
        public int IdProduct { get; set; }

        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Column("Price")]
        public decimal Price { get; set; }

        [Column("PictureUrl")]
        public string PictureUrl { get; set; } = string.Empty;

        [Column("ProductCode")]
        public string ProductCode { get; set; } = string.Empty;

        [Column("IsAvailable")]
        public bool IsAvailable { get; set; }

        [Column("MenuId")]
        public int MenuId { get; set; }

        public IFormFile? Image {  get; set; }

        [Reference(typeof(Menu), ReferenceAttribute.JoinType.Inner, includeInQuery: true)]
        public Menu? Menu { get; set; }
    }
}
