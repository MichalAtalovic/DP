namespace PubCiterAPI.Data
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using PubCiterAPI.Model;

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
        /// Citations collection
        /// </summary>
        public DbSet<Citation> Citations { get; set; }

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
            /*modelBuilder.Entity<Advertisement>()
                .HasOne(x => x.ItemType);

            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new AdvertisementConfiguration());

            modelBuilder.Entity<AdvertisementTag>()
                .HasKey(a => new { a.AdvertisementId, a.TagId });

            modelBuilder.Entity<AdvertisementTag>()
                .HasOne<Advertisement>(e => e.Advertisement)
                .WithMany(p => p.AdvertisementTags);*/
        }
    }
}
