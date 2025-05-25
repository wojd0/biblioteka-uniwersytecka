namespace GetPapierek.Models
{
    public class Book
    {
        public int IdKsiazki { get; set; }
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public string Pulka { get; set; }
        public int? RokWydania { get; set; }

        public int? IdKategorii { get; set; }
        public Category Kategoria { get; set; }
    }
}
