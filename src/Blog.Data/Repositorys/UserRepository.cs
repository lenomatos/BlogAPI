using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Blog.Data.Respository;

namespace Blog.Data.Repositorys
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }
    }
}