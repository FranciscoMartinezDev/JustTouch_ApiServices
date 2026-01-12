namespace JustTouch_Shared.Dtos
{
    public class PublicMenuDto
    {
        public BranchDto? branch { get; set; }
        public List<CategoryDto> catalogs { get; set; } = new();
    }
}
