using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;

namespace Projeto.Endpoints;

public static class InstrutorEndpoints
{
    public static void MapInstrutorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/instrutor").WithTags("Instrutor");

        group.MapGet("/listar", async ([FromServices] AppDbContext ctx) =>
        {
            var instrutores = await ctx.Instrutores.ToListAsync();
            return Results.Ok(instrutores);
        });

        group.MapGet("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var instrutor = await ctx.Instrutores.FindAsync(id);
            return instrutor is not null ? Results.Ok(instrutor) : Results.NotFound();
        });

        group.MapPost("/cadastro", async ([FromBody] Instrutor instrutor, [FromServices] AppDbContext ctx) =>
        {
            ctx.Instrutores.Add(instrutor);
            await ctx.SaveChangesAsync();
            return Results.Created($"/api/instrutor/{instrutor.Id}", instrutor);
        });

        group.MapPut("/{id:int}", async (int id, [FromBody] Instrutor instrutorAtualizado, [FromServices] AppDbContext ctx) =>
        {
            var instrutor = await ctx.Instrutores.FindAsync(id);
            if (instrutor is null) return Results.NotFound();
            instrutor.Nome = instrutorAtualizado.Nome;
            instrutor.Email = instrutorAtualizado.Email;
            instrutor.Especialidade = instrutorAtualizado.Especialidade;
            await ctx.SaveChangesAsync();
            return Results.Ok(instrutor);
        });

        group.MapDelete("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var instrutor = await ctx.Instrutores.FindAsync(id);
            if (instrutor is null) return Results.NotFound();
            ctx.Instrutores.Remove(instrutor);
            await ctx.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}