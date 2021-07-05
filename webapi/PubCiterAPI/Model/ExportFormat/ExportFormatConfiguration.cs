namespace PubCiterAPI.Model
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// ExportFormatConfiguration class
    /// </summary>
    public class ExportFormatConfiguration : IEntityTypeConfiguration<ExportFormat>
    {
        /// <summary>
        /// Configures model
        /// </summary>
        /// <param name="builder">EntityTypeBuilder instance</param>
        public void Configure(EntityTypeBuilder<ExportFormat> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Id)
                .IsRequired();

            builder.Property(prop => prop.Code)
               .IsRequired();

            builder.Property(prop => prop.Template)
               .IsRequired();

            builder.Property(prop => prop.Code)
                .HasMaxLength(30);

            builder.Property(prop => prop.Template)
                .HasMaxLength(500);
        }
    }
}
