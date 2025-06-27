import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { UsersService, User } from '../users.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box.component';

@Component({
  selector: 'app-users-list',
  standalone: true,
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatTableModule,
    MatButtonModule,
    MatSortModule,
    SearchBoxComponent,
  ],
})
export class UsersListComponent implements OnInit {
  private usersService = inject(UsersService);

  users: User[] = [];
  filteredUsers: User[] = [];
  searchTerm = '';
  dataSource = new MatTableDataSource<User>([]);
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.usersService.getUsers().subscribe((users) => {
      this.users = users;
      this.applyFilter();
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter() {
    const term = this.searchTerm.trim().toLowerCase();
    if (!term) {
      this.filteredUsers = this.users;
    } else {
      this.filteredUsers = this.users.filter(
        (u) =>
          (u.firstName && u.firstName.toLowerCase().includes(term)) ||
          (u.lastName && u.lastName.toLowerCase().includes(term)) ||
          (u.email && u.email.toLowerCase().includes(term))
      );
    }
    this.dataSource.data = this.filteredUsers;
    this.dataSource.sort = this.sort;
  }

  onSearchChange() {
    this.applyFilter();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  openAddDialog() {
    const firstName = window.prompt('Podaj imię użytkownika:');
    if (!firstName) return;
    const lastName = window.prompt('Podaj nazwisko użytkownika:');
    if (!lastName) return;
    const email = window.prompt('Podaj email użytkownika:');
    if (!email) return;
    const password = window.prompt('Podaj hasło użytkownika:');
    if (!password) return;

    const newUser: User = {
      userId: 0, // userId will be set by backend
      firstName,
      lastName,
      email,
      password,
    };
    this.usersService.addUser(newUser).subscribe({
      next: () => this.loadUsers(),
      error: (err) => window.alert('Błąd podczas dodawania użytkownika: ' + err.message),
    });
  }
}
