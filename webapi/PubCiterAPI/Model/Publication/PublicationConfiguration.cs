namespace PubCiterAPI.Model
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// PublicationConfiguration class
    /// </summary>
    public class PublicationConfiguration : IEntityTypeConfiguration<Publication>
    {
        /// <summary>
        /// Configures model
        /// </summary>
        /// <param name="builder">EntityTypeBuilder instance</param>
        public void Configure(EntityTypeBuilder<Publication> builder)
        {
            builder.HasKey(prop => prop.PublicationId);

            builder.Property(prop => prop.PublicationId)
                .IsRequired();

            builder.Property(prop => prop.AuthorId)
                .IsRequired();

            builder.Property(prop => prop.PublicationYear)
                .HasMaxLength(255);

            builder.Property(prop => prop.Authors)
                .HasMaxLength(255);

            builder.Property(prop => prop.Journal)
                .HasMaxLength(255);

            builder.Property(prop => prop.JournalVolume)
                .HasMaxLength(255);

            builder.Property(prop => prop.Pages)
                .HasMaxLength(20);

            builder.Property(prop => prop.Publisher)
                .HasMaxLength(255);

            builder.Property(prop => prop.Doi)
               .HasMaxLength(50);

            builder.Property(prop => prop.CitesPerYear)
               .HasMaxLength(255);

            builder.Property(prop => prop.EprintUrl)
               .HasMaxLength(1000);
        }
    }
}
