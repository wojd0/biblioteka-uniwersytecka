import { Component, inject } from '@angular/core';
import { AuthService } from '../../auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login-form',
  standalone: true,
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
})
export class LoginFormComponent {
  email = '';
  password = '';
  error = '';

  private auth = inject(AuthService);
  private router = inject(Router);
  private notificationService = inject(MatSnackBar);

  onLogin() {
    this.error = '';
    this.auth.login(this.email, this.password).subscribe({
      next: () => {
        this.router.navigateByUrl('/');
        this.notificationService.open('Zalogowano pomyślnie');
      },
      error: (err) => {
        this.error = err.error || 'Błąd logowania';
      },
    });
  }
}
