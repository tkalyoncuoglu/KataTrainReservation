namespace TicketOfficeService;

public record ReservationRequest(string TrainId, int SeatCount);
public record Reservation(string TrainId, string BookingId, List<Seat> Seats);
public record Seat(string Coach, int SeatNumber);

public class TicketOffice
{
    public  Reservation MakeReservation(ReservationRequest request)
    {
        var bookingReferenceService = new BookingReferenceService("https://localhost:7182", new HttpClient());

        Task<string> reference = bookingReferenceService.ReferenceAsync();

        Task.WaitAll(reference);

        return new Reservation("", reference.Result, null);
    }
}