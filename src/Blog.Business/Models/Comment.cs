using System;

namespace Blog.Business.Models
{
    public class Comment : Entity
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string CommentContent { get; set; }

        /* EF Relations */
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
