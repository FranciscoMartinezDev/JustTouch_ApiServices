namespace JustTouch_Shared.Dtos
{
    public class PublicMenuDto
    {
        public BranchDto? branch { get; set; }
        public List<CatalogDto> catalogs { get; set; } = new();
    }
}
