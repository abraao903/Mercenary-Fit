using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;

namespace Projeto.Endpoints;

public static class FichaDeTreinoEndpoints
{
    public static void MapFichaDeTreinoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/fichadetreino").WithTags("FichaDeTreino");

        group.MapGet("/listar", async ([FromServices] AppDbContext ctx) =>
        {
            var fichas = await ctx.FichasDeTreino
                .Include(f => f.Aluno)
                .Include(f => f.Instrutor)
                .Include(f => f.Exercicios)
                .ToListAsync();
            return Results.Ok(fichas);
        });

        group.MapGet("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var ficha = await ctx.FichasDeTreino
                .Include(f => f.Aluno)
                .Include(f => f.Instrutor)
                .Include(f => f.Exercicios)
                .FirstOrDefaultAsync(f => f.Id == id);
            return ficha is not null ? Results.Ok(ficha) : Results.NotFound();
        });

        group.MapPost("/cadastro", async ([FromBody] FichaDeTreino ficha, [FromServices] AppDbContext ctx) =>
        {
            var aluno = await ctx.Alunos.FindAsync(ficha.AlunoId);
            if (aluno is null) return Results.BadRequest("Aluno não encontrado.");
            var instrutor = await ctx.Instrutores.FindAsync(ficha.InstrutorId);
            if (instrutor is null) return Results.BadRequest("Instrutor não encontrado.");
            ctx.FichasDeTreino.Add(ficha);
            await ctx.SaveChangesAsync();
            return Results.Created($"/api/fichadetreino/{ficha.Id}", ficha);
        });

        group.MapPut("/{id:int}", async (int id, [FromBody] FichaDeTreino fichaAtualizada, [FromServices] AppDbContext ctx) =>
        {
            var ficha = await ctx.FichasDeTreino.FindAsync(id);
            if (ficha is null) return Results.NotFound();
            ficha.Titulo = fichaAtualizada.Titulo;
            ficha.Descricao = fichaAtualizada.Descricao;
            ficha.Observacoes = fichaAtualizada.Observacoes;
            ficha.AlunoId = fichaAtualizada.AlunoId;
            ficha.InstrutorId = fichaAtualizada.InstrutorId;
            await ctx.SaveChangesAsync();
            return Results.Ok(ficha);
        });

        group.MapDelete("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var ficha = await ctx.FichasDeTreino.FindAsync(id);
            if (ficha is null) return Results.NotFound();
            ctx.FichasDeTreino.Remove(ficha);
            await ctx.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}