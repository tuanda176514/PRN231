using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Cart
    {
        public Cart()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Item> Items { get; set; }

        
    }
}
