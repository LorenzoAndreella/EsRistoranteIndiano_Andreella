using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EsRistoranteIndiano;

public partial class MainWindow : Window
{
    private CGestioneTavoli gestione = new CGestioneTavoli();

    public MainWindow()
    {
        InitializeComponent();

        gestione.Carica();
        lstMenu.ItemsSource = new List<CPiatto>(gestione.Menu.Piatti);
        AggiornaTutto();
    }

    private void AggiornaTutto()
    {
        AggiornaTavoli();
        AggiornaOrdine();
        AggiornaStorico();
    }

    private void AggiornaTavoli()
    {
        int selezionato = lstTavoli.SelectedIndex;
        lstTavoli.ItemsSource = new List<CTavolo>(gestione.Tavoli);
        lstTavoli.SelectedIndex = selezionato;
    }

    private void AggiornaOrdine()
    {
        if (lstTavoli.SelectedItem is not CTavolo)
        {
            txtTavoloSelezionato.Text = "Nessun tavolo selezionato";
            lstOrdine.ItemsSource = null;
            txtTotale.Text = "";
            return;
        }

        CTavolo t = (CTavolo)lstTavoli.SelectedItem;
        txtTavoloSelezionato.Text = $"Ordine, Tavolo {t.Numero}";
        if (!t.Occupato)
        {
            txtTavoloSelezionato.Text += " (libero)";
        }

        lstOrdine.ItemsSource = gestione.RigheOrdine(t.Numero);
        txtTotale.Text = $"Totale: {gestione.CalcolaTot(t.Ordine):F2} €";
    }

    private void AggiornaStorico()
    {
        lstStorico.ItemsSource = new List<COrdine>(gestione.Storico);
        txtTotaleGiornata.Text = $"Giornata: {gestione.CalcolaTot(gestione.Storico):F2} €";
    }

    private void SelezioneTavoli(object? sender, SelectionChangedEventArgs e)
    {
        txtMessaggio.Text = "";
        pnlConto.IsVisible = false;
        AggiornaOrdine();
    }

    private void OccupaTavolo(object? sender, RoutedEventArgs e)
    {
        txtMessaggio.Text = "";

        try
        {
            if (lstTavoli.SelectedItem is CTavolo t)
            {
                if (NmrPersone.Value is null || NmrPersone.Value <= 0)
                {
                    throw new ArgumentException("Numero di persone non valido");
                }

                int persone = (int)NmrPersone.Value;

                gestione.OccupaTavolo(t.Numero, persone);
                AggiornaTutto();
                NmrPersone.Value = 1;

            }
            else
            {
                throw new ArgumentException("Seleziona un tavolo");
            }
        }
        catch (Exception ex)
        {
            txtMessaggio.Text = ex.Message;
        }
    }

    private void RimuoviPiatto(object? sender, RoutedEventArgs e)
    {
        txtMessaggio.Text = "";
        try
        {
            if (lstTavoli.SelectedItem is CTavolo t)
            {
                if (lstOrdine.SelectedIndex >= 0)
                {
                    gestione.RimuoviPiatto(t.Numero, lstOrdine.SelectedIndex);
                    AggiornaOrdine();
                }
                else
                {
                    throw new ArgumentException("Seleziona un piatto dell'ordine");
                }
            }
            else
            {
                throw new ArgumentException("Seleziona un tavolo");
            }
        }
        catch (Exception ex)
        {
            txtMessaggio.Text = ex.Message;
        }
    }

    private void ChiudiConto(object? sender, RoutedEventArgs e)
    {
        txtMessaggio.Text = "";
        try
        {
            if (lstTavoli.SelectedItem is CTavolo t)
            {
                if (!t.Occupato)
                {
                    throw new ArgumentException("Tavolo non occupato");
                }

                if (t.Ordine.Piatti.Count == 0)
                {
                    throw new ArgumentException("Ordine vuoto");
                }

                txtRiepilogo.Text = gestione.Riepilogo(t.Numero);
                pnlConto.IsVisible = true;
            }
            else
            {
                throw new ArgumentException("Seleziona un tavolo");
            }
        }
        catch (Exception ex)
        {
            txtMessaggio.Text = ex.Message;
        }
    }

    private void ConfermaPagamento(object? sender, RoutedEventArgs e)
    {
        txtMessaggio.Text = "";
        try
        {
            if (lstTavoli.SelectedItem is CTavolo t)
            {
                gestione.ChiudiConto(t.Numero);
                pnlConto.IsVisible = false;
                AggiornaTutto();
            }
            else
            {
                throw new ArgumentException("Seleziona un tavolo");
            }
        }
        catch (Exception ex)
        {
            txtMessaggio.Text = ex.Message;
        }
    }

    private void AnnullaOrdine(object? sender, RoutedEventArgs e)
    {
        pnlConto.IsVisible = false;
    }

    private void BtnAggiungiPiatto(object? sender, RoutedEventArgs e)
    {
        txtMessaggio.Text = "";

        try
        {
            if (lstTavoli.SelectedItem is CTavolo t)
            {
                if (lstMenu.SelectedItem is CPiatto p)
                {
                    if (NmrQuantita.Value is null || NmrQuantita.Value <= 0)
                    {
                        throw new ArgumentException("Quantità non valida");
                    }

                    int quantita = (int)NmrQuantita.Value;
                    gestione.AggiungiPiatto(t.Numero, p.Nome, quantita);
                    AggiornaOrdine();
                    NmrQuantita.Value = 1;
                }
                else
                {
                    throw new ArgumentException("Seleziona un piatto");
                }
            }
            else
            {
                throw new ArgumentException("Seleziona un tavolo");
            }
        }
        catch (Exception ex)
        {
            txtMessaggio.Text = ex.Message;
        }
    }
}