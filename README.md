# Upravljanje distribuiranim energijama (DER)

**DERManagementSolution** je sveobuhvatan sistem dizajniran za upravljanje distribuiranim energijama (DER). Ovo rešenje se sastoji od nekoliko projekata, od kojih svaki odgovara za različite aspekte aplikacije.

## Sadržaj
- **Projekti**
  - Common
  - DERServer
  - DERClient
  - UserClient
  - DERManagementSystem.Tests
- **Baza podataka:** XML
- **Kako početi**
- **Testiranje**

## Projekti

### Opis komunikacije
Komunikacija između servera i klijenata je omogućena pomoću WCF (Windows Communication Foundation) komunikacije, koja pruža fleksibilan model komunikacije između klijenta i servera. Ovaj model podržava različite protokole, bezbednosne opcije i načine razmene podataka. WCF je izabran zbog svoje široke primene u raznim projektima na ovom GitHub profilu.

### Common
Projekat **Common** sadrži deljene modele i interfejse koji se koriste u drugim projektima unutar rešenja. Definiše strukture podataka za resurse, rasporede i statistiku. Takođe sadrži folder **Data**, u kome se nalazi klasa za operacije sa XML bazom podataka.

### DERServer
Projekat **DERServer** implementira funkcionalnost na strani servera za upravljanje resursima. Izlaže usluge za registraciju, deaktivaciju i praćenje resursa. Takođe upravlja skladištem podataka o resursima u XML datoteci.

### DERClient
Projekat **DERClient** pruža klijentsku logiku za interakciju sa **DERServer**. Omogućava korisnicima da aktiviraju i deaktiviraju resurse, kao i da preuzmu informacije o traženim resursima i njihovim rasporedima.

### UserClient
Projekat **UserClient** služi kao korisnički interfejs i omogućava različite operacije nad resursima, kao i kontrolu interakcije sa korisnicima.

### DERManagementSystem.Tests
Projekat **DERManagementSystem.Tests** sadrži jedinične testove za servise i klijentske komponente rešenja. Koristi MSTest i Moq za validaciju funkcionalnosti i osiguranje pouzdanosti sistema.

## Kako početi

### Preduslovi
- .NET Framework 4.8 ili noviji
- Visual Studio (bilo koja edicija, preporučena verzija 2022)

### Pokretanje rešenja
1. Otvorite rešenje u Visual Studio-u.
2. Postavite **DERServer** kao projekat koji se prvi pokreće, a zatim pokrenite **DERClient** i **UserClient**.
3. Pokrenite projekte na opciji "Start".
4. U konzoli **UserClient** imate ponuđene opcije za dodavanje resursa (preko konzole ili iz fajla), prikazivanje svih informacija o resursima (uključujući aktivnu snagu i proizvedenu energiju), kao i brisanje svih dosadašnjih resursa iz XML baze.
5. U konzoli **DERClient** unesite ID resursa koji ste dodali preko **UserClient**. Ako resurs postoji, biće prikazane informacije o njemu i njegovom rasporedu. Takođe, imate mogućnost aktivacije i deaktivacije resursa, uz ispis svih potrebnih informacija na **DERServer**.
6. Kada deaktivirate resurse iz **DERClient**, informacije u **UserClient** prikazaće ukupnu proizvedenu energiju na osnovu vremena aktivacije i jačine resursa.

## Testiranje rešenja
1. Projekat **DERManagementSystem.Tests** je zadužen za testiranje pomoću jediničnih testova.
2. U Visual Studio-u idite na **Test** -> **Test Explorer** -> **Run All Tests In View** kako biste pokrenuli sve testove.
3. Ako su svi testovi prošli, to znači da aplikacija funkcioniše bez grešaka.

## Licenca
Ovaj projekat je licenciran pod MIT licencom.

## U nastavku imate DEMO snimak testiranja aplikacije:
