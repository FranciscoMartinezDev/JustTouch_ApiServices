using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Security.Cryptography;

namespace JustTouch_Shared.Models
{
    [Table("Users")]
    public class Users : BaseModel
    {
        [PrimaryKey("IdUser")]
        public int IdUser { get; set; }

        [Column("AccountKey")]
        public string AccountKey { get; set; } = string.Empty;

        [Column("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [Column("LastName")]
        public string LastName { get; set; } = string.Empty;

        [Column("UserName")]
        public string UserName { get; set; } = string.Empty;

        [Column("Phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("Password")]
        public string Password { get; set; } = string.Empty;

        [Column("CreatedDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [Column("FirstLogin")]
        public DateTimeOffset FirstLogin { get; set; }
        
        [Column("IsConfirmed")]
        public bool IsConfirmed { get; set; }

        [Reference(typeof(Franchise), ReferenceAttribute.JoinType.Inner, includeInQuery: true, columnName: "IdUser", foreignKey: "UserId")]
        public List<Franchise>? Franchises { get; set; }
    }
}
