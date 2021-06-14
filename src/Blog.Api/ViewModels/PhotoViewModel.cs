using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Api.ViewModels
{
    public class PhotoViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid AlbumId { get; set; }
        public string Imagem { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string ImagemUpload { get; set; }

        /* EF Relations */
        public AlbumViewModel Album { get; set; }
    }
}
