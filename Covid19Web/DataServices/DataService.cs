using Covid19Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using Microsoft.Owin.Security;

namespace Covid19Web.DataServices
{
    public class DataService
    {
        private SqlConnection conn;

        public DataService()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
 

        public int AddImage(Images image)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string sqlQuery = "INSERT into Images(Session,Bytes,ContentType) values (@Session, @Bytes,@ContentType)";
                int rowsAffected = db.Execute(sqlQuery, image);
                return rowsAffected;
            }
        }

        public int AddRequest(Request request)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                if(request.BirthDate == null || request.BirthDate == DateTime.MinValue)
                {
                    request.BirthDate = DateTime.Now;
                }
                if (request.AppointmentDate == null || request.AppointmentDate == DateTime.MinValue)
                {
                    request.AppointmentDate = DateTime.Now;
                }

                string sqlQuery = @"INSERT into Request(Session,IDNumber,UserEmail,IsPassport,IsIDCard,FirstName,LastName,BirthDate,Sex,
                                    City,Address,Phone,Email,Question1,Question2,Question3,District,Clinic,AppointmentDate) values 
                                    (@Session, @IDNumber, @UserEmail, @IsPassport, @IsIDCard, @FirstName, @LastName, @BirthDate, @Sex,
                                    @City, @Address, @Phone, @Email, @Question1, @Question2, @Question3, @District, @Clinic, @AppointmentDate)";
                int rowsAffected = db.Execute(sqlQuery, request);

                foreach (var f in request.Flights)
                    AddFlight(f);

                return rowsAffected;
            }
        }

        public int AddFlight(Flight flight)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                if (flight.Departure == null || flight.Departure == DateTime.MinValue)
                {
                    flight.Departure = DateTime.Now;
                }
                string sqlQuery = @"INSERT into Flight(Session,FlightNo,Company,Destination,Departure) values 
                                    (@Session, @FlightNo, @Company, @Destination, @Departure)";
                int rowsAffected = db.Execute(sqlQuery, flight);
                return rowsAffected;
            }
        }

        public Request GetRequest(string id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.Query<Request>("SELECT FirstName,LastName, IDNumber FROM Request WHERE IDNumber = @IDNumber", new { IDNumber = id }).SingleOrDefault();
            }
        }

        public List<Request> GetRequests()
        {
            List<Request> list = new List<Request>();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return list = db.Query<Request>("SELECT * FROM Request").ToList();
            }
        }

        public List<Flight> GetFlightsByRequestId(string session)
        {
            List<Flight> list = new List<Flight>();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return list = db.Query<Flight>("SELECT * FROM Flight WHERE Session = @Session", new { Session = session }).ToList();
            }
        }

        public List<Images> GetImagesByRequestId(string session)
        {
            List<Images> list = new List<Images>();
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return list = db.Query<Images>("SELECT * FROM Images WHERE Session = @Session", new { Session = session }).ToList();
            }
        }
    }
}