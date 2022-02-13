using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BibliotecaDB>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Biblioteca") ?? "DataSource=biblioteca.db;Cache=Shared"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapFallback(() => Results.Redirect("/swagger"));

app.UseHttpsRedirection();

app.MapGet("/obras", async (BibliotecaDB db) => await db.Obras.ToListAsync());

app.MapGet("/obras/{id}", async (BibliotecaDB db, int id) =>
{
    return await db.Obras.FindAsync(id) switch
    {
        Obra obra => Results.Ok(obra),
        null => Results.NotFound()
    };
});

app.MapPost("/obras", async (Obra obra, BibliotecaDB db) =>
    {
        db.Obras.Add(obra);
        await db.SaveChangesAsync();

        return Results.Created($"/obras/{obra.id}", obra);
    });

app.MapPut("/obras/{id}", async (int id, Obra _obra, BibliotecaDB db) =>
    {
        var obra = await db.Obras.FindAsync(id);

        if (obra is null) return Results.NotFound();

        obra.titulo = _obra.titulo;
        obra.editora = _obra.editora;
        obra.foto = _obra.foto;
        obra.autores = _obra.autores;

        await db.SaveChangesAsync();

        return Results.NoContent();
    });

app.MapDelete("/obras/{id}", async (int id, BibliotecaDB db) =>
    {
        if (await db.Obras.FindAsync(id) is Obra obra)
        {
            db.Obras.Remove(obra);
            await db.SaveChangesAsync();
            return Results.Ok(obra);
        }

        return Results.NotFound();
    });

await app.RunAsync();
