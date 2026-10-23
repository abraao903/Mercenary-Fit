using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;

namespace Projeto.Endpoints;

public static class AlunoEndpoints
{
    public static void MapAlunoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/aluno").WithTags("Aluno");

        group.MapGet("/listar", async ([FromServices] AppDbContext ctx) =>
        {
            var alunos = await ctx.Alunos.Include(a => a.Plano).ToListAsync();
            return Results.Ok(alunos);
        });

        group.MapGet("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var aluno = await ctx.Alunos.Include(a => a.Plano).FirstOrDefaultAsync(a => a.Id == id);
            return aluno is not null ? Results.Ok(aluno) : Results.NotFound();
        });

        group.MapPost("/cadastro", async ([FromBody] Aluno aluno, [FromServices] AppDbContext ctx) =>
        {
            var plano = await ctx.Planos.FindAsync(aluno.PlanoId);
            if (plano is null)
                return Results.BadRequest("PlanoId inválido.");

            aluno.DataMatricula = DateTime.Now;
            ctx.Alunos.Add(aluno);
            await ctx.SaveChangesAsync();
            return Results.Created($"/api/aluno/{aluno.Id}", aluno);
        });

        group.MapPut("/{id:int}", async (int id, [FromBody] Aluno alunoAtualizado, [FromServices] AppDbContext ctx) =>
        {
            var aluno = await ctx.Alunos.FindAsync(id);
            if (aluno is null) return Results.NotFound();
            aluno.Nome = alunoAtualizado.Nome;
            aluno.Idade = alunoAtualizado.Idade;
            aluno.TotalHoras = alunoAtualizado.TotalHoras;
            aluno.PlanoId = alunoAtualizado.PlanoId;
            await ctx.SaveChangesAsync();
            return Results.Ok(aluno);
        });

        group.MapDelete("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var aluno = await ctx.Alunos.FindAsync(id);
            if (aluno is null) return Results.NotFound();
            ctx.Alunos.Remove(aluno);
            await ctx.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}