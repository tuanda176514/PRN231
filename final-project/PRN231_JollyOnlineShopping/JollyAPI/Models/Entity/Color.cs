using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Color
    {
        public Color()
        {
            Images = new HashSet<Image>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Hex { get; set; }

        public virtual ICollection<Image>? Images { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
