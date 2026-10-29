// ============================================================================
//  Stația de tratare a apei – pompa dozatoare de hipoclorit P-301.
//
//  PARTEA 1 – codul colegilor (HMI, rețete, raportare, mentenanță).
//             Puteți ȘTERGE doar liniile pe care compilatorul le refuză după
//             refactorizarea clasei DosingPump. Nu adăugați și nu modificați nimic.
//  PARTEA 2 – verificările. NU le modificați. Toate trebuie să afișeze OK.
// ============================================================================

// ---------- PARTEA 1: codul colegilor ----------

// Punerea în funcțiune: tabela de calibrare măsurată de tehnician.
List<double> calibration = new() { 12.1, 24.6, 37.0, 49.0 };
DosingPump pump = new("P-301", 50, calibration);

// HMI: operatorul pornește dozarea la 30 l/h.
pump.Flow = 30;
pump.IsRunning = true;
double acceptedFlow = pump.Flow;

// HMI: greșeli de tastare ale operatorului.
bool rejectedNegative = Rejected(() => pump.Flow = -5);
bool rejectedTooHigh = Rejected(() => pump.Flow = 80);

// Modulul de rețete: „rețeta nouă cere 60 l/h, măresc maximul".
pump.MaxFlow = 60;

pump.Run(2);   // 2 ore de funcționare

// Modulul de raportare: „resetez contorul la schimbul de tură".
pump.TotalDosed = 0;

// Achiziții: refolosește lista pentru o pompă nouă, cu alte valori.
calibration[3] = 99.0;

// Mentenanță: „recalibrez mai târziu, deocamdată golesc tabela".
pump.CalibrationPoints.Clear();

// HMI copiat de la linia 2, „uitat" un rând.
pump.Tag = "P-302";

// HMI: operatorul oprește dozarea punând debitul pe 0.
pump.Flow = 0;
bool stoppedByZero = !pump.IsRunning;

// HMI: operatorul repornește la 20 l/h.
pump.Flow = 20;
bool runningAgain = pump.IsRunning;
pump.Run(1);   // încă o oră

// Un modul nou încearcă să creeze o pompă cu date greșite.
bool badPumpRejected = Rejected(() => new DosingPump("", -10, new List<double>()));

// ---------- PARTEA 2: verificări (NU MODIFICAȚI) ----------
int failures = 0;

Check("1. Debitul valid de 30 l/h a fost acceptat", () => acceptedFlow == 30);
Check("2. Debitul negativ a fost refuzat", () => rejectedNegative);
Check("3. Debitul peste maxim (80 > 50 l/h) a fost refuzat", () => rejectedTooHigh);
Check("4. Debitul maxim a rămas cel din fișa tehnică (50 l/h)", () => pump.MaxFlow == 50);
Check("5. Eticheta pompei a rămas P-301", () => pump.Tag == "P-301");
Check("6. Tabela de calibrare are tot 4 puncte", () => pump.CalibrationPoints.Count == 4);
Check("7. Modificarea listei originale nu schimbă pompa (49.0 l/h la 100 %)",
      () => pump.CalibrationPoints[3] == 49.0);
Check("8. Debit 0 înseamnă pompă oprită", () => stoppedByZero);
Check("9. Debit 20 l/h înseamnă pompă pornită", () => runningAgain);
Check("10. Totalizatorul: 30 l/h × 2 h + 20 l/h × 1 h = 80 l", () => pump.TotalDosed == 80);
Check("11. Constructorul refuză o pompă fără etichetă și cu debit maxim negativ",
      () => badPumpRejected);
Check("12. Debitul curent este 20 l/h", () => pump.Flow == 20);

Console.WriteLine(failures == 0 ? "\nToate verificările au trecut." : $"\n{failures} verificări au eșuat.");

void Check(string description, Func<bool> condition)
{
  bool ok;
  try { ok = condition(); }
  catch (Exception) { ok = false; }   // o verificare care aruncă excepție a eșuat
  Console.WriteLine($"{(ok ? "OK " : "BUG")}  {description}");
  if (!ok) failures++;
}

bool Rejected(Action action)
{
  try { action(); return false; }
  catch (ArgumentException) { return true; }   // include ArgumentOutOfRangeException
}
