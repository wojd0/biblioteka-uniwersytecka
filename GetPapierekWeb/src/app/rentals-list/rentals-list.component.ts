import { Component, OnInit } from '@angular/core';
import { RentalsService, Rental } from './rentals.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-rentals-list',
  standalone: true,
  templateUrl: './rentals-list.component.html',
  styleUrls: ['./rentals-list.component.scss'],
  imports: [CommonModule, FormsModule, HttpClientModule],
})
export class RentalsListComponent implements OnInit {
  rentals: Rental[] = [];
  filteredRentals: Rental[] = [];
  searchTerm = '';

  constructor(private rentalsService: RentalsService) {}

  ngOnInit() {
    this.loadRentals();
  }

  loadRentals() {
    this.rentalsService.getRentals().subscribe((rentals) => {
      this.rentals = rentals;
      this.applyFilter();
    });
  }

  applyFilter() {
    const term = this.searchTerm.toLowerCase();
    this.filteredRentals = this.rentals.filter(
      (r) =>
        (r.book?.title?.toLowerCase().includes(term) ?? false) ||
        (r.user?.name?.toLowerCase().includes(term) ?? false) ||
        r.status.toLowerCase().includes(term)
    );
  }

  onSearchChange() {
    this.applyFilter();
  }
}
