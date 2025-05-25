import { Component, OnInit, ViewChild } from '@angular/core';
import { BooksService, Book } from '../books.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { SearchBoxComponent } from '../../shared/components/search-box/search-box.component';

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
    SearchBoxComponent,
  ],
})
export class BooksListComponent implements OnInit {
  books: Book[] = [];
  filteredBooks: Book[] = [];
  searchTerm = '';
  dataSource = new MatTableDataSource<Book>([]);
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private booksService: BooksService) {
    this.dataSource.sortingDataAccessor = (item: Book, property: string) => {
      switch (property) {
        case 'year':
          return item.publicationYear;
        case 'category':
          return item.category?.name || '';
        case 'shelf':
          return item.shelf || '';
        default:
          // fallback for title, author, etc.
          return (item as any)[property];
      }
    };
  }

  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.booksService.getBooks().subscribe((books) => {
      this.books = books;
      this.applyFilter();
      this.dataSource.sort = this.sort;
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
}
