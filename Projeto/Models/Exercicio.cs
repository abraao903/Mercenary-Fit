using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models;

public class Exercicio
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    public string GrupoMuscular { get; set; } = string.Empty;

    public int Series { get; set; }

    public int Repeticoes { get; set; }

    public string Divisao { get; set; } = "A";

    public int FichaDeTreinoId { get; set; }

    [ForeignKey("FichaDeTreinoId")]
    public FichaDeTreino? FichaDeTreino { get; set; }
}

