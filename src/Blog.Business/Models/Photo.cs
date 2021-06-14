using System;

namespace Blog.Business.Models
{
    public class Photo : Entity
    {
        public Guid AlbumId { get; set; }
        public string Imagem { get; set; }

        /* EF Relations */
        public Album Album { get; set; }
    }
}
