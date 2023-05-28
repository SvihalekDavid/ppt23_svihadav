using System.Net.Security;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Ppt23.Shared;
using PPT23.API.Data;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(corsOptions => corsOptions.AddDefaultPolicy(policy =>
    policy.WithOrigins(builder.Configuration["AllowedOrigins"])
    .WithMethods("GET", "DELETE", "POST", "PUT")
    .AllowAnyHeader()
));


Console.WriteLine(builder.Configuration["AllowedOrigins"]);

builder.Services.AddDbContext<PptDbContext>(opt => opt.UseSqlite("FileName=mojeDatabaze.db"));


var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.CreateScope().ServiceProvider
  .GetRequiredService<PptDbContext>()
  .Database.Migrate();

app.UseHttpsRedirection();
List<VybaveniVM> seznamVybaveni = VybaveniVM.VratRandSeznam(10);
List<RevizeViewModel> seznamRevizi = RevizeViewModel.VratRandSeznam(10);

app.MapGet("/vybaveni", (PptDbContext db) =>
{
    return seznamVybaveni;
    //List<VybaveniVM> destinations = db.MakeListVybaveniVM();
    //return destinations;
});

app.MapGet("/revize", (PptDbContext db) =>
{
    List<RevizeViewModel> destinations = db.MakeListRevizeVM();
    return destinations;
});

app.MapGet("/revize/{Name}", (string Name, PptDbContext db) =>
{
    List<RevizeViewModel> listR = db.FindRevizeVM(Name);
    if (listR.Count == 0)
        return Results.NotFound("Zadne vysledky!");
    return Results.Ok(listR);
});

app.MapGet("/vybaveni/{id}", (Guid id, PptDbContext db) =>
{
    VybaveniVM? item = db.FindVybaveniVM(id);
    if (item is null)
        return Results.NotFound("Item Not Found!");
    return Results.Ok(item);
});


app.MapPost("/vybaveni", (VybaveniVM prichoziModel, PptDbContext db) =>
{
    prichoziModel.Id = Guid.Empty;
    //Vybaveni en = Vybaveni.MakeVybaveniFromVybaveniVM(prichoziModel);
    var en = prichoziModel.Adapt<Vybaveni>();
    db.Vybavenis.Add(en);
    db.SaveChanges();
    return en.Id;
});

app.MapDelete("/vybaveni/{Id}", (Guid Id, PptDbContext db) =>
{
    Vybaveni? item = db.FindVybaveni(Id);
    if (item == null)
        return Results.NotFound("Tato položka nebyla nalezena!!");
    db.Vybavenis.Remove(item);
    db.SaveChanges();
    return Results.Ok();
}
);

app.MapPut("/vybaveni/{Id}", (VybaveniVM prichoziModel, PptDbContext db) =>
{
    Vybaveni? item = db.FindVybaveni(prichoziModel.Id);
    if (item == null)
        return Results.NotFound("Tato položka nebyla nalezena!!");
    db.UpdateVybaveni(prichoziModel);
    db.SaveChanges();
    return Results.Ok();
});

app.Run();
