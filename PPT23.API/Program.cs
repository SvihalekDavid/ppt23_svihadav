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
builder.Services.AddScoped<SeedingData>();

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


using var appContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<PptDbContext>();
try
{
    appContext.Database.Migrate();
}
catch (Exception ex)
{
    Console.WriteLine($"Exception during db migration {ex.Message}");
}

app.MapGet("/vybaveni", (PptDbContext db) =>
{
    List<VybaveniVM> destinations = db.Vybavenis.ProjectToType<VybaveniVM>().ToList();
    return destinations;
});

app.MapGet("/revize", (PptDbContext db) =>
{
    List<RevizeViewModel> destinations = db.MakeListRevizeVM();
    return destinations;
});

app.MapGet("/revize/{Name}", (string Name, PptDbContext db) =>
{
    List<RevizeViewModel> listR = db.FindRevizeVM(Name);
    //if (listR.Count == 0)
    //    return Results.NotFound(listR);
    return Results.Ok(listR);
});

app.MapPost("/revize", (RevizeViewModel prichoziModel, PptDbContext db) =>
{
    prichoziModel.Id = Guid.Empty;
    var en = prichoziModel.Adapt<Revize>();
    db.Revizes.Add(en);
    db.SaveChanges();
    return en.Id;
});

app.MapGet("/vybaveni/{id}", (Guid id, PptDbContext db) =>
{
    VybaveniSrevizemaVM? item = db.FindVybaveniSRevizema(id);
    if (item is null)
        return Results.NotFound(null);
    return Results.Ok(item);
});


app.MapPost("/vybaveni", (VybaveniVM prichoziModel, PptDbContext db) =>
{
    prichoziModel.Id = Guid.Empty;
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

await app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingData>().SeedData();

app.Run();