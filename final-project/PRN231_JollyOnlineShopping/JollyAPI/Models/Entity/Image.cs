using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? ProductId { get; set; }
        public int? ColorId { get; set; }

        public virtual Color Color { get; set; }
        public virtual Product Product { get; set; }
    }
}
