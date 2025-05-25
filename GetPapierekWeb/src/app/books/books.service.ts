import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Book {
  bookId: number;
  title: string;
  author: string;
  publicationYear: number;
  shelf: string;
  categoryId: number;
  category?: { id: number; name: string };
}

@Injectable({ providedIn: 'root' })
export class BooksService {
  private http = inject(HttpClient);

  private apiUrl = 'http://localhost:5241/api/Books';

  getBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl);
  }

  searchBooks(query: string): Observable<Book[]> {
    return this.http.get<Book[]>(
      `${this.apiUrl}/search?query=${encodeURIComponent(query)}`
    );
  }

  getBook(id: number): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }
}
