using Covid19Web.DataServices;
using Covid19Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Covid19Web.Controllers
{
    public class HomeController : Controller
    {
        private DataService _ds;

        public HomeController()
        {
            _ds = new DataService();
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                Guid g = Guid.NewGuid();
                ViewBag.Session = g.ToString().Replace("-", "");
                return View();
            }
            else
                return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public JsonResult AjaxPostCall(Request wizardData)
        {
            string result = "OK";
            if (User.Identity.IsAuthenticated)
            {
                Request wizard = new Request
                {
                    Session = wizardData.Session,
                    UserEmail = User.Identity.Name,
                    FirstName = wizardData.FirstName,
                    IDNumber = wizardData.IDNumber,
                    IsIDCard = wizardData.IsIDCard,
                    IsPassport = wizardData.IsPassport,
                    LastName = wizardData.LastName,
                    Address = wizardData.Address,
                    BirthDate = wizardData.BirthDate,
                    City = wizardData.City,
                    Sex = wizardData.Sex,
                    AppointmentDate = wizardData.AppointmentDate,
                    Clinic = wizardData.Clinic,
                    District = wizardData.District,
                    Email = wizardData.Email,
                    Phone = wizardData.Phone,
                    Question1 = wizardData.Question1,
                    Question2 = wizardData.Question2,
                    Question3 = wizardData.Question3,
                };

                wizard.Flights = new List<Flight>();

                foreach (var f in wizardData.Flights)
                {
                    if (!String.IsNullOrEmpty(f.FlightNo))
                    {
                        wizard.Flights.Add(new Flight { Company = f.Company, Departure = f.Departure, Destination = f.Destination, FlightNo = f.FlightNo, Session = wizardData.Session });
                    }

                }

                _ds.AddRequest(wizard);

            }
            else
            {
                result = "You are not authorized to perform this action";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFile()
        {
            string result = "OK";
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    if(Request.Headers["session"] != null)
                    {
                        if (!String.IsNullOrEmpty(Request.Headers["session"]))
                        {
                            foreach (string fileName in Request.Files)
                            {
                                HttpPostedFileBase file = Request.Files[fileName];

                                if (file != null && file.ContentLength > 0)
                                {
                                    byte[] data;
                                    using (Stream inputStream = file.InputStream)
                                    {
                                        MemoryStream memoryStream = inputStream as MemoryStream;
                                        if (memoryStream == null)
                                        {
                                            memoryStream = new MemoryStream();
                                            inputStream.CopyTo(memoryStream);
                                        }
                                        data = memoryStream.ToArray();
                                    }
                                    if (data != null && data.Length > 0)
                                    {
                                        try
                                        {
                                            string type = file.ContentType.Split('/')[1];
                                            _ds.AddImage(new Images { Bytes = data, Session = Request.Headers["session"].ToString(), ContentType = type });
                                            result = "File updated succesfully";
                                        }
                                        catch (Exception ex)
                                        {
                                            result = "File storing failed";
                                        }
                                    }

                                }
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    result = "File storing error occured";
                }
            }
            else
            {
                result = "You are not authorized to perform this action";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStatus(string id)
        {
            Result result = new Result();
            result.Status = -1;
            if (User.Identity.IsAuthenticated)
            {
                result.IDNumber = id;
                Request request = _ds.GetRequest(id);
                if(request != null && request.IDNumber == id) 
                {
                    result.FirstName = request.FirstName;
                    result.LastName = request.LastName;
                    result.Status = new Random().Next(0, 10) > 4 ? 1 : 0;
                }
                
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetImages(string id)
        {
            List<ImageModel> model = new List<ImageModel>();
            if (User.Identity.IsAuthenticated)
            {
                var images = _ds.GetImagesByRequestId(id);
                if(images != null && images.Count() > 0)
                {
                    string filename;
                    string path;
                    foreach (var img in images)
                    {
                        if(img.Bytes.Length > 0)
                        {
                            filename = Path.GetFileName(img.Id + img.Session+"." + img.ContentType);
                            path = Path.Combine(Server.MapPath("~/content/images/"), filename);
                            if(!System.IO.File.Exists(path))
                                System.IO.File.WriteAllBytes(path, img.Bytes);

                            model.Add(new ImageModel { Path = filename, Type = img.ContentType });
                        }
                    }
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Results()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Data()
        {
            DataViewModel model = new DataViewModel();
            model.Requests = _ds.GetRequests();
           // List<Request> reqs = _ds.GetRequests();
            
            /*foreach (var req in reqs)
            {
                List<Flight> flights = _ds.GetFlightsByRequestId(req.Session);
                List<Images> images = _ds.GetImagesByRequestId(req.Session);

                model.Items.Add(new DataViewModelItem { Request = req, Images = images, Flights = flights});
            }*/

            return View(model);
        }
    }
}