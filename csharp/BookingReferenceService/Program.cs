using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var count = 123456789;

app.MapGet("/booking_reference", () =>
{
    count++;
return JsonSerializer.Serialize(count.ToString("x"));
}).WithOpenApi();

app.Run();
