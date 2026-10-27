using ApiCafe.Data;
using ApiCafe.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registra o DbContext
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite("Data Source=cafes.db"));

// Serve arquivos estáticos do frontend
builder.Services.AddDirectoryBrowser();

var app = builder.Build();

// Cria o banco se não existir
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    db.Database.EnsureCreated();
}

// Frontend na mesma origem
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapGet("/api/cafes", async (ApiDbContext db) =>
    await db.Cafes.ToListAsync());

app.MapGet("/api/cafes/{id:int}", async (int id, ApiDbContext db) =>
{
    var cafe = await db.Cafes.FindAsync(id);

    if (cafe is null)
        return Results.NotFound();

    return Results.Ok(cafe);
});

app.MapPost("/api/cafes", async (CafeEntity cafe, ApiDbContext db) =>
{
    db.Cafes.Add(cafe);
    await db.SaveChangesAsync();
    return Results.Created($"/api/cafes/{cafe.Id}", cafe);
});

app.MapPut("/api/cafes/{id:int}", async (int id, CafeEntity entrada, ApiDbContext db) =>
{
    var cafe = await db.Cafes.FindAsync(id);
    if (cafe is null) return Results.NotFound();

    cafe.Marca = entrada.Marca;
    cafe.Volume = entrada.Volume;
    cafe.Tipo = entrada.Tipo;

    await db.SaveChangesAsync();
    return Results.Ok(cafe);
});

app.MapDelete("/api/cafes/{id:int}", async (int id, ApiDbContext db) =>
{
    var cafe = await db.Cafes.FindAsync(id);
    if (cafe is null) return Results.NotFound();

    db.Cafes.Remove(cafe);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
