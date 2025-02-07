using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Size
    {
        public Size()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
