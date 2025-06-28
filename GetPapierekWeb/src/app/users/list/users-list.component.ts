import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { UsersService, User } from '../users.service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box.component';
import { AuthService } from '../../shared/auth.service';
import { UserAddFormComponent } from './user-add-form.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-users-list',
  standalone: true,
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatTableModule,
    MatButtonModule,
    MatSortModule,
    SearchBoxComponent,
    MatDialogModule,
  ],
})
export class UsersListComponent implements OnInit {
  private usersService = inject(UsersService);
  auth = inject(AuthService);
  private dialog = inject(MatDialog);

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
    const dialogRef = this.dialog.open(UserAddFormComponent, {
      width: '400px',
      height: 'auto',
    });
    dialogRef.afterClosed().subscribe((result: User | undefined) => {
      if (result) {
        this.usersService.addUser(result).subscribe({
          next: () => this.loadUsers(),
          error: (err) =>
            window.alert('Błąd podczas dodawania użytkownika: ' + err.message),
        });
      }
    });
  }
}
