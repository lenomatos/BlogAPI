using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.ViewModels
{
    public class UserViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} é precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} é precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Email { get; set; }
        /* EF Relations */
        public IEnumerable<AlbumViewModel> Albums { get; set; }
        public IEnumerable<PostViewModel> Posts { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
