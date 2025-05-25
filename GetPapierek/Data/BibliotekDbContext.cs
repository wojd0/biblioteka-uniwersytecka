using GetPapierek.Models;
using Microsoft.EntityFrameworkCore;

namespace GetPapierek.Data
{
    public class BibliotekDbContext : DbContext
    {
        public BibliotekDbContext(DbContextOptions<BibliotekDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Ksiazki { get; set; }
        public DbSet<Category> KategorieKsiazek { get; set; }
        public DbSet<User> Uzytkownicy { get; set; }
        public DbSet<Rental> Wypozyczenia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
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

            // Seed some initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { IdKategorii = 1, NazwaKategorii = "Powieść" },
                new Category { IdKategorii = 2, NazwaKategorii = "Nauka" },
                new Category { IdKategorii = 3, NazwaKategorii = "Historia" },
                new Category { IdKategorii = 4, NazwaKategorii = "Fantastyka" }
            );

            // Seed books
            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "Pan Tadeusz", Author = "Adam Mickiewicz", PublicationYear = 1834, CategoryId = 1, Shelf = "A1" },
                new Book { BookId = 2, Title = "Lalka", Author = "Bolesław Prus", PublicationYear = 1890, CategoryId = 1, Shelf = "A1" },
                new Book { BookId = 3, Title = "Krótka historia czasu", Author = "Stephen Hawking", PublicationYear = 1988, CategoryId = 2, Shelf = "B2" },
                new Book { BookId = 4, Title = "Dziady", Author = "Adam Mickiewicz", PublicationYear = 1822, CategoryId = 1, Shelf = "A1" }
            );

            // Seed users
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@example.com", Password = "hashed_password" },
                new User { UserId = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@example.com", Password = "hashed_password" }
            );
        }
    }
}
