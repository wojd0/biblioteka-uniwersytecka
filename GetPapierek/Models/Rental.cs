namespace GetPapierek.Models
{
    public class Rental
    {
        public int IdWypozyczenia { get; set; }

        public int IdUzytkownika { get; set; }
        public User Uzytkownik { get; set; }

        public int IdKsiazki { get; set; }
        public Book Ksiazka { get; set; }

        public DateTime DataWypozyczenia { get; set; }
        public DateTime? DataZwrotu { get; set; }

        public RentalStatus Status { get; set; }
    }
}
