import { Component, OnInit } from '@angular/core';
import { UsersService, User } from './users.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-users-list',
  standalone: true,
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
  imports: [CommonModule, FormsModule, HttpClientModule],
})
export class UsersListComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  searchTerm = '';

  constructor(private usersService: UsersService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.usersService.getUsers().subscribe((users) => {
      this.users = users;
      this.applyFilter();
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
  }

  onSearchChange() {
    this.applyFilter();
  }
}
