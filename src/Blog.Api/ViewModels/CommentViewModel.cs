using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.ViewModels
{
    public class CommentViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid PostId { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(250, ErrorMessage = "O campo {0} é precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string CommentContent { get; set; }

        /* EF Relations */
        public PostViewModel Post { get; set; }
        public UserViewModel User { get; set; }
    }
}
