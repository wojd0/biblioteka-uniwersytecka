import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'ksiazki',
    title: 'Lista książek',
    loadComponent: () =>
      import('./books/list/books-list.component').then(
        (m) => m.BooksListComponent
      ),
  },
  {
    path: 'wypozyczenia',
    title: 'Lista wypożyczeń',
    loadComponent: () =>
      import('./rentals/list/rentals-list.component').then(
        (m) => m.RentalsListComponent
      ),
  },
  {
    path: 'uzytkownicy',
    title: 'Lista użytkowników',
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
  { path: '**', redirectTo: '' },
];
