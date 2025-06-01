namespace GetPapierek.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Shelf { get; set; }
        public int? PublicationYear { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
