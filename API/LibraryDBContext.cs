namespace API
{
    using API.Models;
    using Microsoft.EntityFrameworkCore;

    public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
    {

        // Tables
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }


}
