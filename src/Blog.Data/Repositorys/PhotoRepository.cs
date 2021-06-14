using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Blog.Data.Respository;

namespace Blog.Data.Repositorys
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DataContext context) : base(context) { }
    }
}