using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace EsRistoranteIndiano;

public class CGestioneTavoli
{
    private readonly string fileTavoli = "tavoli.json";
    private readonly string fileStorico = "storico.json";
    public List<CTavolo> Tavoli { get; }
    public List<COrdine> Storico { get; }
    public CMenu Menu { get; }

    public CGestioneTavoli()
    {
        Tavoli = new List<CTavolo>();
        Storico = new List<COrdine>();
        Menu = new CMenu();
    }

    public void AggiungiTavolo(int posti)
    {
        Tavoli.Add(new CTavolo(Tavoli.Count, posti));
    }

    public CTavolo CercaTavolo(int numero)
    {
        foreach (var t in Tavoli)
        {
            if (t.Numero == numero)
            {
                return t;
            }
        }

        throw new ArgumentException("Tavolo non trovato");
    }

    public void OccupaTavolo(int numero, int persone)
    {
        CercaTavolo(numero).AggiungiPersone(persone);
        Salva();
    }

    public void AggiungiPiatto(int numero, string nomePiatto, int quantita)
    {
        CTavolo t = CercaTavolo(numero);
        if (!t.Occupato)
        {
            throw new ArgumentException("Tavolo non occupato");
        }

        Menu.TrovaPiatto(nomePiatto);
        t.Ordine.AggiungiAllOrdine(nomePiatto, quantita);
        Salva();
    }

    public void RimuoviPiatto(int numero, int riga)
    {
        CTavolo t = CercaTavolo(numero);
        if (!t.Occupato)
        {
            throw new ArgumentException("Tavolo non occupato");
        }

        t.Ordine.RimuoviPiatto(riga);
        Salva();
    }

    public List<string> RigheOrdine(int numero)
    {
        CTavolo t = CercaTavolo(numero);
        List<string> righe = new List<string>();
        foreach (var coppia in t.Ordine.Piatti)
        {
            CPiatto p = Menu.TrovaPiatto(coppia.Key);
            double subTot = p.Prezzo * coppia.Value;
            righe.Add($"{coppia.Value} x {p.Nome} ({p.Prezzo:F2} € cad.) = {subTot:F2} €");
        }

        return righe;
    }

    public string Riepilogo(int numero)
    {
        CTavolo t = CercaTavolo(numero);
        string s = $"Tavolo {t.Numero} occupato da {t.NumeroPostiOccupati} persone\n";
        foreach (var riga in RigheOrdine(numero))
        {
            s += riga + "\n";
        }

        s += $"Totale da pagare: {CalcolaTot(t.Ordine):F2} €\n";
        return s;
    }

    public COrdine ChiudiConto(int numero)
    {
        CTavolo t = CercaTavolo(numero);
        if (!t.Occupato)
        {
            throw new ArgumentException("Tavolo non occupato");
        }

        if (t.Ordine.Piatti.Count == 0)
        {
            throw new ArgumentException("Nessun piatto ordinato");
        }

        COrdine concluso = t.Ordine;
        concluso.NumeroTavolo = t.Numero;
        concluso.Persone = t.NumeroPostiOccupati;
        concluso.DataOra = DateTime.Now;
        concluso.TotalePagato = CalcolaTot(concluso);

        Storico.Add(concluso);
        t.Libera();
        Salva();
        return concluso;
    }

    public double CalcolaTot(COrdine ordine)
    {
        double tot = 0;
        foreach (var coppia in ordine.Piatti)
        {
            CPiatto p = Menu.TrovaPiatto(coppia.Key);
            tot += p.Prezzo * coppia.Value;
        }

        return tot;
    }

    public double CalcolaTot(List<COrdine> ordini)
    {
        double tot = 0;
        foreach (var o in ordini)
        {
            if (o.DataOra.Day == DateTime.Now.Day)
            {
                tot += o.TotalePagato;
            }
        }

        return tot;
    }

    public void Carica()
    {
        Tavoli.Clear();
        Storico.Clear();

        if (File.Exists(fileTavoli))
        {
            string json = File.ReadAllText(fileTavoli);
            var caricati = JsonSerializer.Deserialize<List<CTavolo>>(json);
            if (caricati != null)
            {
                foreach (var t in caricati)
                {
                    Tavoli.Add(t);
                }
            }
        }

        if (File.Exists(fileStorico))
        {
            string json = File.ReadAllText(fileStorico);
            var caricati = JsonSerializer.Deserialize<List<COrdine>>(json);
            if (caricati != null)
            {
                foreach (var o in caricati)
                {
                    Storico.Add(o);
                }
            }
            return;
        }
        
        int[] posti = { 2, 2, 4, 4, 6, 6, 8, 10 };
        foreach (var p in posti)
        {
            AggiungiTavolo(p);
        }
    }

    public void Salva()
    {
        string json = JsonSerializer.Serialize(Tavoli);
        File.WriteAllText(fileTavoli, json);

        json = JsonSerializer.Serialize(Storico);
        File.WriteAllText(fileStorico, json);
    }
}