using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
            Blogs = new HashSet<Blog>();
            Comments = new HashSet<Comment>();
            Orders = new HashSet<Order>();
            Ratings = new HashSet<Rating>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool? Status { get; set; }
        public string Role { get; set; }
        public int? WishlistId { get; set; }
        public int? CartId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual WishList Wishlist { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
