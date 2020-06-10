using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covid19Web.Models
{
    public class Request
    {
        public string Session { get; set; }
        public string UserEmail { get; set; }
        public string IDNumber { get; set; }
        public bool IsPassport { get; set; }
        public bool IsIDCard { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Sex { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Question3 { get; set; }
        public List<Flight> Flights { get; set; }
        public string District { get; set; }
        public string Clinic { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

    public class Flight
    {
        public string Session { get; set; }
        public string FlightNo { get; set; }
        public string Company { get; set; }
        public string Destination { get; set; }
        public DateTime Departure { get; set; }
    }

}