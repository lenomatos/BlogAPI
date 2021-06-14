using Blog.Business.Interfaces;
using Blog.Data.Context;
using Blog.Data.Repositorys;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Api.Configuration
{
    public static class DependecyInjectionConfig
    {
        public static IServiceCollection AddDenpendencyConfig(this IServiceCollection services)
        {
            services.AddScoped<DataContext>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
