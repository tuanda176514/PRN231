using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Rating
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? Quantity { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
