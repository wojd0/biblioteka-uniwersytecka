import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-search-box',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.scss'],
})
export class SearchBoxComponent {
  @Input() label: string = 'Search...';
  @Input() placeholder: string = '';
  @Input() value: string = '';
  @Output() valueChange = new EventEmitter<string>();
  @Output() search = new EventEmitter<string>();

  clearSearch(): void {
    this.valueChange.emit('');
  }

  onEnterKey(event: Event): void {
    event.preventDefault();
    if (this.value.trim()) {
      this.search.emit(this.value);
    }
  }
}
