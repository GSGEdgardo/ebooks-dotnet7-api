using System.ComponentModel;
using System.IO.Compression;
using ebooks_dotnet7_api;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("ebooks"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
/*
var ebooks = app.MapGroup("api/ebook");

// TODO: Add more routes
ebooks.MapPost("/", CreateEBookAsync);

app.Run();

// TODO: Add more methods
async Task<IResult> CreateEBookAsync(DataContext context)
{
    return Results.Ok();
}
*/

app.MapGet("/api/ebook", async (DataContext db) =>
    await db.EBooks.ToListAsync()
);

//Create Ebook With Author and Title being unique validation
app.MapPost("/api/ebook", async (DataContext db, CreateEbookDto createEbookDto) =>
{
    var existingEbook = await db.EBooks.Where(e => 
                                                e.Title == createEbookDto.Title 
                                            &&  e.Author == createEbookDto.Author).FirstOrDefaultAsync();
    if(existingEbook is not null)
    {
        return TypedResults.BadRequest("El libro que intenta ingresar ya está registrado en el sistema");
    }

    var ebook = new EBook{
        Title = createEbookDto.Title,
        Author = createEbookDto.Author,
        Genre = createEbookDto.Genre,
        Format = createEbookDto.Format,
        IsAvailable = true,
        Price = createEbookDto.Price,
        Stock = 0
    };

    db.EBooks.Add(ebook);
    await db.SaveChangesAsync();
    return Results.Created($"/api/ebook/{ebook.Id}", ebook);
});


app.MapPut("/api/ebook/{id}", async(int id, EditEbookDto editEbookDto, DataContext db) => 
{
    var existingEbook = await db.EBooks.FindAsync(id);

    if(existingEbook is null)
    {
        return Results.NotFound();
    }

    if(editEbookDto.Title != string.Empty)
    {
        existingEbook.Title = editEbookDto.Title;
    }

    if(editEbookDto.Author != string.Empty)
    {
        existingEbook.Author = editEbookDto.Author;
    }

    if(editEbookDto.Genre != string.Empty)
    {
        existingEbook.Genre = editEbookDto.Genre;
    }

    if(editEbookDto.Format != string.Empty)
    {
        existingEbook.Format = editEbookDto.Format;
    }

    if(editEbookDto.Price > 0)
    {
        existingEbook.Price = editEbookDto.Price;
    }

    await db.SaveChangesAsync();
    return Results.Ok(existingEbook);
});


app.MapPut("/api/ebook/{id}/change-availability", async (int id, DataContext db) =>
{
    var existingEbook = await db.EBooks.FindAsync(id);
    if(existingEbook.IsAvailable == true)
    {
        existingEbook.IsAvailable = false;
        await db.SaveChangesAsync();
        return Results.Ok("El libro ya no está disponible");
    }
    existingEbook.IsAvailable = true;
    await db.SaveChangesAsync();
    return Results.Ok("El libro ahora está disponible");
});

app.MapPut("/api/ebook/{id}/increment-stock", async (int id, DataContext db, UpdateStockDto updateStockDto) => 
{
    var existingEbook = await db.EBooks.Where(e => e.Id == id).FirstOrDefaultAsync();
    if(updateStockDto.Stock <= 0)
    {
        return TypedResults.BadRequest("Ingrese un stock que sea mayor a 0");
    }
    
    existingEbook.Stock += updateStockDto.Stock;
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("/api/ebook/purchase", async(DataContext db, PurchaseEbookDto purchaseEbookDto) => 
{
    var existingEbook = await db.EBooks.Where(e => e.Id == purchaseEbookDto.Id).FirstOrDefaultAsync();

    if(existingEbook is null)
    {
        return TypedResults.BadRequest("El libro que quieres comprar no existe");
    }

    if(purchaseEbookDto.Cantidad > existingEbook.Stock)
    {
        return TypedResults.BadRequest("No hay suficientes libros en stock para realizar la venta");
    }
    int total = purchaseEbookDto.Cantidad * existingEbook.Price;
    if(total != purchaseEbookDto.Pago)
    {
        return TypedResults.BadRequest("El monto total del pago no coincide con el monto necesario a pagar.");
    }

    existingEbook.Stock -= purchaseEbookDto.Cantidad;
    await db.SaveChangesAsync();
    return Results.Ok("Se ha realizado la compra!");
});

app.MapDelete("/api/ebook/{id}", async(DataContext db, int id) =>
{
    if(await db.EBooks.FindAsync(id) is EBook ebook)
    {
        db.EBooks.Remove(ebook);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();