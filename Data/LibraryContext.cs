using Microsoft.EntityFrameworkCore;
public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    public DbSet<Books> Books { get; set; }
    public DbSet<Orders> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Orders>()
            .HasOne(o => o.Book)
            .WithMany(b => b.Orders)
            .HasForeignKey(o => o.BookId);
    }
}