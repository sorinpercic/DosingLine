# Provocarea săptămânii 5A — „Pompa fără apărare"

**Disciplina:** Programare Orientată pe Obiecte
**An:** II, Automatică și Informatică Aplicată
**Concept vizat:** modificatori de acces, proprietăți (validare, doar citire, calculate), `readonly`, `readonly` ≠ imutabil, validarea în constructor, interfața publică minimă
**Prezentare:** 5 minute, în fața colegilor, la cursul următor

> Fiecare student alege **una** dintre provocările 5A și 5B.

---

## 1. Contextul

Într-o stație de tratare a apei, pompa dozatoare `P-301` injectează hipoclorit (NaOCl) în apă. Un debit prea mare face apa nepotabilă, iar un debit prea mic lasă apa nedezinfectată. Pompa are un debit maxim fix (din fișa tehnică), o tabelă de calibrare cu 4 puncte (debitul măsurat la 25 / 50 / 75 / 100 % din cursă) și un totalizator: litrii dozați de la punerea în funcțiune, pe care se bazează raportul trimis la Direcția de Sănătate Publică.

Clasa `DosingPump` (`Provocarea05A_start/`) a fost scrisă „rapid, ca să meargă HMI-ul": toate datele sunt câmpuri `public`. Partea 1 din `Program.cs` conține codul colegilor (HMI, rețete, raportare, mentenanță) care folosesc pompa. Toți au „rezolvat" câte o problemă scriind direct în câmpuri.

Rulați programul. Partea a doua conține **12 verificări**, iar 9 dintre ele afișează `BUG`. Înainte de orice modificare, răspundeți:

- Cine a modificat debitul maxim și de ce este periculos pentru o pompă de clor?
- De ce tabela de calibrare a pompei s-a schimbat, deși nimeni nu a scris `pump.CalibrationPoints[3] = ...`?
- Ce valoare are `IsRunning` după ce operatorul pune debitul pe 0? De ce?
- Cine a resetat totalizatorul? Ce se întâmplă cu raportul către DSP?

## 2. Cerința

### Etapa 1 — Analiză (pe hârtie, înainte de cod)

1. **Tabelul membrilor.** Pentru fiecare membru al clasei `DosingPump`, completați tabelul din secțiunea 3: cine are voie să-l citească, cine are voie să-l modifice și ce valori sunt valide.
2. **Diagrama UML** a clasei, **înainte** și **după**, cu simbolurile de vizibilitate (`+`, `-`).

### Etapa 2 — Implementare

Modificați **doar** clasa `DosingPump`, astfel încât să se apere singură:

1. **Câmpurile devin private.** Ce trebuie să vadă restul programului se expune prin proprietăți.
2. **Debitul** se validează: în afara intervalului `0 … MaxFlow`, setter-ul aruncă `ArgumentOutOfRangeException` (C4).
3. **Debitul maxim și eticheta** se stabilesc la creare și nu se mai schimbă.
4. **`IsRunning`** nu mai este stocată: se calculează din debit.
5. **Totalizatorul** poate fi citit de oricine, dar crește doar prin `Run(hours)`.
6. **Tabela de calibrare** se poate citi (`Count`, `[i]`), dar nu se poate modifica din exterior. Atenție la lista primită în constructor: cine mai are o referință la ea?
7. **Constructorul** refuză o pompă fără etichetă, cu debit maxim ≤ 0 sau cu alt număr de puncte de calibrare decât 4 (`ArgumentException` / `ArgumentOutOfRangeException`).

După refactorizare, unele linii din Partea 1 **nu vor mai compila**. Este exact scopul: compilatorul devine prima linie de apărare. **Ștergeți doar acele linii** și notați fiecare în README, cu eroarea primită.

### Constrângeri

| Constrângere | Detaliu |
|---|---|
| Verificările nu se modifică | Partea 2 din `Program.cs` rămâne identică (se verifică prin `git diff`). |
| Partea 1: doar ștergeri | Se șterg **numai** liniile refuzate de compilator. Nu se adaugă și nu se modifică nimic. În sală, liniile șterse se pun înapoi: fiecare trebuie să dea o eroare de compilare. |
| Utilizarea corectă compilează nemodificată | `pump.Flow = 30`, `pump.Run(2)`, `pump.CalibrationPoints[3]` rămân valide. Trecerea de la câmp la proprietate nu schimbă codul utilizatorului. |
| Excepții, nu valori corectate în tăcere | Un debit invalid nu se „rotunjește" la limită: se refuză cu excepție. |
| Fără câmpuri `public` | Niciun câmp `public` în `DosingPump`. |

## 3. Livrabile

1. **Repository Git** cu proiectul .NET complet. Un commit cu codul inițial, apoi commit-urile voastre.
2. **README.md** cu:
   - răspunsurile la cele 4 întrebări din secțiunea 1;
   - diagramele UML înainte / după;
   - tabelul de mai jos, completat;
   - lista liniilor șterse din Partea 1, cu eroarea de compilare primită pentru fiecare;
   - ieșirea programului (12 × `OK`);
   - o propoziție: *de ce `readonly` nu a fost suficient pentru tabela de calibrare*.

| Membru | Cine citește | Cine modifică | Valori valide | Implementare aleasă |
|---|---|---|---|---|
| `Tag` | | | | |
| `MaxFlow` | | | | |
| `Flow` | | | | |
| `IsRunning` | | | | |
| `CalibrationPoints` | | | | |
| `TotalDosed` | | | | |

3. Link-ul trimis cu cel puțin 24 de ore înainte de curs.

## 4. Prezentarea (5 minute, cronometrate)

| Interval | Conținut |
|---|---|
| 0:00 – 1:30 | Tabelul membrilor și diagrama UML după refactorizare. |
| 1:30 – 3:00 | **Demo live:** `Program.cs` inițial (9 × `BUG`) și cel final (12 × `OK`). Explicați verificările 7 și 8. |
| 3:00 – 5:00 | Întrebări. Veți primi în sală o cerință nouă și veți explica, fără să scrieți cod, ce s-ar schimba. |

## 5. Grila de evaluare (10 puncte)

| Criteriu | Punctaj |
|---|---|
| Încapsulare corectă: câmpuri private, validare în setter și constructor, proprietăți doar pentru citire / calculate, copie defensivă + expunere doar pentru citire a tabelei | 3 p |
| Tabelul membrilor și diagramele UML; liniile șterse justificate prin erorile compilatorului | 2 p |
| Răspunsul la cerința nouă primită în sală | 3 p |
| 12 × `OK`, verificările nemodificate; README complet | 1 p |
| Încadrarea în cele 5 minute | 1 p |
