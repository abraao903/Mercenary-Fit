using System.Collections.Generic;
namespace Projeto.Models;

public class Plano
{
    public int Id { get; set; }
    public ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
    public string Nome{get; set;} = string.Empty;
    public int DuracaoDias {get; set;}
    public decimal Preco {get; set;}
}