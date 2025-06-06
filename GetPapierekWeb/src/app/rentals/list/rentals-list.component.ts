import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { RentalsService, Rental } from '../rentals.service';
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
  selector: 'app-rentals-list',
  standalone: true,
  templateUrl: './rentals-list.component.html',
  styleUrls: ['./rentals-list.component.scss'],
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
export class RentalsListComponent implements OnInit {
  private rentalsService = inject(RentalsService);

  rentals: Rental[] = [];
  filteredRentals: Rental[] = [];
  searchTerm = '';
  dataSource = new MatTableDataSource<Rental>([]);
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.loadRentals();
  }

  loadRentals() {
    this.rentalsService.getRentals().subscribe((rentals) => {
      this.rentals = rentals;
      this.applyFilter();
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter() {
    const term = this.searchTerm.toLowerCase();
    this.filteredRentals = this.rentals.filter(
      (r) =>
        (r.book?.title?.toLowerCase().includes(term) ?? false) ||
        (r.user && `${r.user.firstName} ${r.user.lastName}`.toLowerCase().includes(term)) ||
        (r.status === 0 && 'wypożyczona'.includes(term)) ||
        (r.status === 1 && 'zwrócona'.includes(term))
    );
    this.dataSource.data = this.filteredRentals;
    this.dataSource.sort = this.sort;
  }

  onSearchChange() {
    this.applyFilter();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  getStatusText(status: number): string {
    return status === 0 ? 'Wypożyczona' : 'Zwrócona';
  }
}
