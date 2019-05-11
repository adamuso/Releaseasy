# Releaseasy

An web application for project and team management.

## Building

Required dependencies:
* [Visual Studio 2017](https://visualstudio.microsoft.com/pl/downloads/?rr=https%3A%2F%2Fwww.google.pl%2F)
* [.NET Core 2.2.106](https://dotnet.microsoft.com/download/dotnet-core/2.2#sdk-2.2.106) (this one is compatible with VS2017)
* MS SQL Database (can be local using [MS SQL Server](https://www.microsoft.com/pl-pl/sql-server/sql-server-editions-express) or remote if available) + [SMSS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017) (very handy tool)
* [Node.JS](https://nodejs.org/en/) for building the front-end

## Docker support

Dokcer support is planned but not ready yet

# Dodatkowe założenia

Aplikacja:
- główna strona wyświetla projekty i użytkowników
- po zalogowaniu użytkownikowi wyświetlany zostaje jego przydział zadań
- kliknięcie na zadanie przenosi do przypisanego zadania wewnatrz projektu
- możliwości filtracji

Projekt:
 - nazwa, opis, czas trwania, pula dziennych punków, grupy zawierające się w projekcie
- podział na "pomysły", "zadania do zrobienia", "zadania w trakcie", "oddane", "zadania przeznaczone do testów", "przetestowane", "gotowe do wydania".

Zadanie:
- nazwa, opis, rodzaj grupy, punktacja, czas trwania, 
- łączna pula punktów zadań nie noże przekraczaś możliwej do wykorzystania liczby punktów dziennej projektu
- zadanie musi zawierać grupę zależną od grup wyznaczonych w projekcie
- czas trwania zadania musi znajdować się z czasie trwania projektu
- jeżeli zadanie nie ma ilości punktów i lub grupy, jest zawsze przydzielane do "to do"
- możliwość ustawienia priorytetu dla zadania
- 

Zespół:
- nazwa, opis

Grupy użytkowników:
- unikalna nazwa, opis, opcjonalnie przypisane tagi

Tagi:
- unikalna nazwa, opis
- możliwość dodawania, usuwania i edycji

Użytkownik:
- avatar nazwa(imie i nazwisko) rodzaje grup do jakich należy (np. frontend, scrum manager, księgowa) razem z przypisanym poziomem dla danej kategorii, opis, umiejętności(jako tagi), orientacyjne godziny pracy, gdzie najczęściej można go znaleźć
- prosta skrzynka wiadomości w postaci notek jak i zadań które można uaktywnić (przykład: PM wysyła zadanie do zatwierdzenia przez osobę A, osoba A zgada się i odrazu przypisuje to zadanie do projektu)
- pula zadań o łacznej wartości nie większej niż limit użytkownika (np 8h * 4<-lvl umiejętności = 32 pkt na dzień)
- zadania zgodne z zakresem obowiązków
- stanowisko(pm, dev, itp) zależne od zadania
- określona liczba punków do rozdysponowana odzwierciedlająca stosunek czasu do umiejętności dla danego zadania(np zadanie to frontend użytkonik ma 24pkt a zadanie ma 60pkt i jest na czas dwóch dni 60 > 24*2 więc nie można go przypisać do zadania


