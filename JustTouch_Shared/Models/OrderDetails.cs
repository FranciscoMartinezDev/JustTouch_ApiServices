using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace JustTouch_Shared.Models
{
    [Table("OrderDetails")]
    public class OrderDetails : BaseModel
    {
        [PrimaryKey("IdOrderDetails")]
        public int IdOrderDetails { get; set; }

        [Column("ProductName")]
        public string ProductName { get; set; } = string.Empty;

        [Column("Quantity")]
        public int Quantity { get; set; }
        
        [Column("OrderId")]
        public int OrderId { get; set; }

        [Reference(typeof(Orders), ReferenceAttribute.JoinType.Inner, includeInQuery: false, columnName: "OrderId", foreignKey: "IdOrder")]
        public Orders? Orders { get; set; }

    }
}
