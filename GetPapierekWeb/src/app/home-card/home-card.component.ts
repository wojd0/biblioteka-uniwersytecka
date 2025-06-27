import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-home-card',
  standalone: true,
  templateUrl: './home-card.component.html',
  styleUrls: ['./home-card.component.scss'],
  imports: [MatCardModule, MatIconModule, MatButtonModule],
})
export class HomeCardComponent {}
