using JustTouch_Shared.Enums;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace JustTouch_Shared.Models
{
    public class OrderGroup : BaseModel
    {
        [Column("IdOrderGroup")]
        public int IdOrderGroup { get; set; }

        [Column("CreatedDate")]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        [Column("CanceledDate")]
        public DateTimeOffset? CanceledDate { get; set; } = null;

        [Column("ClosedDate")]
        public DateTimeOffset? ClosedDate { get; set; } = null;

        [Column("Delivery")]
        public string Delivery { get; set; } = string.Empty;

        [Column("Subtotal")]
        public decimal Subtotal { get; set; }

        [Column("Total")]
        public decimal Total { get; set; }

        [Column("Tip")]
        public decimal Tip { get; set; }
        
        [Column("Phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("BranchCode")]
        public string BranchCode { get; set; } = string.Empty;
        
        [Column("GroupCode")]
        public string GroupCode { get; set; } = string.Empty;

        [Column("State")]
        public int State { get; set; }

        [Reference(typeof(Branch), ReferenceAttribute.JoinType.Inner, includeInQuery: true, columnName: "IdOrderGroup", foreignKey: "OrderGroupId")]
        public List<Orders>? Orders { get; set; }
    }
}
