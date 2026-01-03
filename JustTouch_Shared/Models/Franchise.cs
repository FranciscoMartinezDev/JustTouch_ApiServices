using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace JustTouch_Shared.Models
{
    [Table("Franchise")]
    public class Franchise : BaseModel
    {
        [PrimaryKey("IdFranchise")]
        public int IdFranchise { get; set; }

        [Column("FanstasyName")]
        public string FanstasyName { get; set; } = string.Empty;

        [Column("CompanyName")]
        public string CompanyName { get; set; } = string.Empty;

        [Column("TaxId")]
        public string TaxId { get; set; } = string.Empty;

        [Column("FanstasyName")]
        public string TaxCategory { get; set; } = string.Empty;

        [Column("CreatedDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [Column("FranchiseCode")]
        public string FranchiseCode { get; set; } = string.Empty;
        
        [Column("UserId")]
        public int UserId { get; set; }

        [Reference(typeof(Users),ReferenceAttribute.JoinType.Inner, includeInQuery: true, columnName: "UserId", foreignKey: "IdUser")]
        public Users? User { get; set; }

        [Reference(typeof(Branch), ReferenceAttribute.JoinType.Inner, includeInQuery: true, columnName: "IdFranchise", foreignKey: "FranchiseId")]
        public List<Branch>? Branches { get; set; } 

    }
}
