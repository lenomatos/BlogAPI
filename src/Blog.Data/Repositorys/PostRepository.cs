using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Blog.Data.Respository;

namespace Blog.Data.Repositorys
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DataContext context) : base(context) { }
    }
}