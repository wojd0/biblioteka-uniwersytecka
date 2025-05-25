import { Component, OnInit, ViewChild } from '@angular/core';
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
  users: User[] = [];
  filteredUsers: User[] = [];
  searchTerm = '';
  dataSource = new MatTableDataSource<User>([]);
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private usersService: UsersService) {}

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
    const term = this.searchTerm.toLowerCase();
    this.filteredUsers = this.users.filter(
      (u) =>
        (u.name?.toLowerCase?.().includes(term) ?? false) ||
        (u.email?.toLowerCase?.().includes(term) ?? false) ||
        (u.role?.toLowerCase?.().includes(term) ?? false)
    );
    this.dataSource.data = this.filteredUsers;
    this.dataSource.sort = this.sort;
  }

  onSearchChange() {
    this.applyFilter();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
}
