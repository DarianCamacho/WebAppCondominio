using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAppCondominio.FirebaseAuth;
using WebAppCondominio.Models;

namespace WebAppCondominio.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
				return RedirectToAction("Index", "Error");

			ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

			return GetUsers();
		}

		public IActionResult GetUserName()
		{


			// Leemos de la sesión los datos del usuario
			Models.User? user = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

			// Pasamos el nombre de usuario a la vista
			ViewBag.UserName = user?.Name;

			return View();
		}

		private IActionResult GetUsers()
		{
			UsersHandler usersHandler = new UsersHandler();

			ViewBag.Users = usersHandler.GetUsersCollection().Result;

			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string UserId)
        {
            try
            {
                // Primero, obtén la referencia al documento de la tarjeta que deseas eliminar en Firebase
                var cardDocRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId)
                    .Collection("Users")
                    .Document(UserId);

                // Borra el documento de la tarjeta
                await cardDocRef.DeleteAsync();

                // Redirige a la vista principal (Index) después de eliminar la tarjeta
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                return View();
            }
        }

    }
}
