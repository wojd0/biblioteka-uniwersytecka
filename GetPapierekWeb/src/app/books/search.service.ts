import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from './books.service';

export interface SearchResult {
  query: string;
  totalCount: number;
  groupedResults: Array<{
    category: string;
    books: Book[];
  }>;
}

@Injectable({ providedIn: 'root' })
export class SearchService {
  private http = inject(HttpClient);

  private apiUrl = 'http://localhost:5241/api/Search';

  search(query: string): Observable<SearchResult> {
    return this.http.get<SearchResult>(
      `${this.apiUrl}?query=${encodeURIComponent(query)}`
    );
  }

  advancedSearch(params: {
    title?: string;
    author?: string;
    categoryId?: number;
    yearFrom?: number;
    yearTo?: number;
  }): Observable<{ totalCount: number; books: Book[] }> {
    const queryParams = Object.entries(params)
      .filter(([_, v]) => v !== undefined && v !== null && v !== '')
      .map(([k, v]) => `${k}=${encodeURIComponent(v as string)}`)
      .join('&');
    return this.http.get<{ totalCount: number; books: Book[] }>(
      `${this.apiUrl}/advanced?${queryParams}`
    );
  }
}
