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

@Component({
  selector: 'app-add-book-dialog',
  standalone: true,
  template: `
    <h2 mat-dialog-title>Dodaj książkę</h2>
    <div mat-dialog-content>
      <form #bookForm="ngForm">
        <mat-form-field style="width:100%">
          <mat-label>Tytuł</mat-label>
          <input matInput [(ngModel)]="book.title" name="title" required />
        </mat-form-field>
        <mat-form-field style="width:100%">
          <mat-label>Autor</mat-label>
          <input matInput [(ngModel)]="book.author" name="author" required />
        </mat-form-field>
        <mat-form-field style="width:100%">
          <mat-label>Rok wydania</mat-label>
          <input
            matInput
            type="number"
            [(ngModel)]="book.publicationYear"
            name="publicationYear"
            required
          />
        </mat-form-field>
        <mat-form-field style="width:100%">
          <mat-label>Kategoria</mat-label>
          <input matInput [(ngModel)]="book.category" name="category" />
        </mat-form-field>
        <mat-form-field style="width:100%">
          <mat-label>Półka</mat-label>
          <input matInput [(ngModel)]="book.shelf" name="shelf" />
        </mat-form-field>
      </form>
    </div>
    <div mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Anuluj</button>
      <button
        mat-raised-button
        color="primary"
        [disabled]="!book.title || !book.author || !book.publicationYear"
        (click)="onAdd()"
      >
        Dodaj
      </button>
    </div>
  `,
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

  constructor(
    public dialogRef: MatDialogRef<AddBookDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  onCancel() {
    this.dialogRef.close();
  }

  onAdd() {
    this.dialogRef.close(this.book);
  }
}
