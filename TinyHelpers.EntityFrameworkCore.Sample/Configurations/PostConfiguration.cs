using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyHelpers.EntityFrameworkCore.Extensions;
using TinyHelpers.EntityFrameworkCore.Sample.Models;

namespace TinyHelpers.EntityFrameworkCore.Sample.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Title).HasMaxLength(80).IsRequired();
            builder.Property(x => x.Content).IsRequired();

            builder.Property(x => x.Authors).HasArrayConversion().IsRequired();
            builder.Property(x => x.Reviews).HasJsonConversion();
        }
    }
}
