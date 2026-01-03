using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace JustTouch_Shared.Models
{
    [Table("Orders")]
    public class Orders : BaseModel
    {
        [PrimaryKey("IdOrder")]
        public int IdOrder { get; set; }

        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Column("OrderGroupId ")]
        public int OrderGroupId { get; set; }

        [Reference(typeof(OrderGroup), ReferenceAttribute.JoinType.Inner, includeInQuery: false, columnName: "OrderGroupId", foreignKey: "IdOrderGroup")]
        public virtual OrderGroup? OrderGroup { get; set; }

        [Reference(typeof(OrderDetails), ReferenceAttribute.JoinType.Inner, includeInQuery: true, columnName: "IdOrder", foreignKey: "OrderId")]
        public List<OrderDetails> OrderDetails { get; set; } = new(); 
    }
}
