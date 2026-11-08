using System;
using System.Collections.Generic;
using System.Linq;

namespace EsRistoranteIndiano;

public class COrdine
{
    public Dictionary<string, int> Piatti { get; set; }

    public int NumeroTavolo { get; set; }
    public int Persone { get; set; }
    public DateTime DataOra { get; set; }
    public double TotalePagato { get; set; }

    public COrdine()
    {
        Piatti = new Dictionary<string, int>();
    }

    public void AggiungiAllOrdine(string nomePiatto, int quantita)
    {
        if (quantita <= 0)
        {
            throw new ArgumentException("Quantita non valida");
        }

        if (Piatti.ContainsKey(nomePiatto))
        {
            Piatti[nomePiatto] += quantita;
        }
        else
        {
            Piatti.Add(nomePiatto, quantita);
        }
    }

    public void RimuoviPiatto(int indice)
    {
        if (indice < 0 || indice >= Piatti.Count)
        {
            throw new ArgumentException("Riga dell'ordine non valida");
        }

        string nome = Piatti.Keys.ElementAt(indice);
        Piatti.Remove(nome);
    }

    public override string ToString()
    {
        if (Piatti.Count == 0)
        {
            return "Ordine vuoto";
        }

        string ordineString = "";

        if (DataOra != DateTime.MinValue)
        {
            ordineString += $"Tavolo {NumeroTavolo}, {DataOra:dd/MM/yyyy HH:mm}, Clienti: {Persone}\n";
        }

        foreach (var coppia in Piatti)
        {
            ordineString += $"{coppia.Value} x {coppia.Key}\n";
        }

        if (DataOra != DateTime.MinValue)
        {
            ordineString += $"Totale pagato: {TotalePagato:F2} €\n";
        }

        return ordineString;
    }
}