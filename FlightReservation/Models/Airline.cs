using FlightReservation.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Humanizer.In;

public class Airline
{
    [Key]
    public int AirlineID { get; set; }
    [Required]
    [MaxLength(255)]
    public string AirlineName { get; set; }
    public bool IsStatus { get; set; }
    public ICollection<Flight> Flights { get; set; }
}