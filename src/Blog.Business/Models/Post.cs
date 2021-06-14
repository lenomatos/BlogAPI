using System;
using System.Collections.Generic;

namespace Blog.Business.Models
{
    public class Post : Entity
    {
        public Guid UserId { get; set; }
        public string PostContent { get; set; }

        /* EF Relations */
        public IEnumerable<Comment> Comments { get; set; }
        public User User { get; set; }
    }
}
