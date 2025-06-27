import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../shared/auth.service';

@Component({
  selector: 'app-navbar-menu',
  templateUrl: './navbar-menu.component.html',
  imports: [MatButtonModule, MatIconModule, RouterLink],
})
export class NavbarMenuComponent {
  private readonly auth = inject(AuthService);

  isMobile = signal<boolean | null>(null);
  resizeObserver = signal<ResizeObserver | null>(null);

  menuItems = signal<
    { label: string; icon: string; url: string; protected?: boolean }[]
  >([
    {
      label: 'Książki',
      icon: 'menu_book',
      url: '/ksiazki',
    },
    {
      label: 'Wypożyczenia',
      icon: 'assignment_returned',
      url: '/wypozyczenia',
      protected: true,
    },
    {
      label: 'Użytkownicy',
      icon: 'people',
      url: '/uzytkownicy',
      protected: true,
    },
  ]);

  isLoggedIn() {
    return this.auth.isLoggedIn();
  }

  logout() {
    this.auth.logout();
  }

  ngOnInit() {
    this.handleResponsiveButtons();
  }

  ngOnDestroy() {
    if (this.resizeObserver) {
      this.resizeObserver()?.disconnect();
    }
  }

  private handleResponsiveButtons() {
    const resizeObserver = new ResizeObserver((entries) => {
      for (const entry of entries) {
        this.isMobile.set(entry.contentRect.width < 600);
      }
    });
    resizeObserver.observe(document.body);

    this.resizeObserver.set(resizeObserver);
  }
}
