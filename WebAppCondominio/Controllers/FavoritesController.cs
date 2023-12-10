using Microsoft.AspNetCore.Mvc;
using WebAppCondominio.Models;
using Firebase.Storage;
using Newtonsoft.Json;

namespace WebAppCondominio.Controllers
{
    public class FavoritesController : Controller
    {
        // GET: FavoritosController
        public IActionResult Index()
        {
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("Index", "Error");


            //Si el usuario tiene un rol de 0 no se podra ver el Index de Visitas
            //if (user.Role == 0)
            //    return RedirectToAction("Index", "Error");

            //ViewBag.Role = user.Role;

            //Muestra el get en la vista
            return View();
        }

        private IActionResult GetFavorites()
        {
            FavoritesHandler favoritesHandler = new FavoritesHandler();

            ViewBag.Favorites = favoritesHandler.GetFavoritesCollection().Result;

            return View();
        }

        public IActionResult Favorites()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("Index", "Error");

            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            //Si el usuario tiene un rol de 0 no se podra ver el Index de Visitas
            //if (user.Role == 0)
            //    return RedirectToAction("Index", "Error");

            //ViewBag.Role = user.Role;

            //Muestra el get en la vista
            return GetFavorites();
        }

       //POST
       [HttpPost]
       [ValidateAntiForgeryToken]
        public IActionResult Create(string cedula, string name, string vehicle, string brand, string model, string color, string date)
        {
            try
            {
                FavoritesHandler favoritesHandler = new FavoritesHandler();

                bool result = favoritesHandler.Create(cedula, name, vehicle, brand, model, color, date).Result;

                return GetFavorites();
            }

            catch (FirebaseStorageException ex)
            {
                ViewBag.Error = new ErrorHandler()
                {
                    Title = ex.Message,
                    ErrorMessage = ex.InnerException?.Message,
                    ActionMessage = "Go back",
                    Path = "/Favorites"
                };

                return View("ErrorHandler");
            }
        }
    }
}