using JustTouch_Shared.Models;
using Supabase.Postgrest.Attributes;

namespace JustTouch_Shared.Dtos
{
    public class FranchiseDto
    {
        public int id { get; set; }
        public string fanstasyName { get; set; } = string.Empty;
        public string companyName { get; set; } = string.Empty;
        public string taxId { get; set; } = string.Empty;
        public string taxCategory { get; set; } = string.Empty;
        public ICollection<BranchDto> branches { get; set; } = new List<BranchDto>();
    }
}
