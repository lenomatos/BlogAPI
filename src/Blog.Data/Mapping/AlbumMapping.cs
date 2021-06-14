using Blog.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mapping
{
    public class AlbumMapping : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(f => f.Photos)
                 .WithOne(p => p.Album)
                 .HasForeignKey(p => p.AlbumId);

            builder.ToTable("Album");
        }
    }
}


