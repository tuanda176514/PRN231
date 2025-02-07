using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Blog
    {
        public Blog()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string Content { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Image { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
