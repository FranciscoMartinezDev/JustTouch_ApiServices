using Microsoft.AspNetCore.Http;

namespace JustTouch_Shared.Dtos
{
    public class ProductDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string price { get; set; } = string.Empty;
        public string pictureUrl { get; set; } = string.Empty;
        public string signedUrl { get; set; } = string.Empty;
        public string productCode { get; set; } = string.Empty;
        public bool isAvailable { get; set; }
        public IFormFile? image { get; set; }
    }
}
