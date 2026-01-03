using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace JustTouch_Shared.Models
{
    [Table("Branch")]
    public class Branch : BaseModel
    {
        [PrimaryKey("IdBranch")]
        public int IdBranch { get; set; }

        [Column("Country")]
        public string Country { get; set; } = string.Empty;

        [Column("ProvinceOrState")]
        public string ProvinceOrState { get; set; } = string.Empty;

        [Column("City")]
        public string City { get; set; } = string.Empty;

        [Column("Address")]
        public string Address { get; set; } = string.Empty;

        [Column("PostalCode")]
        public string PostalCode { get; set; } = string.Empty;

        [Column("Email")]
        public string? Email { get; set; }

        [Column("OpenTime")]
        public TimeOnly? OpenTime { get; set; }

        [Column("CloseTime")]
        public TimeOnly? CloseTime { get; set; }

        [Column("CreatedDate")]
        public DateTimeOffset? CreatedDate { get; set; }

        [Column("PictureUrl")]
        public string? PictureUrl { get; set; }

        [Column("InstagramUrl")]
        public string? InstagramUrl { get; set; }

        [Column("FacebookUrl")]
        public string? FacebookUrl { get; set; }

        [Column("WhatsappUrl")]
        public string? WhatsappUrl { get; set; }

        [Column("BranchCode")]
        public string BranchCode { get; set; } = string.Empty;

        [Column("FranchiseId")]
        public int FranchiseId { get; set; }

        [Reference(typeof(Franchise), ReferenceAttribute.JoinType.Inner, includeInQuery: false, foreignKey: "IdFranchise")]
        public Franchise? Franchise { get; set; }

        [Reference(typeof(Menu), ReferenceAttribute.JoinType.Inner, includeInQuery: false)]
        public List<Menu>? Menus { get; set; }

        [Reference(typeof(Orders), ReferenceAttribute.JoinType.Inner, includeInQuery: false, foreignKey: "BranchId")]
        public List<Orders>? Orders { get; set; }
    }
}
