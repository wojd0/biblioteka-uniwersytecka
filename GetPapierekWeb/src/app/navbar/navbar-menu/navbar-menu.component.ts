import { Component, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar-menu',
  templateUrl: './navbar-menu.component.html',
  imports: [MatButtonModule, MatIconModule, RouterLink],
})
export class NavbarMenuComponent {
  menuItems = signal<{ label: string; icon: string; url: string }[]>([
    {
      label: 'Książki',
      icon: 'menu_book',
      url: '/ksiazki',
    },
    {
      label: 'Wypożyczenia',
      icon: 'assignment_returned',
      url: '/wypozyczenia',
    },
    {
      label: 'Użytkownicy',
      icon: 'people',
      url: '/uzytkownicy',
    },
  ]);
  isMobile = signal<boolean | null>(null);
  resizeObserver = signal<ResizeObserver | null>(null);

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
