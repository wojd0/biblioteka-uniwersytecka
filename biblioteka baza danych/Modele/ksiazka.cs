namespace Biblioteka.Modele
{
    public class ksiazka
    {
        public int IdKsiazki { get; set; }
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public string Pulka { get; set; }
        public int? RokWydania { get; set; }

        public int? IdKategorii { get; set; }
        public KategoriaKsiazki Kategoria { get; set; }
    }
}
