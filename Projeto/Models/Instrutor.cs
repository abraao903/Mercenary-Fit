using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models;

public class Instrutor
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email em formato inválido.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A especialidade é obrigatória.")]
    [StringLength(100, ErrorMessage = "A especialidade deve ter no máximo 100 caracteres.")]
    public string Especialidade { get; set; } = string.Empty;

    // Relacionamento com Fichas de Treino (um instrutor pode criar/gerenciar várias fichas)
    public ICollection<FichaDeTreino> FichasDeTreino { get; set; } = new List<FichaDeTreino>();
}
