using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.ViewModels
{

    public class UserTokenViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
