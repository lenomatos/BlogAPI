using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.ViewModels
{
    public class AlbumViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid UserId { get; set; }
        /* EF Relations */
        public IEnumerable<PhotoViewModel> Photos { get; set; }
        public UserViewModel User { get; set; }
    }
}
