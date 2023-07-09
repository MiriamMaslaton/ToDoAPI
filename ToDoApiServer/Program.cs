using Microsoft.OpenApi.Models;
using ToDoApi;
// using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddDbContext<ToDoDbContext>();

var app = builder.Build();

app.UseCors("AllowOrigin");

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// app.UseSwagger();

// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
// });

app.MapGet("/items", (ToDoDbContext db) => db.Items.ToList());

app.MapPost("/items", async (ToDoDbContext db, Item item) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return item;
});

app.MapPut("/items/{id}", async (int id, Item item, ToDoDbContext db) =>
{
    var it = await db.Items.FindAsync(id);
    if (it is null) return Results.NotFound();

    // it.Name = item.Name;
    it.IsComplete = item.IsComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var it = await db.Items.FindAsync(id);
    if (it is null) return Results.NotFound();

    db.Items.Remove(it);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();