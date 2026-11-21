using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Models;
using Projeto.Data;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data source=mercenaryfit.db"));

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddCors(Options =>
{
   Options.AddPolicy("AllowFrontend", policy =>
   {
      policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod(); 
   }); 
});


var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapGet("/", () => "Mercenary Fit API");


// API/PLANO/LISTAR = Contem a lista dos planos
app.MapGet("/api/plano/listar", async ([FromServices] AppDbContext ctx) =>
{
    var planos = await ctx.Planos.ToListAsync();
    if (planos.Count == 0)
        return Results.NotFound("Nenhum plano cadastrado");

    return Results.Ok(planos);
});

app.MapPost("/api/plano/cadastro", async ([FromBody]Plano? plano,[FromServices] AppDbContext ctx) => 
{
    if (plano is null)
    {
        return Results.BadRequest("Dados do plano são obrigatorios");
    }
    if (string.IsNullOrWhiteSpace(plano.Nome))
    {
        return Results.BadRequest("O Nome do plano é obrigatório");
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
    await ctx.SaveChangesAsync();
    return Results.Created("", plano);
});

// listar com plano da academia/ metodo especial para 
// não gerar erros de algo infinito
app.MapGet("/api/aluno/listar", async ([FromServices] AppDbContext ctx) =>
{
    var alunos = await ctx.Alunos
        .Include(a => a.Plano)
        .Select(a => new 
        {
            a.Id,
            a.Nome,
            a.Idade,
            a.TotalHoras,
            a.DataMatricula,
            a.PlanoId,
            Plano = a.Plano == null ? null : new
            {
                a.Plano.Id,
                a.Plano.Nome,
                a.Plano.Preco,
                a.Plano.DuracaoDias
            }
        })
        .ToListAsync();

    return Results.Ok(alunos);
});


//CADASTRO DO ALUNO
app.MapPost("/api/aluno/cadastro", async ([FromBody]Aluno? aluno,[FromServices] AppDbContext ctx) => 
{
    if (aluno is null)
    {
        return Results.BadRequest("Dados do aluno são obrigatórios");
    }
    if (string.IsNullOrWhiteSpace(aluno.Nome))
    {
        return Results.BadRequest("O nome do aluno é obrigatorio");
    }
    if (aluno.Idade <= 0)
    {
        return Results.BadRequest("A idade deve ser maior que zero");
    }


    var planoExiste = await ctx.Planos.AnyAsync(p => p.Id== aluno.PlanoId );
    if (!planoExiste) return Results.BadRequest("O plano informado não existe");

    ctx.Alunos.Add(aluno);
    await ctx.SaveChangesAsync();
    return Results.Created($"/api/aluno/{aluno.Id}", aluno);
});

// Busca aluno por Id
app.MapGet("/api/aluno/{id:int}", async ([FromServices] AppDbContext ctx, int id ) =>
{
    var aluno = await ctx.Alunos.Include(a => a.Plano).FirstOrDefaultAsync(a => a.Id == id);
    if (aluno is null) return Results.NotFound("Aluno não encontrado.");
    return Results.Ok(aluno);

});


//ATUALIZA ALUNO
app.MapPut("/api/aluno/{id:int}", async ([FromServices] AppDbContext ctx, int id, [FromBody] Aluno? alunoAlterado ) =>
{
    if(alunoAlterado is null)
    {
        return Results.BadRequest("Dados para atualizar são obrigatorios");
    }

    var aluno = await ctx.Alunos.FindAsync(id);
    if (aluno is null) return Results.NotFound("Aluno não encontrado");

    if (string.IsNullOrWhiteSpace(alunoAlterado.Nome))
    {
        return Results.BadRequest("O nome do aluno é obrigatorio");
    }

    var planoExiste = await ctx.Planos.AnyAsync(p => p.Id == alunoAlterado.PlanoId);
    if (!planoExiste)
    {
        return Results.BadRequest("O plano informado não existe");
    }

    aluno.Nome = alunoAlterado.Nome;
    aluno.Idade = alunoAlterado.Idade;
    aluno.TotalHoras = alunoAlterado.TotalHoras;
    aluno.PlanoId = alunoAlterado.PlanoId;
    
    await ctx.SaveChangesAsync();
    return Results.Ok(aluno);
});

//DELETA O ALUNO
app.MapDelete("/api/aluno/{id:int}", async ([FromServices] AppDbContext ctx, int id) => 
{
    var aluno = await ctx.Alunos.FindAsync(id);
    if (aluno is null) return Results.NotFound("Aluno desconhecido");

    ctx.Alunos.Remove(aluno);
    await ctx.SaveChangesAsync();
    return Results.NoContent();
});


//CADASTRO DO INSTRUTOR
app.MapPost("/api/instrutor/cadastro", async ([FromBody] Instrutor? instrutor,[FromServices] AppDbContext ctx) =>
{
    if(instrutor is null)
    {
        return Results.BadRequest("Dados do instrutor são obrigatorios");
    }
    if(string.IsNullOrWhiteSpace(instrutor.Nome) || string.IsNullOrWhiteSpace(instrutor.Email))
    {
        return Results.BadRequest("Nome ou email em falta");
    }

    ctx.Instrutores.Add(instrutor);
    await ctx.SaveChangesAsync();
    return Results.Created($"/api/instrutor/{instrutor.Id}", instrutor);
});

//LISTAGEM DO INSTRUTOR
app.MapGet("/api/instrutor/listar", async ([FromServices] AppDbContext ctx) =>
{
    var instrutores = await ctx.Instrutores.ToListAsync();

    return Results.Ok(instrutores);
});


//PROCURAR INSTRUTOR PELO ID
app.MapGet("/api/instrutor/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
{
    var instrutor = await ctx.Instrutores.FindAsync(id);
    if (instrutor is null) return Results.NotFound("Instrutor não encontrado.");
    return Results.Ok(instrutor);

});

//Alterar instrutor pelo Id
app.MapPut("/api/instrutor/{id:int}", async (int id, [FromBody] Instrutor? dados, [FromServices] AppDbContext ctx) =>
{
    if(dados is null)
    {
        return Results.BadRequest("Dados do instrutor são obrigatorios");
    }
    
    var instrutor = await ctx.Instrutores.FindAsync(id);
    if (instrutor is null)
        return Results.NotFound("Instrutor não encontrado.");

    instrutor.Nome = dados.Nome;
    instrutor.Email = dados.Email;
    instrutor.Especialidade = dados.Especialidade;

    await ctx.SaveChangesAsync();
    return Results.Ok(instrutor);
    
});

//Instrutor deletado
app.MapDelete("/api/instrutor/{id:int}", async (int id, [FromServices] AppDbContext ctx) =>
{
    var instrutor = await ctx.Instrutores.FindAsync(id);
    if (instrutor is null)
        return Results.NotFound("Instrutor não encontrado.");

    ctx.Instrutores.Remove(instrutor);
    await ctx.SaveChangesAsync();

    return Results.NoContent();
});


//FICHA DE TREINO 
app.MapPost("/api/ficha/cadastro", async ([FromBody] FichaDeTreino? ficha, [FromServices] AppDbContext ctx) =>
{
    if(ficha is null)
    {
        return Results.BadRequest("Dados da ficha são obrigatorios");
    }
    if (string.IsNullOrWhiteSpace(ficha.Titulo))
    {
        return Results.BadRequest("O titulo da ficha é obrigatorio");
    }

    var alunoExiste = await ctx.Alunos.AnyAsync(a => a.Id == ficha.AlunoId);
    var instrutorExiste = await ctx.Instrutores.AnyAsync(i => i.Id == ficha.InstrutorId);

    if (!alunoExiste || !instrutorExiste) return Results.BadRequest("Aluno ou instrutor não existe");

    ctx.FichaDeTreinos.Add(ficha);
    await ctx.SaveChangesAsync();
    return Results.Created("", ficha);

});


// Metodo de listar para excluir pontos nulos dentro do listar
// e deixar mais compacto/ tudo certinho
// Utiliza .Select para estruturar o JSON de resposta de forma limpa, 
// removendo ciclos e campos nulos desnecessários.
app.MapGet("/api/ficha/listar", async ([FromServices] AppDbContext ctx) =>
{
    var fichas = await ctx.FichaDeTreinos
        .Include(f => f.Aluno)
            .ThenInclude(a => a!.Plano)
        .Include(f => f.Instrutor)
        .Select(f => new
        {
            f.Id,
            f.Titulo,
            f.Descricao,
            f.AlunoId,
            f.InstrutorId,
            Aluno = f.Aluno == null ? null : new
            {
                f.Aluno.Id,
                f.Aluno.Nome,
                f.Aluno.Idade,
                Plano = f.Aluno.Plano == null ? null : new
                {
                    f.Aluno.Plano.Id,
                    f.Aluno.Plano.Nome,
                    f.Aluno.Plano.Preco
                }
            },
            Instrutor = f.Instrutor == null ? null : new
            {
                f.Instrutor.Id,
                f.Instrutor.Nome,
                f.Instrutor.Especialidade,
                f.Instrutor.Email
            }
        })
        .ToListAsync();

    return Results.Ok(fichas);
});


app.Run();
