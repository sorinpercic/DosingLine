// Pompă dozatoare de hipoclorit (NaOCl) – stația de tratare a apei, treapta de dezinfecție.
// Scrisă „rapid, ca să meargă HMI-ul" de un coleg care între timp a plecat din echipă.
//
// Fișa tehnică: debitul maxim al pompei este fix (din construcție).
// Tabela de calibrare: debitul măsurat (l/h) la 25 %, 50 %, 75 % și 100 % din cursă.
public class DosingPump
{
    public string Tag;                        // ex. P-301 (eticheta din schema P&ID)
    public double MaxFlow;                    // l/h, din fișa tehnică
    public double Flow;                       // l/h, debitul cerut acum
    public bool IsRunning;                    // pompa dozează?
    public List<double> CalibrationPoints;    // 4 valori, l/h
    public double TotalDosed;                 // litri dozați de la punerea în funcțiune

    public DosingPump(string tag, double maxFlow, List<double> calibrationPoints)
    {
        Tag = tag;
        MaxFlow = maxFlow;
        CalibrationPoints = calibrationPoints;
    }

    // Simulează funcționarea pompei pentru un număr de ore.
    public void Run(double hours)
    {
        if (IsRunning)
        {
            TotalDosed += Flow * hours;
        }
    }

    public override string ToString()
    {
        return $"{Tag}: {Flow} / {MaxFlow} l/h, total {TotalDosed} l";
    }
}
