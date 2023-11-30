using Microsoft.AspNetCore.Mvc;
using WebAppCondominio.Models;
using Firebase.Storage;
using Newtonsoft.Json;
using Google.Cloud.Firestore;
using WebAppCondominio.FirebaseAuth;

namespace WebAppCondominio.Controllers
{
    public class VisitasController : Controller
    {
        // GET: VisitasController
        public IActionResult Index()
        {
            //ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
            //    return RedirectToAction("Index", "Error");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("Index", "Error");

            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            //Si el usuario tiene un rol de 0 no se podra ver el Index de Visitas
            //if (user.Role == 0)
            //    return RedirectToAction("Index", "Error");

            //ViewBag.Role = user.Role;

            //Muestra el get en la vista
            return GetVisits();
        }

        public IActionResult GetUserName()
        {


            // Leemos de la sesión los datos del usuario
            Models.User? user = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            // Pasamos el nombre de usuario a la vista
            ViewBag.UserName = user?.Name;

            return View();
        }

        public IActionResult List()
        {
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("List", "Error");

            //Muestra el get en la vista
            return GetVisits();
        }


        private IActionResult GetVisits()
        {
            VisitsHandler visitsHandler = new VisitsHandler();

            ViewBag.Visits = visitsHandler.GetVisitsCollection().Result;

            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( string cedula, string name, string vehicle, string brand, string model, string color, string date)
        {
            try
            {
                VisitsHandler visitsHandler = new VisitsHandler();

                bool result = visitsHandler.Create( cedula, name, vehicle, brand, model, color, date).Result;

                return GetVisits();
            }

            catch (FirebaseStorageException ex)
            {
                ViewBag.Error = new ErrorHandler()
                {
                    Title = ex.Message,
                    ErrorMessage = ex.InnerException?.Message,
                    ActionMessage = "Go back",
                    Path = "/Visitas"
                };

                return View("ErrorHandler");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditVisit(string id, string cedula, string name, string vehicle, string brand, string model, string color, string date)
        {
            try
            {
                VisitsHandler visitsHandler = new VisitsHandler();

                bool result = visitsHandler.Edit(id, cedula, name, vehicle, brand, model, color, date).Result;

                return GetVisits();
            }

            catch (FirebaseStorageException ex)
            {
                ViewBag.Error = new ErrorHandler()
                {
                    Title = ex.Message,
                    ErrorMessage = ex.InnerException?.Message,
                    ActionMessage = "Go to index",
                    Path = "/List"
                };

                return View("ErrorHandler");
            }
        }

        public IActionResult Edit(string id, string cedula, string name, string vehicle, string brand, string model, string color, string date)
        {
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            Visit edited = new Visit
            {
                Id = id,
                Cedula = cedula,
                Name = name,
                Vehicle = vehicle,
                Brand = brand,
                Model = model,
                Color = color,
                Date = date
            };

            ViewBag.Edited = edited;

            //Muestra el get en la vista
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string VisitId)
        {
            try
            {
                // Primero, obtén la referencia al documento de la tarjeta que deseas eliminar en Firebase
                var cardDocRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId)
                    .Collection("Visits")
                    .Document(VisitId);

                // Borra el documento de la tarjeta
                await cardDocRef.DeleteAsync();

                // Redirige a la vista principal (Index) después de eliminar la tarjeta
                return RedirectToAction("List", "Visitas");
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine("Error al eliminar tarjeta: " + ex.Message);
                return View();
            }
        }

        public ActionResult Visitas()
        {
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            return View();
        }

        public ActionResult EasyPass()
        {
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            return View();
        }
    }
}