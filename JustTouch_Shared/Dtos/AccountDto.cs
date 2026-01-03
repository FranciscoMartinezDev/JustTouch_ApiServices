namespace JustTouch_Shared.Dtos
{
    public class AccountDto
    {
        public UserDto? userData { get; set; }
        public ICollection<FranchiseDto> franchises { get; set; } = new List<FranchiseDto> ();
    }
}
