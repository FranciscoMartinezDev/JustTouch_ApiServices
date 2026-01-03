namespace JustTouch_Shared.Dtos
{
    public class CatalogDto
    {
        public string catalog { get; set; } = string.Empty;
        public string catalogCode { get; set; } = string.Empty;
        public string branchCode { get; set; } = string.Empty;
        public List<ProductDto> products { get; set; } = new();
    }
}
