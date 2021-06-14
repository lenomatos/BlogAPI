using Blog.Business.Interfaces;
using Blog.Business.Models;
using Blog.Data.Context;
using Blog.Data.Respository;

namespace Blog.Data.Repositorys
{
    public class AlbumRepository : Repository<Album>, IAlbumRepository
    {
        public AlbumRepository(DataContext context) : base(context) { }
    }
}
