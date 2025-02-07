using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class WishList
    {
        public WishList()
        {
            WishItems = new HashSet<WishItem>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<WishItem> WishItems { get; set; }
    }
}
