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
                .HasOne(k => k.Kategoria)
                .WithMany()
                .HasForeignKey(k => k.IdKategorii);

            modelBuilder.Entity<Rental>()
                .HasOne(w => w.Uzytkownik)
                .WithMany()
                .HasForeignKey(w => w.IdUzytkownika);

            modelBuilder.Entity<Rental>()
                .HasOne(w => w.Ksiazka)
                .WithMany()
                .HasForeignKey(w => w.IdKsiazki);

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
                new Book { IdKsiazki = 1, Tytul = "Pan Tadeusz", Autor = "Adam Mickiewicz", RokWydania = 1834, IdKategorii = 1, Pulka = "A1" },
                new Book { IdKsiazki = 2, Tytul = "Lalka", Autor = "Bolesław Prus", RokWydania = 1890, IdKategorii = 1, Pulka = "A1" },
                new Book { IdKsiazki = 3, Tytul = "Krótka historia czasu", Autor = "Stephen Hawking", RokWydania = 1988, IdKategorii = 2, Pulka = "B2" },
                new Book { IdKsiazki = 4, Tytul = "Dziady", Autor = "Adam Mickiewicz", RokWydania = 1822, IdKategorii = 1, Pulka = "A1" }
            );

            // Seed users
            modelBuilder.Entity<User>().HasData(
                new User { IdUzytkownika = 1, Imie = "Jan", Nazwisko = "Kowalski", Email = "jan.kowalski@example.com", Haslo = "hashed_password" },
                new User { IdUzytkownika = 2, Imie = "Anna", Nazwisko = "Nowak", Email = "anna.nowak@example.com", Haslo = "hashed_password" }
            );
        }
    }
}
