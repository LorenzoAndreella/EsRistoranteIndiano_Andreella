using System;
using System.Collections.Generic;

namespace EsRistoranteIndiano;

public class CMenu
{
    public List<CPiatto> Piatti { get; }

    public CMenu()
    {
        Piatti = new List<CPiatto>();
        CaricaPiattiDefault();
    }
    
    public void AggiungiPiatto(string nome, ECategoria categoria, double prezzo)
    {
       
        foreach (var p in Piatti)
        {
           if (p.Nome == nome)
           {
               throw new ArgumentException("Il piatto è già presente nel menu");
           }
        }
        Piatti.Add(new CPiatto(nome, categoria, prezzo));
    }

    public CPiatto TrovaPiatto(string nome)
    {
        foreach (var p in Piatti)
        {
            if (p.Nome == nome)
            {
                return p;
            }
        }
        throw new ArgumentException("Piatto non presente nel menu");
    }

    public void CaricaPiattiDefault()
    {
        AggiungiPiatto("Samosa", ECategoria.antipasto, 4.50);
        AggiungiPiatto("Pakora", ECategoria.antipasto, 5.00);
        AggiungiPiatto("Chicken Tikka", ECategoria.antipasto, 8.50);
        AggiungiPiatto("Butter Chicken", ECategoria.piattoPrincipale, 11.00);
        AggiungiPiatto("Chicken Curry", ECategoria.piattoPrincipale, 12.50);
        AggiungiPiatto("Palak Paneer", ECategoria.piattoPrincipale, 10.50);
        AggiungiPiatto("Biryani", ECategoria.piattoPrincipale, 12.00);
        AggiungiPiatto("Dal", ECategoria.contorno, 5.40);
        AggiungiPiatto("Naan", ECategoria.pane, 2.50);
        AggiungiPiatto("Gulab Jamun", ECategoria.dolce, 4.00);
        AggiungiPiatto("Lassi", ECategoria.bevanda, 4.00);
        AggiungiPiatto("Acqua", ECategoria.bevanda, 2.00);
    }
}