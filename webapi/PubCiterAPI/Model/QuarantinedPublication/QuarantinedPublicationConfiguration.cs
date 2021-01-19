namespace PubCiterAPI.Model
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// QuarantineConfiguration class
    /// </summary>
    public class QuarantinedPublicationConfiguration : IEntityTypeConfiguration<QuarantinedPublication>
    {
        /// <summary>
        /// Configures model
        /// </summary>
        /// <param name="builder">EntityTypeBuilder instance</param>
        public void Configure(EntityTypeBuilder<QuarantinedPublication> builder)
        {
            builder.HasKey(prop => prop.QuarantinedPublicationId);

            builder.Property(prop => prop.QuarantinedPublicationId)
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
        }
    }
}
