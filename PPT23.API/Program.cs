using System.Net.Security;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Ppt23.Shared;
using PPT23.API.Data;
using Mapster;
using System.Text;

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

string? sqliteDbPath = builder.Configuration[nameof(sqliteDbPath)];
ArgumentNullException.ThrowIfNull(sqliteDbPath);
builder.Services.AddDbContext<PptDbContext>(opt => opt.UseSqlite($"FileName={sqliteDbPath}"));

var app = builder.Build();
app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
List<VybaveniVM> seznamVybaveni = VybaveniVM.VratRandSeznam(1);
List<RevizeViewModel> seznamRevizi = RevizeViewModel.VratRandSeznam(10);


using var appContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<PptDbContext>();
try
{
    appContext.Database.Migrate();
}
catch (Exception ex)
{
    Console.WriteLine($"Exception during db migration {ex.Message}");
    VybaveniVM v = new VybaveniVM();
    v.Name = $"Exception during db migration {ex.Message}";
    seznamVybaveni.Add(v);
    //throww;
}

app.MapGet("/vybaveni", (PptDbContext db) =>
{
    try
    {
        List<VybaveniVM> destinations = db.Vybavenis.ProjectToType<VybaveniVM>().ToList();
        return destinations;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception during: {ex.Message}");
        VybaveniVM v = new VybaveniVM();
        v.Name = ex.ToString();
        seznamVybaveni.Add(v);
    }
    return seznamVybaveni;
});

app.MapGet("/revize", () =>
{
    return seznamRevizi;
});

app.MapGet("/revize/{Name}", (string Name) =>
{
    List<RevizeViewModel> seznamVyhledanychRevizi = new();
    foreach (RevizeViewModel r in seznamRevizi)
    {
        if (r.Name.Contains(Name))
        {
            seznamVyhledanychRevizi.Add(r);
        }
    }
    if (seznamVyhledanychRevizi.Count == 0)
        return Results.NotFound("Zadne vysledky!");
    return Results.Ok(seznamVyhledanychRevizi);
});

app.MapGet("/vybaveni/{id}", (Guid id) =>
{
    VybaveniVM? en = seznamVybaveni.SingleOrDefault(x => x.Id == id);
    if (en is null)
        return Results.NotFound("Item Not Found!");
    return Results.Ok(en);
});


app.MapPost("/vybaveni", (VybaveniVM prichoziModel) =>
{
    seznamVybaveni.Remove(prichoziModel);
    prichoziModel.Id = Guid.NewGuid();
    seznamVybaveni.Insert(0, prichoziModel);
    return prichoziModel.Id;
});

app.MapDelete("/vybaveni/{Id}", (Guid Id) =>
{
    var item = seznamVybaveni.SingleOrDefault(x => x.Id == Id);
    if (item == null)
        return Results.NotFound("Tato položka nebyla nalezena!!");
    seznamVybaveni.Remove(item);
    return Results.Ok();
}
);

app.MapPut("/vybaveni/{Id}", (VybaveniVM prichoziModel) =>
{
    var item = seznamVybaveni.SingleOrDefault(x => x.Id == prichoziModel.Id);
    if (item == null)
        return Results.NotFound("Tato položka nebyla nalezena!!");

    item.Name = prichoziModel.Name;
    item.BoughtDateTime = prichoziModel.BoughtDateTime;
    item.LastRevisionDateTime = prichoziModel.LastRevisionDateTime;
    item.Cena = prichoziModel.Cena;
    return Results.Ok();
});

app.Run();