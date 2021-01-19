namespace PubCiterAPI.Model
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// AdvertisementDbContext class
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Authors collection
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Publications collection
        /// </summary>
        public DbSet<Publication> Publications { get; set; }

        /// <summary>
        /// Quarantined publications collection
        /// </summary>
        public DbSet<QuarantinedPublication> QuarantinedPublications { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">DB context options</param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { 
        }

        /// <summary>
        /// On model creating method
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasOne(x => x.Settings);

            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new PublicationConfiguration());
            modelBuilder.ApplyConfiguration(new CitationConfiguration());
            modelBuilder.ApplyConfiguration(new QuarantinedPublicationConfiguration());

            modelBuilder.Entity<PublicationCitation>()
                .HasKey(a => new { a.PublicationId, a.CitationId });

            modelBuilder.Entity<PublicationCitation>()
                .HasOne(e => e.Publication)
                .WithMany(p => p.PublicationCitationList);

            modelBuilder.Entity<QuarantinedPublicationCitation>()
                .HasKey(a => new { a.QuarantinedPublicationId, a.CitationId });

            modelBuilder.Entity<QuarantinedPublicationCitation>()
                .HasOne(e => e.QuarantinedPublication)
                .WithMany(p => p.QuarantinedPublicationCitationList);
        }
    }
}
