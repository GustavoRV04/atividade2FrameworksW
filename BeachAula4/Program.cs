using BeachAula4.Data;
using BeachAula4.Entidades;
using BeachAula4.Interfaces;
using BeachAula4.Repositorios;
using BeachAula4.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco e Injeção de Dependências
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IQuadraRepository, QuadraRepository>();
builder.Services.AddScoped<IQuadraService, QuadraService>();

var app = builder.Build();
app.UseHttpsRedirection();

// 1. READ (GET) - Listar todas as quadras ----------------------------------------------------
app.MapGet("/api/quadras", async (IQuadraService service) =>
{
    var quadras = await service.ListarAsync();
    return Results.Ok(quadras);
});

// 2. READ (GET) - Buscar quadras ativas -------------------------------------------------------
app.MapGet("/api/quadras/ativas", async (IQuadraService service) =>
{
    var quadras = await service.ListarAtivasAsync();
    return Results.Ok(quadras);
});

// 3. READ (GET) - Buscar uma quadra pelo ID --------------------------------------------------
app.MapGet("/api/quadras/{id:int}", async (int id, IQuadraService service) =>
{
    var quadra = await service.BuscarPorIdAsync(id);
    return quadra == null ? Results.NotFound() : Results.Ok(quadra);
});

// 4. CONSULTA AVANÇADA LINQ (GET) - Ativas até um valor máximo, ordenadas por preço ----------
// Exemplo de chamada: GET /api/quadras/por-preco?precoMaximo=150
app.MapGet("/api/quadras/por-preco", async (decimal precoMaximo, IQuadraService service) =>
{
    var quadras = await service.ListarAtivasPorPrecoMaximoAsync(precoMaximo);
    return Results.Ok(quadras);
});

// 5. CREATE (POST) - Cadastrar uma nova quadra -----------------------------------------------
app.MapPost("/api/quadras", async (Quadra quadra, IQuadraService service) =>
{
    try
    {
        await service.AdicionarAsync(quadra);
        return Results.Created($"/api/quadras/{quadra.Id}", quadra);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { mensagem = ex.Message });
    }
});

// 6. UPDATE (PUT) - Atualizar uma quadra ------------------------------------------------------
app.MapPut("/api/quadras/{id:int}", async (int id, Quadra quadra, IQuadraService service) =>
{
    quadra.Id = id;
    var atualizado = await service.AtualizarAsync(quadra);

    return atualizado ? Results.NoContent() : Results.NotFound();
});

// 7. DELETE (DELETE) - Excluir uma quadra ----------------------------------------------------
app.MapDelete("/api/quadras/{id:int}", async (int id, IQuadraService service) =>
{
    var excluido = await service.ExcluirAsync(id);

    return excluido ? Results.NoContent() : Results.NotFound();
});

app.Run();