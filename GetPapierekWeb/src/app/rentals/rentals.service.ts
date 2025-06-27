import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Rental {
  rentalId: number;
  bookId: number;
  userId: number;
  rentalDate: string;
  returnDate: string | null;
  status: number; // 0 = Rented, 1 = Returned
  book?: {
    title: string;
    author: string;
    bookId: number;
    shelf: string;
    publicationYear: number;
    categoryId: number;
    category: { id: number; name: string };
  };
  user?: {
    userId: number;
    firstName: string;
    lastName: string;
    email: string;
  };
}

@Injectable({ providedIn: 'root' })
export class RentalsService {
  private http = inject(HttpClient);

  private apiUrl = 'http://localhost:5241/api/Rental';

  getRentals(): Observable<Rental[]> {
    return this.http.get<Rental[]>(this.apiUrl);
  }

  createRental(bookId: number, userId: number): Observable<Rental> {
    const rental = {
      bookId,
      userId,
      rentalDate: new Date().toISOString(),
      status: 0, // 0 = Rented
    };
    return this.http.post<Rental>(this.apiUrl, rental);
  }

  makeReturn(rentalId: number): Observable<unknown> {
    return this.http.post<string>(`${this.apiUrl}/${rentalId}/zwrot`, undefined);
  }
}
