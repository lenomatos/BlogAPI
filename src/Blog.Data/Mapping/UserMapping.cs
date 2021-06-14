using Blog.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.HasMany(f => f.Albums)
                 .WithOne(p => p.User)
                 .HasForeignKey(p => p.UserId);

            builder.HasMany(f => f.Posts)
                 .WithOne(p => p.User)
                 .HasForeignKey(p => p.UserId);

            builder.HasMany(f => f.Comments)
                 .WithOne(p => p.User)
                 .HasForeignKey(p => p.UserId);

            builder.ToTable("Users");
        }
    }
}
