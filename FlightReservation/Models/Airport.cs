using FlightReservation.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Humanizer.In;

public class Airport
{
    [Key]
    public int AirportID { get; set; }

    [Required]
    [MaxLength(255)]
    public string AirportName { get; set; }

    [Required]
    [MaxLength(255)]
    public string City { get; set; }

    [Required]
    [MaxLength(255)]
    public string Country { get; set; }
    public string Image { get; set; }
    public bool IsStatus { get; set; }
    public ICollection<Flight> DepartureFlights { get; set; }
    public ICollection<Flight> ArrivalFlights { get; set; }
}
