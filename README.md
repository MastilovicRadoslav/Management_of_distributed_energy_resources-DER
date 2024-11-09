# Upravljanje distribuiranim energijama (DER)

**DERManagementSolution** je sveobuhvatan sistem dizajniran za upravljanje distribuiranim energijama (DER). Ovo rješenje se sastoji od nekoliko projekata, od kojih svaki odgovara za različite aspekte aplikacije.

## Sadržaj
- **Projekti**
  - Common
  - DERServer
  - DERClient
  - UserClient
  - DERManagementSystem.Tests
- **Baza podataka:** XML i SSMS
- **Kako početi**
- **Testiranje**

## Projekti

### Opis komunikacije
Komunikacija između servera i klijenata omogućena je pomoću WCF (Windows Communication Foundation) komunikacije, koja pruža fleksibilan model komunikacije između klijenta i servera. Ovaj model podržava različite protokole, bezbjednosne opcije i načine razmjene podataka. WCF je izabran zbog svoje široke primjene kao i to što je ga autor koristio na raznim projektima na ovom GitHub profilu i fakultetu.

### Common
Projekat **Common** sadrži dijeljene modele (folder **Models**) i interfejse (folder **Interfaces**) koji se koriste u drugim projektima unutar rješenja. Definiše strukture podataka za resurse, rasporede i statistiku. Takođe sadrži folder **Data**, u kome se nalazi klasa za operacije sa XML bazom podataka.

### DERServer
Projekat **DERServer** implementira funkcionalnost na strani servera za upravljanje resursima (folder **Services**). Izlaže usluge za registraciju, deaktivaciju i praćenje resursa. Takođe upravlja skladištem podataka o resursima, podržavajući rad sa XML i SSMS bazama podataka. U slučaju XML baze, ID resursa se generiše ručno kroz konzolu, dok kod SSMS baze podataka ID automatski generiše baza.

### DERClient
Projekat **DERClient** pruža klijentsku logiku za interakciju sa **DERServer**. Omogućava korisnicima da aktiviraju i deaktiviraju resurse (folder **Services**), kao i da preuzmu informacije o traženim resursima i njihovim rasporedima (autor posjeduje i rješenje u obliku automatskog podesavanja pokretanja i simulacije DERClient projekta (distribuiranog resursa) ali je zbog lakšeg pregleda i testiranja odlucio se na ovaj način).

### UserClient
Projekat **UserClient** služi kao korisnički interfejs i omogućava različite operacije nad resursima (folder **Services**), kao i kontrolu interakcije sa korisnicima.

### DERManagementSystem.Tests
Projekat **DERManagementSystem.Tests** sadrži jedinične testove za servise i klijentske komponente rešenja. Koristi MSTest i Moq za validaciju funkcionalnosti i osiguranje pouzdanosti sistema, ovo je na osnovu predmeta na fakultetu na kojem je bila implementacija testova, repozitorijum **Elementi_razvoja_softvera - ERS**.

## Kako početi

### Preduslovi
- .NET Framework 4.8 ili noviji
- Visual Studio (bilo koja edicija, preporučena verzija 2022)

### Pokretanje rešenja
1. Otvorite rešenje u Visual Studio-u.
2. Postavite **DERServer** kao projekat koji se prvi pokreće, a zatim pokrenite **UserClient** i **DerClient**.
3. Pokrenite projekte na opciji "Start".
4. U konzoli **UserClient** imate ponuđene opcije za dodavanje resursa (preko konzole ili iz fajla), prikazivanje svih informacija o resursima (uključujući aktivnu snagu i proizvedenu energiju), kao i brisanje svih dosadašnjih resursa iz baze (XML ili SSMS).
5. U konzoli **DERClient** unesite ID resursa koji ste dodali preko **UserClient**. Ako resurs postoji, biće prikazane informacije o njemu i njegovom rasporedu. Takođe, imate mogućnost aktivacije i deaktivacije resursa, uz ispis svih potrebnih informacija na **DERServer** kao i na ostalim projektima. Što se tiče automatizovanog rješenja tu se distribuirani resurs random izabere i simulira se njegovo pokretanje kao i zaustavljanje uz odgovarajuće ispise bez toga da se manuelno bira resurs.
6. Kada deaktivirate resurse iz **DERClient** (nije neophodno deaktivirati sve se vidi u real-time vremenu i dok rade i dok ne rade), informacije u **UserClient** prikazaće **ukupnu kolicinu energije (kWh) koju su resursi proizveli od pocetka rada servera - računa se sabiranjem količina energija koji se svaki resurs proizveo na osnovu svog vremena rada** i **ukupnu snagu (kW) trenutno angazovanih resursa - računa se zbirom snaga savkog resursa** kao i sve ostale informacije koje će biti prikazane na konzoli.
   
### Napomena
- U slučaju korišćenja **XML baze podataka**, ID resursa se unosi ručno kroz konzolu prilikom registracije resursa.
- U slučaju **SSMS baze podataka**, ID se automatski generiše prilikom dodavanja resursa u bazu.

## Testiranje rešenja
1. Projekat **DERManagementSystem.Tests** je zadužen za testiranje pomoću jediničnih testova.
2. U Visual Studio-u idite na **Test** -> **Test Explorer** -> **Run All Tests In View** kako biste pokrenuli sve testove.
3. Ako su svi testovi prošli, to znači da aplikacija funkcioniše bez grešaka.

## Licenca
Ovaj projekat je licenciran pod MIT licencom.

## DEMO 1 (aplikacija kao manuelno pokretanje DERClient):
U nastavku se nalazi demo snimak testiranja aplikacije sa SSMS bazom podataka (sa XML je isto samo se ID dodatno unosi) koji prikazuje kako funkcionišu različite operacije u **UserClient** i **DERClient** konzolama sve to povezano sa **DERServer**:

[YouTube Video manually](https://www.youtube.com/watch?v=ZXahiiVbVGU&ab_channel=%D0%A0%D0%B0%D0%B4%D0%BE%D1%81%D0%BB%D0%B0%D0%B2%D0%9C%D0%B0%D1%81%D1%82%D0%B8%D0%BB%D0%BE%D0%B2%D0%B8%D1%9B)

## DEMO 2 (aplikacija kao automatsko pokretanje DERClient):

[YouTube Video automatically](https://www.youtube.com/watch?v=1O6RYomK_1Y&ab_channel=%D0%A0%D0%B0%D0%B4%D0%BE%D1%81%D0%BB%D0%B0%D0%B2%D0%9C%D0%B0%D1%81%D1%82%D0%B8%D0%BB%D0%BE%D0%B2%D0%B8%D1%9B)




