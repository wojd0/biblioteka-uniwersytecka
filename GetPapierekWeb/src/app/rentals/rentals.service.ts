import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Rental {
  rentalId: number;
  bookId: number;
  userId: number;
  rentalDate: string;
  returnDate: string | null;
  status: string;
  book?: { title: string; author: string };
  user?: { name: string; email: string };
}

@Injectable({ providedIn: 'root' })
export class RentalsService {
  private http = inject(HttpClient);

  private apiUrl = 'http://localhost:5241/api/Rental';

  getRentals(): Observable<Rental[]> {
    return this.http.get<Rental[]>(this.apiUrl);
  }
}
