namespace PubCiterAPI.Model
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// PublicationCategoryConfiguration class
    /// </summary>
    public class PublicationCategoryConfiguration : IEntityTypeConfiguration<PublicationCategory>
    {
        /// <summary>
        /// Configures model
        /// </summary>
        /// <param name="builder">EntityTypeBuilder instance</param>
        public void Configure(EntityTypeBuilder<PublicationCategory> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Id)
                .IsRequired();

            builder.Property(prop => prop.Code)
               .IsRequired();

            builder.Property(prop => prop.Group)
               .IsRequired();

            builder.Property(prop => prop.Name)
               .IsRequired();

            builder.Property(prop => prop.Code)
                .HasMaxLength(30);

            builder.Property(prop => prop.Group)
                .HasMaxLength(30);

            builder.Property(prop => prop.Name)
                .HasMaxLength(500);
        }
    }
}
