using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Models;
using Projeto.Data;
using Projeto.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data source=mercenaryfit.db"));

var app = builder.Build();

app.MapGet("/", () => "Mercenary Fit API");

app.MapPlanoEndpoints();
app.MapInstrutorEndpoints();
app.MapAlunoEndpoints();
app.MapFichaDeTreinoEndpoints();
app.MapExercicioEndpoints();

app.Run();
