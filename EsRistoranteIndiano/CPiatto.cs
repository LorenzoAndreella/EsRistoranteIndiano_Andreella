namespace EsRistoranteIndiano;

public enum ECategoria
{
    antipasto,
    piattoPrincipale,
    contorno,
    pane,
    dolce,
    bevanda
}

public class CPiatto
{
    public string Nome { get; }
    protected ECategoria Categoria { get; }
    public double Prezzo { get; }

    public CPiatto(string nome, ECategoria categoria, double prezzo)
    {
        Nome = nome;
        Categoria = categoria;
        Prezzo = prezzo;
    }

    public override string ToString()
    {
        return $"{Nome} - {Categoria} - {Prezzo:F2} €";
    }
}