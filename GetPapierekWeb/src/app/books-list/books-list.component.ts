import { Component, OnInit } from '@angular/core';
import { BooksService, Book } from './books.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BooksSearchComponent } from './books-search.component';

@Component({
  selector: 'app-books-list',
  standalone: true,
  templateUrl: './books-list.component.html',
  styleUrls: ['./books-list.component.scss'],
  imports: [CommonModule, FormsModule, HttpClientModule, BooksSearchComponent],
})
export class BooksListComponent implements OnInit {
  books: Book[] = [];
  filteredBooks: Book[] = [];
  searchTerm = '';

  constructor(private booksService: BooksService) {}

  ngOnInit() {
    this.loadBooks();
  }

  loadBooks() {
    this.booksService.getBooks().subscribe((books) => {
      this.books = books;
      this.applyFilter();
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
  }

  onSearchChange() {
    this.applyFilter();
  }
}
