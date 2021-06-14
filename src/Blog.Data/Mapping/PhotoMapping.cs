using Blog.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mapping
{
    public class PhotoMapping : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Imagem)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.ToTable("Photo");
        }
    }
}


