using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Models;
using Projeto.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data source=mercenaryfit.db"));

var app = builder.Build();

app.MapGet("/", () => "Mercenary Fit API");

app.MapPost("/api/plano/cadastro", ([FromBody]Plano plano,[FromServices] AppDbContext ctx) => 
{

    ctx.Planos.Add(plano);

    ctx.SaveChanges();

    return Results.Created("", plano);
});

app.MapGet("/api/plano/listar", ([FromServices] AppDbContext ctx) =>
{
    // ve se tem algo cadastrado 
    if(!ctx.Planos.Any())
    {
        return Results.NotFound("Nenhum plano cadastrado");
    }

    return Results.Ok(ctx.Planos.ToList());
});

app.Run();
