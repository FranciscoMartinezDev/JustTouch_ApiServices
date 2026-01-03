namespace JustTouch_Shared.Dtos
{
    public class OrderGroupDto
    {
        public string groupCode { get; set; } = string.Empty;
        public string branchCode { get; set; } = string.Empty;
        public string delivery { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public decimal tip { get; set; }
        public int state { get; set; }
        public List<OrderDto> orders { get; set; } = new();
    }
}
