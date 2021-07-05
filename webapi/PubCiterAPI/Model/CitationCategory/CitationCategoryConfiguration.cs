namespace PubCiterAPI.Model
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// CitationCategoryConfiguration class
    /// </summary>
    public class CitationCategoryConfiguration : IEntityTypeConfiguration<CitationCategory>
    {
        /// <summary>
        /// Configures model
        /// </summary>
        /// <param name="builder">EntityTypeBuilder instance</param>
        public void Configure(EntityTypeBuilder<CitationCategory> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Id)
                .IsRequired();

            builder.Property(prop => prop.Code)
               .IsRequired();

            builder.Property(prop => prop.Name)
               .IsRequired();

            builder.Property(prop => prop.Name)
                .HasMaxLength(500);
        }
    }
}
