using JustTouch_Shared.Enums;
using JustTouch_Shared.Models;
using Supabase.Postgrest.Attributes;

namespace JustTouch_Shared.Dtos
{
    public class OrderDto
    {
        public string description { get; set; } = string.Empty;
        public List<OrderDetailDto> details { get; set; } = new();
    }
}
