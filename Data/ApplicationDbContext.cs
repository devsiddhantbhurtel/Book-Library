using Microsoft.EntityFrameworkCore;
using BookLibrarySystem.Models;

namespace BookLibrarySystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<StaffClaimRecord> StaffClaimRecords { get; set; }
        public DbSet<BookPublisher> BookPublishers { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<StaffNotification> StaffNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.MembershipID)
                .IsUnique();

            // Book configuration
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            // Order configuration
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // StaffClaimRecord configuration
            modelBuilder.Entity<StaffClaimRecord>()
                .HasOne(s => s.Order)
                .WithOne(o => o.StaffClaimRecord)
                .HasForeignKey<StaffClaimRecord>(s => s.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StaffClaimRecord>()
                .Property(s => s.ClaimDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Review configuration
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Book-Award relationship
            modelBuilder.Entity<Award>()
                .HasOne(a => a.Book)
                .WithMany(b => b.Awards)
                .HasForeignKey(a => a.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships
            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(br => br.Book)
                .WithMany()
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowingRecord>()
                .HasOne(br => br.Member)
                .WithMany()
                .HasForeignKey(br => br.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // CartItem configuration
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.UserID, ci.BookID });

            // Bookmark configuration
            modelBuilder.Entity<Bookmark>()
                .HasKey(b => new { b.UserID, b.BookID });

            // Review configuration
            modelBuilder.Entity<Review>()
                .Property(r => r.Rating)
                .HasAnnotation("Range", new[] { 1, 5 });

            // Discount configuration
            modelBuilder.Entity<Discount>()
                .Property(d => d.DiscountType);
            modelBuilder.Entity<Discount>()
                .Property(d => d.DiscountValue);
            modelBuilder.Entity<Discount>()
                .Property(d => d.StackingRule);

            modelBuilder.Entity<BookAuthor>()
                .ToTable("BookAuthor")
                .HasKey(ba => new { ba.BookID, ba.AuthorID });
            modelBuilder.Entity<BookGenre>()
                .ToTable("BookGenre")
                .HasKey(bg => new { bg.BookID, bg.GenreID });
            modelBuilder.Entity<BookPublisher>()
                .ToTable("BookPublisher")
                .HasKey(bp => new { bp.BookID, bp.PublisherID });

            modelBuilder.Entity<StaffNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationID);
                entity.Property(e => e.ClaimCode).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                
                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}