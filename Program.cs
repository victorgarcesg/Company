var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// In-memory list to store items
var items = new List<Item>();

// Read all
app.MapGet("/items", () => Results.Ok(items));

// Read one
app.MapGet("/items/{id}", (int id) =>
{
    var item = items.FirstOrDefault(i => i.Id == id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
});

// Update
app.MapPut("/items/{id}", (int id, Item updatedItem) =>
{
    var index = items.FindIndex(i => i.Id == id);
    if (index == -1) return Results.NotFound();

    items[index] = updatedItem with { Id = id };
    return Results.NoContent();
});

// Delete
app.MapDelete("/items/{id}", (int id) =>
{
    var item = items.FirstOrDefault(i => i.Id == id);
    if (item is null) return Results.NotFound();

    items.Remove(item);
    return Results.NoContent();
});

app.Run();

record Item(int Id, string Name, string Description);
