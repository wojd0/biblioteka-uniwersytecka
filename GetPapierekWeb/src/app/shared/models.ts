export interface Book {
  bookId: number;
  title: string;
  author: string;
  publicationYear: number;
  shelf: string;
  categoryId: number;
  category?: { id: number; name: string };
}
