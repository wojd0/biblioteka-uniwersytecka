import { Component, forwardRef } from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-book-filters',
  imports: [MatFormFieldModule, MatSelectModule, ReactiveFormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => BookFiltersComponent),
      multi: true,
    },
  ],
  templateUrl: './book-filters.component.html',
  styleUrl: './book-filters.component.scss',
})
export class BookFiltersComponent implements ControlValueAccessor {
  onChange!: (value: any) => void;
  onTouched!: () => void;
  onSetDisabledState!: (disabled: boolean) => void;

  writeValue(obj: any): void {
    throw new Error('Method not implemented.');
  }
  registerOnChange(fn: typeof this.onChange): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    throw new Error('Method not implemented.');
  }
}
