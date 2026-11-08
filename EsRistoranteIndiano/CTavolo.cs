using System;

namespace EsRistoranteIndiano;

public class CTavolo
{
    public int Numero { get; set; }
    public int Posti { get; set; }
    public bool Occupato { get; set; }
    public int NumeroPostiOccupati { get; set; }
    public COrdine Ordine { get; set; }

    public CTavolo()
    {
        Ordine = new COrdine();
    }
    public CTavolo(int numero, int posti)
    {
        Numero = numero;
        Posti = posti;
        NumeroPostiOccupati = 0;
        Occupato = false;
        Ordine = new COrdine();
    }

    public void AggiungiPersone(int persone)
    {
        if (Occupato)
        {
            throw new ArgumentException("Il tavolo è già occupato");
        }

        if (persone <= 0)
        {
            throw new ArgumentException("Numero di persone non valido");
        }

        if (persone > Posti)
        {
            throw new ArgumentException("Non ci sono abbastanza posti disponibili");
        }

        NumeroPostiOccupati = persone;
        Occupato = true;
    }

    public void Libera()
    {
        NumeroPostiOccupati = 0;
        Occupato = false;
        Ordine = new COrdine();
    }

    public override string ToString()
    {
        string occup = Occupato ? "Si" : "No";
        return $"Tavolo {Numero} - PostiMax: {Posti} - PostiOccupati: {NumeroPostiOccupati} - Occupato: {occup}";
    }
}