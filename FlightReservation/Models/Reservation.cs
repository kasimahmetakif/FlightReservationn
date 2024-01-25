using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Reservation
{
    [Key]
    public int ReservationID { get; set; }

    [ForeignKey("Flight")]
    public int FlightID { get; set; }

    [ForeignKey("Passenger")]
    public int PassengerID { get; set; }

    [Required]
    public DateTime ReservationDateTime { get; set; }

    [Required]
    [MaxLength(255)]
    public string SeatNumber { get; set; }

    public bool IsStatus { get; set; }

    public Flight Flight { get; set; }
    public Passenger Passenger { get; set; }
}