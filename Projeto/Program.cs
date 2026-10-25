using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Models;
using Projeto.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data source=mercenaryfit.db"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.MapGet("/", () => "Mercenary Fit API");


// API/PLANO/LISTAR = Contem a lista dos planos
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

// listar com plano da academia/ metodo especial para 
// não gerar erros de algo infinito
app.MapGet("/api/aluno/listar", ([FromServices] AppDbContext ctx) =>
{
    var alunos = ctx.Alunos
        .Include(a => a.Plano)
        .Select(a => new 
        {
            a.Id,
            a.Nome,
            a.Idade,
            a.TotalHoras,
            a.DataMatricula,
            Plano = new 
            {
                a.Plano.Id,
                a.Plano.Nome,
                a.Plano.Preco,
                a.Plano.DuracaoDias
            }
        })
        .ToList();

    return Results.Ok(alunos);
});


//CADASTRO DO ALUNO
app.MapPost("/api/aluno/cadastro", ([FromBody]Aluno aluno,[FromServices] AppDbContext ctx) => 
{
    var planoExiste = ctx.Planos.Any(p => p.Id== aluno.PlanoId );
    if (!planoExiste) return Results.BadRequest("O plano informado não existe");

    ctx.Alunos.Add(aluno);
    ctx.SaveChanges();
    return Results.Created("", aluno);
});

// Busca aluno por Id
app.MapGet("/api/aluno/{id:int}", ([FromServices] AppDbContext ctx, int id ) =>
{
    var aluno = ctx.Alunos.Include(a => a.Plano).FirstOrDefault(a => a.Id == id);
    if (aluno is null) return Results.NotFound("Aluno não encontrado.");
    return Results.Ok(aluno);

});


//ATUALIZA ALUNO
app.MapPut("/api/aluno/{id:int}", ([FromServices] AppDbContext ctx, int id, [FromBody] Aluno alunoAlterado ) =>
{
    var aluno = ctx.Alunos.Find(id);
    if (aluno is null) return Results.NotFound("Aluno não encontrado");

    aluno.Nome = alunoAlterado.Nome;
    aluno.Idade = alunoAlterado.Idade;
    aluno.TotalHoras = alunoAlterado.TotalHoras;
    aluno.PlanoId = alunoAlterado.PlanoId;
    
    ctx.SaveChanges();
    return Results.Ok(aluno);
});

//DELETA O ALUNO
app.MapDelete("/api/aluno/{id:int}", ([FromServices] AppDbContext ctx, int id) => 
{
    var aluno = ctx.Alunos.Find(id);
    if (aluno is null) return Results.NotFound("Aluno desconhecido");

    ctx.Alunos.Remove(aluno);
    ctx.SaveChanges();
    return Results.NoContent();

});


//CADASTRO DO INSTRUTOR
app.MapPost("/api/instrutor/cadastro", async ([FromBody] Instrutor instrutor,[FromServices] AppDbContext ctx) =>
{
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
app.MapPut("/api/instrutor/{id:int}", async (int id, [FromBody] Instrutor dados, [FromServices] AppDbContext ctx) =>
{
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
app.MapDelete("/api/instrutor/{id:int}", async (
    int id,
    [FromServices] AppDbContext ctx) =>
{
    var instrutor = await ctx.Instrutores.FindAsync(id);

    if (instrutor is null)
        return Results.NotFound("Instrutor não encontrado.");

    ctx.Instrutores.Remove(instrutor);
    await ctx.SaveChangesAsync();

    return Results.NoContent();
});


//FICHA DE TREINO 
app.MapPost("/api/ficha/cadastro", ([FromBody] FichaDeTreino ficha, [FromServices] AppDbContext ctx) =>
{
    var alunoExiste = ctx.Alunos.Any(a => a.Id == ficha.AlunoId);
    var instrutorExiste = ctx.Instrutores.Any(i => i.Id == ficha.InstrutorId);

    if (!alunoExiste || !instrutorExiste) return Results.BadRequest("Aluno ou instrutor não existe");

    ctx.FichaDeTreinos.Add(ficha);
    ctx.SaveChanges();
    return Results.Created("", ficha);

});


// Metodo de listar para excluir pontos nulos dentro do listar
// e deixar mais compacto/ tudo certinho
// Utiliza .Select para estruturar o JSON de resposta de forma limpa, 
// removendo ciclos e campos nulos desnecessários.
app.MapGet("/api/ficha/listar", ([FromServices] AppDbContext ctx) =>
{
    var fichas = ctx.FichaDeTreinos
        .Include(f => f.Aluno)
            .ThenInclude(a => a!.Plano)
        .Include(f => f.Instrutor)
        .Select(f => new
        {
            f.Id,
            f.Titulo,
            f.Descricao,
            Aluno = new
            {
                f.Aluno!.Id,
                f.Aluno.Nome,
                f.Aluno.Idade,
                Plano = new
                {
                    f.Aluno.Plano.Id,
                    f.Aluno.Plano.Nome,
                    f.Aluno.Plano.Preco
                }
            },
            Instrutor = new
            {
                f.Instrutor!.Id,
                f.Instrutor.Nome,
                f.Instrutor.Especialidade,
                f.Instrutor.Email
            }
        })
        .ToList();

    return Results.Ok(fichas);
});


app.Run();
