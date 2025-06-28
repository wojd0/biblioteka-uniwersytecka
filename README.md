# Aplikacja biblioteki uniwersyteckiej GetPapierek

W pełni funkcjonalna biblioteka uniwersytecka przyjazna zarówno dla użytkowników jak i administratorów.

Działanie:
Przy pomocy API Getpapierek użytkownik może wejść w interakcje z bazą danych szkolnej biblioteki. Wersja zawarta w projekcie będzie używała statycznie zapisanych danych

Możliwości programu

- Dodawania pozycji
- Usuwanie pozycji
- Modyfikowanie rekordów
- Przyjazny sposób wyszukiwania
- System logowania i rejestracji użytkowników
- Responsywny interfejs użytkownika
- Otwarte API dla deweloperów

Język obsługi progamu: C#

## Przykłady paradygmatów programowania obiektowego w projekcie

Poniżej przedstawiono 6 przykładów realizacji paradygmatów OOP w projekcie wraz z odwołaniami do plików i fragmentów kodu:

1. **Enkapsulacja (Hermetyzacja)**

   - Wstrzykiwane zależności są dostępne tylko z poziomu klasy, do której zostały wstrzyknięte. By zapobiec problemom z dostępem do repozytoriów, są one prywatne i dostępne tylko w obrębie klasy, co zapewnia hermetyzację.
   - Plik: [`GetPapierek/Controllers/RentalController.cs`](GetPapierek/Controllers/RentalController.cs)
   - Fragment:
     ```csharp
      private readonly IRentalRepository _wypozyczenieRepository;
      private readonly IBookRepository _ksiazkaRepository;
     ```

2. **Abstrakcja**

   - Interfejsy repozytoriów (`IBookRepository`, `ICategoryRepository`, `IRentalRepository`, `IUserRepository`) definiują kontrakty dla operacji na danych, ukrywając szczegóły implementacji.
   - Plik: [`GetPapierek/Repositories/Interfaces/IBookRepository.cs`](GetPapierek/Repositories/Interfaces/IBookRepository.cs)
   - Fragment:
     ```csharp
     public interface IBookRepository
     {
         Task<List<Book>> GetAllAsync();
         // ...
     }
     ```

3. **Dziedziczenie**

   - Klasa `LibraryDbContextModelSnapshot` dziedziczy po `ModelSnapshot` (Entity Framework), umożliwiając rozszerzenie funkcjonalności.
   - Plik: [`GetPapierek/Migrations/LibraryDbContextModelSnapshot.cs`](GetPapierek/Migrations/LibraryDbContextModelSnapshot.cs)
   - Fragment:
     ```csharp
     partial class LibraryDbContextModelSnapshot : ModelSnapshot
     {
         // ...
     }
     ```

4. **Polimorfizm**

   - Repozytoria implementują interfejsy, umożliwiając korzystanie z różnych implementacji przez ten sam interfejs (np. `BookRepository` implementuje `IBookRepository`).
   - Plik: [`GetPapierek/Repositories/BookRepository.cs`](GetPapierek/Repositories/BookRepository.cs)
   - Fragment:
     ```csharp
     public class BookRepository : IBookRepository
     {
         // ...
     }
     ```

5. **Kompozycja**

   - W aplikacji frontendowej Angular komponent `app.component.html` korzysta z kompozycji, osadzając w sobie inne komponenty, np. navbar czy home-card, budując złożony interfejs z prostszych elementów.
   - Plik: [`GetPapierekWeb/src/app/app.component.html`](GetPapierekWeb/src/app/app.component.html)
   - Fragment:

     ```html
     <app-navbar></app-navbar>
     <main class="app__main">
       <router-outlet class="app__router-outlet"></router-outlet>
     </main>
     ```

6. **Wstrzykiwanie zależności (Dependency Injection)**

   - W aplikacji frontendowej Angular wstrzykiwanie zależności realizowane jest poprzez deklarowanie serwisów w konstruktorach komponentów. Przykładowo, serwis `BookService` może być wstrzyknięty do komponentu `BooksComponent`, co pozwala na korzystanie z logiki biznesowej i komunikacji z API bez bezpośredniego tworzenia instancji serwisu.
   - Plik: [`GetPapierekWeb/src/app/books/list/books-list.component.ts`](GetPapierekWeb/src/app/books/list/books-list.component.ts)
   - Fragment:

     ```typescript
     export class BooksListComponent implements OnInit {
       private booksService = inject(BooksService);
       private usersService = inject(UsersService);
       private rentalsService = inject(RentalsService);
       private dialog = inject(MatDialog);
       private snackbar = inject(MatSnackBar);
       // ...
     }
     ```
