import { Routes } from '@angular/router';
import { authGuard } from './shared/auth.guard';

export const routes: Routes = [
  {
    path: 'ksiazki',
    title: 'Lista książek',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./books/list/books-list.component').then(
        (m) => m.BooksListComponent
      ),
  },
  {
    path: 'wypozyczenia',
    title: 'Lista wypożyczeń',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./rentals/list/rentals-list.component').then(
        (m) => m.RentalsListComponent
      ),
  },
  {
    path: 'uzytkownicy',
    title: 'Lista użytkowników',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./users/list/users-list.component').then(
        (m) => m.UsersListComponent
      ),
  },
  {
    path: '',
    title: 'Strona główna',
    loadComponent: () =>
      import('./home-card/home-card.component').then(
        (m) => m.HomeCardComponent
      ),
  },
  {
    path: 'login',
    title: 'Logowanie',
    loadComponent: () =>
      import('./shared/components/login-form/login-form.component').then(
        (m) => m.LoginFormComponent
      ),
  },
  { path: '**', redirectTo: '' },
];
