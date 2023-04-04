using System.Net.Security;
using Ppt23.Client.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(corsOptions => corsOptions.AddDefaultPolicy(policy =>
    policy.WithOrigins("https://localhost:1111")
    .WithMethods("GET", "DELETE", "POST", "PUT")
    .AllowAnyHeader()
));

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
List<VybaveniVM> seznamVybaveni = VybaveniVM.VratRandSeznam(10);
//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

app.MapGet("/vybaveni", () =>
{
    return seznamVybaveni;
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
//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

app.Run();

//internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
