using Microsoft.EntityFrameworkCore;
using Projeto.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data source=mercenaryfit.db"));

var app = builder.Build();

app.MapGet("/", () => "Mercenary Fit API");

app.Run();
