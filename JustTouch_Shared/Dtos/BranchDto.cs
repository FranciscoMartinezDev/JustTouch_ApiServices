using JustTouch_Shared.Models;
using Supabase.Postgrest.Attributes;

namespace JustTouch_Shared.Dtos
{
    public class BranchDto
    {
        public int id { get; set; }
        public string country { get; set; } = string.Empty;
        public string provinceOrState { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string postalCode { get; set; } = string.Empty;
        public string? email { get; set; }
        public TimeOnly? openTime { get; set; }
        public TimeOnly? closeTime { get; set; }
        public string? pictureUrl { get; set; }
        public string? signelUrl { get; set; }
        public string? instagramUrl { get; set; }
        public string? facebookUrl { get; set; }
        public string? whatsappUrl { get; set; }
        public string branchCode { get; set; } = string.Empty;
    }
}
