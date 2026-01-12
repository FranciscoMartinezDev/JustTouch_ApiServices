using Newtonsoft.Json;

namespace JustTouch_Shared.Dtos
{
    public class CategoryDto
    {
        public string category { get; set; } = string.Empty;
        public string? categoryCode { get; set; }
        public string branchCode { get; set; } = string.Empty;
        public List<ProductDto> products { get; set; } = new();
    }
}
