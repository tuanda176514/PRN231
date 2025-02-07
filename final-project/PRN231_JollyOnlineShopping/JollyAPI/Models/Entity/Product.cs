using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Product
    {
        public Product()
        {
            Images = new HashSet<Image>();
            Items = new HashSet<Item>();
            OrderDetails = new HashSet<OrderDetail>();
            Ratings = new HashSet<Rating>();
            WishItems = new HashSet<WishItem>();
            Colors = new HashSet<Color>();
            Sizes = new HashSet<Size>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string Gender { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Image>? Images { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<WishItem> WishItems { get; set; }

        public virtual ICollection<Color> Colors { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
    }
}
