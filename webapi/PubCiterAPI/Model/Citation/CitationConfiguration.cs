namespace PubCiterAPI.Model
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// CitationConfiguration class
    /// </summary>
    public class CitationConfiguration : IEntityTypeConfiguration<Citation>
    {
        /// <summary>
        /// Configures model
        /// </summary>
        /// <param name="builder">EntityTypeBuilder instance</param>
        public void Configure(EntityTypeBuilder<Citation> builder)
        {
            builder.HasKey(prop => prop.CitationId);

            builder.Property(prop => prop.CitationId)
                .IsRequired();

            builder.Property(prop => prop.Title)
                .HasMaxLength(255);

            builder.Property(prop => prop.Author)
                .HasMaxLength(255);

            builder.Property(prop => prop.PublicationYear)
                .HasMaxLength(255);

            builder.Property(prop => prop.Journal)
                .HasMaxLength(255);

            builder.Property(prop => prop.Doi)
                .HasMaxLength(50);
        }
    }
}
