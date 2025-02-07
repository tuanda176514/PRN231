using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class WishItem
    {
        public int Id { get; set; }
        public int? WishlistId { get; set; }
        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual WishList Wishlist { get; set; }
    }
}
