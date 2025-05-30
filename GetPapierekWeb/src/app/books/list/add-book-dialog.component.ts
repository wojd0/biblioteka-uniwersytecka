import { Component, Inject } from '@angular/core';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { BooksService } from '../books.service';
import { inject } from '@angular/core';

@Component({
  selector: 'app-add-book-dialog',
  standalone: true,
  templateUrl: './add-book-dialog.component.html',
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
})
export class AddBookDialogComponent {
  book = {
    title: '',
    author: '',
    publicationYear: null,
    category: '',
    shelf: '',
  };

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
      title: this.book.title,
      author: this.book.author,
      publicationYear: Number(this.book.publicationYear),
      shelf: this.book.shelf,
      category: { name: this.book.category || '' },
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
