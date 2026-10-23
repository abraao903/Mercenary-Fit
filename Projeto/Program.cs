using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Models;
using Projeto.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data source=mercenaryfit.db"));

var app = builder.Build();

app.MapGet("/", () => "Mercenary Fit API");


app.MapGet("/api/plano/listar", ([FromServices] AppDbContext ctx) =>
{
    // ve se tem algo cadastrado 
    if(!ctx.Planos.Any())
    {
        return Results.NotFound("Nenhum plano cadastrado");
    }

    return Results.Ok(ctx.Planos.ToList());
});

app.MapPost("/api/plano/cadastro", ([FromBody]Plano plano,[FromServices] AppDbContext ctx) => 
{
    // Verifica se é nulo, vazio ou so preenchido com espaços
    if (string.IsNullOrWhiteSpace(plano.Nome))
    {
        return Results.BadRequest("O nome do plano é obrigatorio e não pode ser nulo");
    }
    if(plano.Preco <= 0)
    {
        return Results.BadRequest("O valor do plano não pode ser menor doque zero");
    }
    if(plano.DuracaoDias <= 0)
    {
        return Results.BadRequest("O plano não pode ser cadastrado com zero e abaixo ");
    }

    ctx.Planos.Add(plano);

    ctx.SaveChanges();

    return Results.Created("", plano);
});


app.MapPost("/api/aluno/cadastro", ([FromBody]Aluno aluno,[FromServices] AppDbContext ctx) => 
{

    ctx.Alunos.Add(aluno);

    ctx.SaveChanges();

    return Results.Created("", aluno);
});



app.Run();
