import {Component, DestroyRef, forwardRef, inject} from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormControl,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
  TouchedChangeEvent, ValidationErrors,
  Validator,
  Validators,
  ValueChangeEvent
} from '@angular/forms';
import {NgIf} from '@angular/common';
import {MatFormFieldModule} from '@angular/material/form-field';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {MatInputModule} from '@angular/material/input';
import { Book } from '../../../../shared/models';

@Component({
  selector: 'app-add-book-form',
  imports: [
    NgIf,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule
  ],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => AddBookFormComponent),
    multi: true
  }, {
    provide: NG_VALIDATORS,
    useExisting: forwardRef(() => AddBookFormComponent),
    multi: true
  }],
  templateUrl: './add-book-form.component.html',
  styleUrl: './add-book-form.component.scss'
})
export class AddBookFormComponent implements ControlValueAccessor, Validator {

  bookForm = new FormGroup({
    title: new FormControl<string>('', [Validators.required]),
    author: new FormControl<string>('', [Validators.required]),
    publicationYear: new FormControl<number | undefined>(undefined, [Validators.required, Validators.min(1)]),
    categoryId: new FormControl<number | undefined>(undefined),
    shelf: new FormControl<string | undefined>('-'),
  });

  private onValidatorChange: () => void = () => {};
  private onChange: (value: string) => void = (value: string) => {};
  private onTouched: () => void = () => {};

  private destroyRef = inject(DestroyRef);

  constructor() {
      this.bindFormEvents();
  }

  validate(control: AbstractControl): ValidationErrors | null {
    return this.bookForm.errors;
  }

  registerOnValidatorChange?(fn: () => void): void {
    this.onValidatorChange = fn;
  }

  writeValue(newValue: Book): void {
    this.bookForm.patchValue(newValue)
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    isDisabled ? this.bookForm.disable() : this.bookForm.enable();
  }

  private bindFormEvents(): void {
    this.bookForm.events
      .pipe(
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe((event) => {
      if (event instanceof TouchedChangeEvent) {
        this.onTouched();
      }
      if (event instanceof ValueChangeEvent) {
        this.onTouched();
        this.onChange(event.value);
        this.onValidatorChange();
      }
    });
  }
}
