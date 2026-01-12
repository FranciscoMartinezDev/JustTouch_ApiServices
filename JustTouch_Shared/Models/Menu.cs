using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace JustTouch_Shared.Models
{
    [Table("Menu")]
    public class Menu : BaseModel
    {
        [PrimaryKey("IdMenu")]
        public int IdMenu { get; set; }

        [Column("Category")]
        public string Category { get; set; } = string.Empty;

        [Column("CategoryCode")]
        public string CategoryCode { get; set; } = string.Empty;

        [Column("BranchCode")]
        public string BranchCode { get; set; } = string.Empty;

        [Column("BranchId")]
        public int BranchId { get; set; }

        [Reference(typeof(Branch), ReferenceAttribute.JoinType.Inner, includeInQuery: false)]
        public Branch? Branch { get; set; }


        [Reference(typeof(Product), ReferenceAttribute.JoinType.Inner, includeInQuery: false)]
        public List<Product>? Products { get; set; }
    }
}
