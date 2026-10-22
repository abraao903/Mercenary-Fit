namespace Projeto.Models;

public class Aluno
{
    public int Id { get; set; }
    
    public int PlanoId { get; set; }

    public Plano Plano { get; set; } = null!;

    public string Nome{ get; set; } = string.Empty;

    public int Idade{ get;set; }

    public int TotalHoras { get; set; }

    public DateTime DataMatricula { get; set; }
}