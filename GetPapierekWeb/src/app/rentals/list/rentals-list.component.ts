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
import { map, Observable, switchMap, take } from 'rxjs';

@Component({
  selector: 'app-rentals-list',
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
    this.loadRentals().subscribe(() => {
      this.sort.initialized.pipe(take(1)).subscribe(() => {
        this.sort.sort({
          id: 'rentalDate',
          start: 'desc',
          disableClear: false,
        });
      });
    });
  }

  loadRentals(): Observable<Rental[]> {
    return this.rentalsService.getRentals().pipe(
      take(1),
      map((rentals) => {
        this.rentals = rentals;
        this.applyFilter();
        this.dataSource.sort = this.sort;
        return rentals;
      })
    );
  }

  applyFilter(): void {
    const term = this.searchTerm.toLowerCase();
    this.filteredRentals = this.rentals.filter(
      (r) =>
        (r.book?.title?.toLowerCase().includes(term) ?? false) ||
        (r.user &&
          `${r.user.firstName} ${r.user.lastName}`
            .toLowerCase()
            .includes(term)) ||
        (r.status === 0 && 'wypożyczona'.includes(term)) ||
        (r.status === 1 && 'zwrócona'.includes(term))
    );
    this.dataSource.data = this.filteredRentals;
    this.dataSource.sort = this.sort;
  }

  onSearchChange(): void {
    this.applyFilter();
  }

  onBookReturn(rentalId: number): void {
    this.rentalsService
      .makeReturn(rentalId)
      .pipe(
        take(1),
        switchMap(() => this.loadRentals())
      )
      .subscribe(() => {});
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  getStatusText(status: number): string {
    return status === 0 ? 'Wypożyczona' : 'Zwrócona';
  }
}
