# Expense Tracker — repository instructions

## Project

ASP.NET Core Web API con Entity Framework Core, SQL Server e frontend statico in HTML, CSS e JavaScript ES Modules.

## Frontend conventions

* Usa HTML, CSS, JavaScript ES Modules e Bootstrap 5.
* Non introdurre framework frontend o build npm senza richiesta esplicita.
* Mantieni separate API, logica applicativa e manipolazione DOM.
* Preferisci funzioni piccole e nomi espliciti.
* Non usare stili inline.
* Usa `wwwroot/css/app.css` per il design condiviso.
* Mantieni il layout responsive e accessibile.
* Non aggiungere dati finti.

## Backend conventions

* Mantieni la separazione Controller → Service → Repository.
* Non modificare DTO, entità, migration o API durante task esclusivamente frontend.
* Usa async/await per le operazioni I/O.
* Non inserire connection string, password o segreti nel repository.

## Working agreement

* Preserva le funzionalità esistenti.
* Controlla il diff prima di terminare.
* Esegui build e test disponibili.
* Non effettuare refactoring non collegati al task.
* Nel riepilogo finale sii sintetico.
