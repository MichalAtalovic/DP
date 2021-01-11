namespace PubCiterAPI.Model
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// AuthorConfiguration class
    /// </summary>
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        /// <summary>
        /// Configures model
        /// </summary>
        /// <param name="builder">EntityTypeBuilder instance</param>
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(prop => prop.AuthorId);

            builder.Property(prop => prop.AuthorId)
                .IsRequired();

            builder.Property(prop => prop.ScholarId)
                .HasMaxLength(20);

            builder.Property(prop => prop.Name)
                .HasMaxLength(100);

            builder.Property(prop => prop.UrlPicture)
                .HasMaxLength(500);

            builder.Property(prop => prop.Affiliation)
                .HasMaxLength(500);
        }
    }
}
