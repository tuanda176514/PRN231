using System;
using System.Collections.Generic;

namespace JollyAPI.Models.Entity
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? UserId { get; set; }
        public int? BlogId { get; set; }
		public DateTime? Date { get; set; }
		public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }
    }
}
