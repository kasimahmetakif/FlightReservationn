using FlightReservation.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Passenger
{
    [Key]
    public int PassengerID { get; set; }

    [Required]
    [MaxLength(255)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(255)]
    public string LastName { get; set; }

    [MaxLength(255)]
    public string Email { get; set; }

    [MaxLength(255)]
    public string Phone { get; set; }

    [Required]
    [MaxLength(255)]
    public string Password { get; set; }

    public bool IsStatus { get; set; }

    public ICollection<Reservation> Reservations { get; set; }
}