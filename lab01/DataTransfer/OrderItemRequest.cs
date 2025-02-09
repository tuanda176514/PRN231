using BusinessObjects;
using System.ComponentModel.DataAnnotations;

namespace DataTransfer
{
    public class OrderItemRequest
    {
        [Required]
        public int Quantity { get; set; }
    }
}
