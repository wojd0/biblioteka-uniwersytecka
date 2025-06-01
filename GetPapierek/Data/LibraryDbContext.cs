using GetPapierek.Models;
using Microsoft.EntityFrameworkCore;

namespace GetPapierek.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(k => k.Category)
                .WithMany()
                .HasForeignKey(k => k.CategoryId);

            modelBuilder.Entity<Rental>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<Rental>()
                .HasOne(w => w.Book)
                .WithMany()
                .HasForeignKey(w => w.BookId);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Novel" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "History" },
                new Category { Id = 4, Name = "Fantasy" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Pan Tadeusz", Author = "Adam Mickiewicz", PublicationYear = 1834, CategoryId = 1, Shelf = "A1" },
                new Book { BookId = 2, Title = "Lalka", Author = "Bolesław Prus", PublicationYear = 1890, CategoryId = 1, Shelf = "A1" },
                new Book { BookId = 3, Title = "A Brief History of Time", Author = "Stephen Hawking", PublicationYear = 1988, CategoryId = 2, Shelf = "B2" },
                new Book { BookId = 4, Title = "Dziady", Author = "Adam Mickiewicz", PublicationYear = 1822, CategoryId = 1, Shelf = "A1" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FirstName = "John", LastName = "Smith", Email = "john.smith@example.com", Password = "hashed_password" },
                new User { UserId = 2, FirstName = "Anna", LastName = "Johnson", Email = "anna.johnson@example.com", Password = "hashed_password" }
            );
        }
    }
}
