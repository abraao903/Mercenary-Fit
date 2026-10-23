using Microsoft.EntityFrameworkCore;
using Projeto.Models;

namespace Projeto.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Plano> Planos => Set<Plano>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Instrutor> Instrutores => Set<Instrutor>();
    public DbSet<FichaDeTreino> FichasDeTreino => Set<FichaDeTreino>();
    public DbSet<Exercicio> Exercicios => Set<Exercicio>();
}