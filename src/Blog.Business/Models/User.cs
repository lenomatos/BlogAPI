using System.Collections.Generic;

namespace Blog.Business.Models
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        /* EF Relations */
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
