using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }

        public string ? Size { get; set; }
        public string ? Color { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
