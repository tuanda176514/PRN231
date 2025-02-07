namespace JollyAPI.Models.DTOS
{
    public class OrderDetailViewModel
    {
        public List<OrderDetailDTO> OrderDetailDTOs { get; set; }
        public decimal SubTotal { get; set; }
    }
}
