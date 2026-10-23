using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models;

public class FichaDeTreino
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título da ficha é obrigatório.")]
    [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    // Relacionamento com Aluno
    [Required]
    public int AlunoId { get; set; }
    
    [ForeignKey("AlunoId")]
    public Aluno? Aluno { get; set; }

    // Relacionamento com Instrutor
    [Required]
    public int InstrutorId { get; set; }

    [ForeignKey("InstrutorId")]
    public Instrutor? Instrutor { get; set; }
}
