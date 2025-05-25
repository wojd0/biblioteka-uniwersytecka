import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SearchService, SearchResult } from '../../search.service';

@Component({
  selector: 'app-books-search',
  standalone: true,
  templateUrl: './books-search.component.html',
  imports: [CommonModule, FormsModule],
})
export class BooksSearchComponent {
  query = '';
  result: SearchResult | null = null;
  loading = false;

  constructor(private searchService: SearchService) {}

  onSearch() {
    if (!this.query.trim()) return;
    this.loading = true;
    this.searchService.search(this.query).subscribe((res) => {
      this.result = res;
      this.loading = false;
    });
  }
}
