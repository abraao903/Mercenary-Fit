namespace Projeto.Models;

public class Plano
{
    public int Id {get; set;}
    public string Nome{get; set;} = string.Empty;
    public int DuracaoDias {get; set;}
    public decimal Preco {get; set;}
}