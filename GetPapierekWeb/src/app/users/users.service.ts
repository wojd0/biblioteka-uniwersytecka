import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
}

@Injectable({ providedIn: 'root' })
export class UsersService {
  private http = inject(HttpClient);

  private apiUrl = 'http://localhost:5241/api/Users';

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl);
  }
}
