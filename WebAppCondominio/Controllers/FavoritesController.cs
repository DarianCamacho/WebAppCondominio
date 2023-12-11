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

            if (ViewBag.User is Models.User user)
            {
                if (user.Role != 0)
                {
                    //Redirige a la página de error si el usuario no tiene un rol válido

                    return RedirectToAction("Index", "Error");
                }

                ViewBag.Role = user.Role;
            }
            else
            {
                //Redirigir a la pagina que se selecciono

                return RedirectToAction("Index", "Admin");
            }

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
            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("Index", "Error");

            if (ViewBag.User is Models.User user)
            {
                if (user.Role != 0)
                {
                    //Redirige a la página de error si el usuario no tiene un rol válido

                    return RedirectToAction("Index", "Error");
                }

                ViewBag.Role = user.Role;
            }
            else
            {
                //Redirigir a la pagina que se selecciono

                return RedirectToAction("Index", "Admin");
            }

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