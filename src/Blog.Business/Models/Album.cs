using System;
using System.Collections.Generic;

namespace Blog.Business.Models
{
    public class Album : Entity
    {
        public Guid UserId { get; set; }
        /* EF Relations */
        public IEnumerable<Photo> Photos { get; set; }
        public User User { get; set; }
    }
}
