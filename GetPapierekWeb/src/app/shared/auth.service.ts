import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface AuthUser {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5241/api/Users/login';
  private _user = signal<AuthUser | null>(null);

  get user() {
    return this._user();
  }

  isLoggedIn() {
    return !!this._user();
  }

  login(email: string, password: string): Observable<any> {
    return this.http
      .post<{ user: AuthUser }>(this.apiUrl, { email, haslo: password })
      .pipe(
        tap((res) => {
          this._user.set(res.user);
          localStorage.setItem('user', JSON.stringify(res.user));
        })
      );
  }

  logout() {
    this._user.set(null);
    localStorage.removeItem('user');
  }

  restoreSession() {
    const user = localStorage.getItem('user');
    if (user) {
      this._user.set(JSON.parse(user));
    }
  }
}
