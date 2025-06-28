import { Component, Inject } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { BooksService } from '../../books.service';
import { inject } from '@angular/core';
import { AddBookFormComponent } from './add-book-form/add-book-form.component';
import { AddBookFormModel } from './add-book-form/add-book-form.model';

@Component({
  selector: 'app-add-book-dialog',
  standalone: true,
  templateUrl: './add-book-dialog.component.html',
  imports: [
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    AddBookFormComponent,
    ReactiveFormsModule,
  ],
})
export class AddBookDialogComponent {
  bookControl = new FormControl<AddBookFormModel>({
    title: '',
    author: '',
    publicationYear: undefined,
    shelf: undefined,
    categoryId: undefined,
    disabled: false,
  });

  private booksService = inject(BooksService);
  loading = false;
  error: string | null = null;

  constructor(
    public dialogRef: MatDialogRef<AddBookDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onCancel() {
    this.dialogRef.close();
  }

  onAdd() {
    this.loading = true;
    this.error = null;
    const payload: any = {
      title: this.bookControl.value?.title,
      author: this.bookControl.value?.author,
      publicationYear: this.bookControl.value?.publicationYear,
      shelf: '-',
      categoryId: this.bookControl.value?.categoryId,
    };
    this.booksService.addBook(payload).subscribe({
      next: (createdBook) => {
        this.loading = false;
        this.dialogRef.close(createdBook);
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Błąd podczas dodawania książki.';
      },
    });
  }
}
