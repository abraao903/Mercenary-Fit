using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Models;

namespace Projeto.Endpoints;

public static class PlanoEndpoints
{
    public static void MapPlanoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/plano").WithTags("Plano");

        group.MapGet("/listar", async ([FromServices] AppDbContext ctx) =>
        {
            var planos = await ctx.Planos.ToListAsync();
            return Results.Ok(planos);
        });

        group.MapGet("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var plano = await ctx.Planos.FindAsync(id);
            return plano is not null ? Results.Ok(plano) : Results.NotFound();
        });

        group.MapPost("/cadastro", async ([FromBody] Plano plano, [FromServices] AppDbContext ctx) =>
        {
            ctx.Planos.Add(plano);
            await ctx.SaveChangesAsync();
            return Results.Created($"/api/plano/{plano.Id}", plano);
        });

        group.MapPut("/{id:int}", async (int id, [FromBody] Plano planoAtualizado, [FromServices] AppDbContext ctx) =>
        {
            var plano = await ctx.Planos.FindAsync(id);
            if (plano is null) return Results.NotFound();
            plano.Nome = planoAtualizado.Nome;
            plano.DuracaoDias = planoAtualizado.DuracaoDias;
            plano.Preco = planoAtualizado.Preco;
            await ctx.SaveChangesAsync();
            return Results.Ok(plano);
        });

        group.MapDelete("/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
        {
            var plano = await ctx.Planos.FindAsync(id);
            if (plano is null) return Results.NotFound();
            ctx.Planos.Remove(plano);
            await ctx.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}