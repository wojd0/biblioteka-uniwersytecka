import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { BooksService } from '../books.service';
import { UsersService, User } from '../../users/users.service';
import { RentalsService } from '../../rentals/rentals.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatSelectModule } from '@angular/material/select';
import { MatMenuModule } from '@angular/material/menu';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box.component';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { AddBookDialogComponent } from './add-book-dialog/add-book-dialog.component';
import { Book } from '../../shared/models';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../shared/auth.service';

@Component({
  selector: 'app-books-list',
  standalone: true,
  templateUrl: './books-list.component.html',
  styleUrls: ['./books-list.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatTableModule,
    MatButtonModule,
    MatSortModule,
    MatDialogModule,
    MatSelectModule,
    MatMenuModule,
    SearchBoxComponent,
  ],
})
export class BooksListComponent implements OnInit {
  private booksService = inject(BooksService);
  private usersService = inject(UsersService);
  private rentalsService = inject(RentalsService);
  private dialog = inject(MatDialog);
  private snackbar = inject(MatSnackBar);
  private auth = inject(AuthService);

  books: Book[] = [];
  filteredBooks: Book[] = [];
  users: User[] = [];
  searchTerm = '';
  dataSource = new MatTableDataSource<Book>([]);
  @ViewChild(MatSort) sort!: MatSort;

  constructor() {
    this.dataSource.sortingDataAccessor = (item: Book, property: string) => {
      switch (property) {
        case 'year':
          return item.publicationYear;
        case 'category':
          return item.category?.name || '';
        case 'shelf':
          return item.shelf || '';
        default:
          return (item as any)[property];
      }
    };
  }

  ngOnInit() {
    this.loadBooks();
    this.loadUsers();
  }

  loadBooks() {
    this.booksService.getBooks().subscribe((books) => {
      this.books = books;
      this.applyFilter();
      this.dataSource.sort = this.sort;
    });
  }

  loadUsers() {
    this.usersService.getUsers().subscribe((users) => {
      this.users = users;
    });
  }
  applyFilter() {
    const term = this.searchTerm.toLowerCase();
    this.filteredBooks = this.books.filter(
      (b) =>
        b.title.toLowerCase().includes(term) ||
        b.author.toLowerCase().includes(term) ||
        (b.category?.name?.toLowerCase().includes(term) ?? false)
    );
    this.dataSource.data = this.filteredBooks;
    this.dataSource.sort = this.sort;
  }

  onSearchChange() {
    this.applyFilter();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  rentBook(bookId: number, userId: number) {
    this.rentalsService.createRental(bookId, userId).subscribe({
      next: (rental) => {
        console.log('Book rented successfully:', rental);
        this.snackbar.open(
          'Książka została pomyślnie wypożyczona!',
          'Zamknij',
          {
            duration: 3000,
          }
        );
      },
      error: (error) => {
        console.error('Error renting book:', error);
        this.snackbar.open(
          'Wystąpił błąd podczas wypożyczania książki. Spróbuj ponownie.',
          'Zamknij',
          {
            duration: 3000,
          }
        );
      },
    });
  }

  openAddDialog() {
    const dialogRef = this.dialog.open(AddBookDialogComponent, {
      width: '400px',
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const newBook: Book = {
          title: result.title,
          author: result.author,
          publicationYear: Number(result.publicationYear),
          category:
            result.category && typeof result.category === 'object'
              ? result.category
              : { name: result.category || '' },
          shelf: result.shelf || '',
        } as Book;
        this.books.push(newBook);
        this.applyFilter();
      }
    });
  }
}
