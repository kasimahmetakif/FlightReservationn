using FlightReservation.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Flight
{
    [Key]
    public int FlightID { get; set; }

    [ForeignKey("Airline")]
    public int AirlineID { get; set; }

    [ForeignKey("DepartureAirport")]
    public int DepartureAirportID { get; set; }

    [ForeignKey("ArrivalAirport")]
    public int ArrivalAirportID { get; set; }

    [Required]
    public DateTime DepartureDateTime { get; set; }

    [Required]
    public DateTime ArrivalDateTime { get; set; }

    public int SeatCapacity { get; set; }

    public int? Price { get; set; }

    public bool IsStatus { get; set; }

    public Airline Airline { get; set; }
    public Airport DepartureAirport { get; set; }
    public Airport ArrivalAirport { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}