using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Blog.Data.Respository;

namespace Blog.Data.Repositorys
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(DataContext context) : base(context) { }
    }
}
