using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Item
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? CartId { get; set; }
        public int? Quantity { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }

	}
}
