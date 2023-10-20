using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppCondominio.Models;
using WebAppCondominio.FirebaseAuth;
using static Google.Cloud.Firestore.V1.StructuredAggregationQuery.Types.Aggregation.Types;
using Firebase.Storage;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebAppCondominio.Controllers
{
    public class VisitasController : Controller
    {
        // GET: VisitasController
        public async Task<IActionResult> Index()
        {
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("Index", "Error");

            //Muestra el get en la visa
            return await GetVisits();
        }

        public IActionResult GetUserName()
        {



            // Leemos de la sesión los datos del usuario
            Models.User? user = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            // Pasamos el nombre de usuario a la vista
            ViewBag.UserName = user?.Name;

            return View();
        }

        public async Task<IActionResult> List()
        {
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("List", "Error");

            //Muestra el get en la visa
            return await GetVisits();
        }

        private async Task<IActionResult> GetVisits()
        {
            List<Visit> visitsList = new List<Visit>();
            Query query = FirestoreDb.Create("condominio-cc812").Collection("Visits");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                visitsList.Add(new Visit
                {
                    Id = data["Id"].ToString(),
                    Name = data["Name"].ToString(),
                    Vehicle = data["Vehicle"].ToString(),
                    Brand = data["Brand"].ToString(),
                    Model = data["Model"].ToString(),
                    Color = data["Color"].ToString(),   
                    Date = data["Date"].ToString()
                });
            }

            ViewBag.Visits = visitsList;

            return View();
        }

        // POST: VisitasController/Create  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, string name, string vehicle, string brand, string model, string color, string date)
        {
            try
            {
                DocumentReference addedDocRef =
                    await FirestoreDb.Create("condominio-cc812")
                        .Collection("Visits").AddAsync(new Dictionary<string, object>
                            {
                                { "Id", id },
                                { "Name", name },
                                { "Vehicle", vehicle },
                                { "Brand",  brand },
                                { "Model", model },
                                { "Color", color },
                                { "Date", date },
                            });

                return await GetVisits();
            }

            catch (FirebaseStorageException ex)
            {
                ViewBag.Error = new ErrorHandler()
                {
                    Title = ex.Message,
                    ErrorMessage = ex.InnerException?.Message,
                    ActionMessage = "Go to Visitas",
                    Path = "/Visitas"
                };

                return View("ErrorHandler");
            }
        }

        public ActionResult Visitas()
        {
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            return View();
        }
    }
}