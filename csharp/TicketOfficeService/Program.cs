using TicketOfficeService;

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

app.MapPost("/reserve", (ReserveRequest request) =>
{
    var ticketOffice = new TicketOffice();
    var reservationRequest = new ReservationRequest(request.train_id, request.seat_count);
    var reservation = ticketOffice.MakeReservation(reservationRequest);
    
    return Results.Ok(reservation);
}).WithOpenApi();

app.Run();

record ReserveRequest(string train_id, int seat_count);