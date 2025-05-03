namespace Biblioteka.Modele
{
    public class Wypozyczenie
    {
        public int IdWypozyczenia { get; set; }

        public int IdUzytkownika { get; set; }
        public Uzytkownik Uzytkownik { get; set; }

        public int IdKsiazki { get; set; }
        public Ksiazka Ksiazka { get; set; }

        public DateTime DataWypozyczenia { get; set; }
        public DateTime? DataZwrotu { get; set; }

        public StatusWypozyczenia Status { get; set; }
    }
}
