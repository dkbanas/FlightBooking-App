namespace Application.Utils;

/// <summary>
/// Utility class for calculating flight durations.
/// </summary>
public class DurationCalculator
{
    /// <summary>
    /// Calculates the duration between the specified departure and arrival times.
    /// </summary>
    /// <param name="departureTime">The scheduled departure time.</param>
    /// <param name="arrivalTime">The scheduled arrival time.</param>
    /// <returns>A formatted string representing the duration in hours and minutes, or "Duration not available" if the times are invalid.</returns>
    public static string CalculateDuration(DateTime departureTime, DateTime arrivalTime)
    {
        // Check if both departure and arrival times are valid
        if (departureTime != default && arrivalTime != default)
        {
            // Calculate the duration as a TimeSpan
            TimeSpan duration = arrivalTime - departureTime;
            
            // Extract hours and minutes from the duration
            long hours = (long)duration.TotalHours;
            long minutes = duration.Minutes;
            // Return the formatted duration string
            return $"{hours} h {minutes} min";
        }
        // Return a default message if duration cannot be calculated
        return "Duration not available";
    }
}