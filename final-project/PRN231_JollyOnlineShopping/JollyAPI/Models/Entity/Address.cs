using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
