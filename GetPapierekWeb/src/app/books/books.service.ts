import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {Book} from '../shared/models';

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

  addBook(book: Partial<Book>): Observable<Book> {
    return this.http.post<Book>(this.apiUrl, book);
  }
}
