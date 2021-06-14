using Blog.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mapping
{
    public class PostMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PostContent)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.HasMany(f => f.Comments)
                 .WithOne(p => p.Post)
                 .HasForeignKey(p => p.PostId);

            builder.ToTable("Post");
        }
    }
}

