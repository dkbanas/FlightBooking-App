namespace Application.DTOs;
// Response DTO for a round-trip flight, containing both outbound and return flights
public class RoundTripFlightResponseDTO
{
    public FlightResponseDTO OutboundFlight { get; set; }
    public FlightResponseDTO ReturnFlight { get; set; }
    
    public RoundTripFlightResponseDTO(FlightResponseDTO outbound, FlightResponseDTO returnFlight)
    {
        OutboundFlight = outbound;
        ReturnFlight = returnFlight;
    }
}