using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;

namespace Projeto.Endpoints;

public static class ExercicioEndpoints
{
    public static void MapExercicioEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/exercicio").WithTags("Exercicio");

        group.MapGet("/listar", async ([FromServices] AppDbContext ctx) =>
        {
            var exercicios = await ctx.Exercicios.ToListAsync();
            return Results.Ok(exercicios);
        });

        group.MapGet("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var ex = await ctx.Exercicios.FindAsync(id);
            return ex is not null ? Results.Ok(ex) : Results.NotFound();
        });

        group.MapPost("/cadastro", async ([FromBody] Exercicio ex, [FromServices] AppDbContext ctx) =>
        {
            var ficha = await ctx.FichasDeTreino.FindAsync(ex.FichaDeTreinoId);
            if (ficha is null) return Results.BadRequest("Ficha de treino não encontrada.");
            ctx.Exercicios.Add(ex);
            await ctx.SaveChangesAsync();
            return Results.Created($"/api/exercicio/{ex.Id}", ex);
        });

        group.MapPut("/{id:int}", async (int id, [FromBody] Exercicio exAtualizado, [FromServices] AppDbContext ctx) =>
        {
            var ex = await ctx.Exercicios.FindAsync(id);
            if (ex is null) return Results.NotFound();
            ex.Nome = exAtualizado.Nome;
            ex.GrupoMuscular = exAtualizado.GrupoMuscular;
            ex.Series = exAtualizado.Series;
            ex.Repeticoes = exAtualizado.Repeticoes;
            ex.Divisao = exAtualizado.Divisao;
            ex.FichaDeTreinoId = exAtualizado.FichaDeTreinoId;
            await ctx.SaveChangesAsync();
            return Results.Ok(ex);
        });

        group.MapDelete("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var ex = await ctx.Exercicios.FindAsync(id);
            if (ex is null) return Results.NotFound();
            ctx.Exercicios.Remove(ex);
            await ctx.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}