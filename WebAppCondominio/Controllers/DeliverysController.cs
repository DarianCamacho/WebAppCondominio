using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAppCondominio.FirebaseAuth;
using WebAppCondominio.Models;

namespace WebAppCondominio.Controllers
{
    public class DeliverysController : Controller
    {
        // GET: DeliveryController
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

        private IActionResult GetDeliverys()
        {
            DeliverysHandler deliverysHandler = new DeliverysHandler();

            ViewBag.Deliverys = deliverysHandler.GetDeliverysCollection().Result;

            return View();
        }

        public IActionResult List()
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
            return GetDeliverys();
        }

       //POST
       [HttpPost]
       [ValidateAntiForgeryToken]
        public IActionResult Create(string deliveryId, string vehicle, string items, string date)
        {
            try
            {
                DeliverysHandler deliverysHandler = new DeliverysHandler();

                bool result = deliverysHandler.Create(deliveryId, vehicle, items, date).Result;

                return GetDeliverys();
            }

            catch (FirebaseStorageException ex)
            {
                ViewBag.Error = new ErrorHandler()
                {
                    Title = ex.Message,
                    ErrorMessage = ex.InnerException?.Message,
                    ActionMessage = "Go back",
                    Path = "/List"
                };

                return View("ErrorHandler");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string DelyId)
        {
            try
            {
                // Primero, obtén la referencia al documento de la tarjeta que deseas eliminar en Firebase
                var delyDocRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId)
                    .Collection("Deliverys")
                    .Document(DelyId);

                // Borra el documento de la tarjeta
                await delyDocRef.DeleteAsync();

                // Redirige a la vista principal (Index) después de eliminar la tarjeta
                return RedirectToAction("List", "Delivery");
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine("Error al eliminar pedido: " + ex.Message);
                return View();
            }
        }

    }
}