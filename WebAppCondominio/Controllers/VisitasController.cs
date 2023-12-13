﻿using Microsoft.AspNetCore.Mvc;
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("Index", "Error");

            ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));
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
		public IActionResult Create(string cedula, string name, string vehicle, string brand, string model, string color, string date, int acceso)
		{
			try
			{
				VisitsHandler visitsHandler = new VisitsHandler();

				bool result = visitsHandler.Create(cedula, name, vehicle, brand, model, color, date, acceso).Result;

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
		public async Task<IActionResult> EditVisits(string VisitId, string cedula, string name, string vehicle, string brand, string model, string color, string date, int acceso)
		{
			try
			{
				// First, get a reference to the document of the visit you want to edit in Firebase
				var visitDocRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId)
					.Collection("Visits")
					.Document(VisitId);

				// Create a dictionary to hold the updated visit data
				var updatedVisitData = new Dictionary<string, object>()
				{
					{ "Cedula", cedula },
					{ "Name", name },
					{ "Vehicle", vehicle },
					{ "Brand", brand },
					{ "Model", model },
					{ "Color", color },
					{ "Date", date },
					{ "Acceso", acceso }
                };

				// Update the visit document with the updated data
				await visitDocRef.UpdateAsync(updatedVisitData);

				// Redirect to the List view after editing the visit
				return RedirectToAction("List", "Visitas");
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

			return View();
		}

		public ActionResult EasyPass()
		{
			ViewBag.User = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("userSession"));

			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccess(string VisitId)
        {
            try
            {
                var taskDocRef = FirestoreDb.Create("condominio-cc812")
                    .Collection("Visits")
                    .Document(VisitId);

                var taskSnapshot = await taskDocRef.GetSnapshotAsync();

                if (taskSnapshot.Exists)
                {
                    var taskData = taskSnapshot.ToDictionary();
                    bool acceso = taskData.ContainsKey("Acceso") ? Convert.ToBoolean(taskData["Acceso"]) : false;

                    await taskDocRef.UpdateAsync("Acceso", !acceso);
                }

                return RedirectToAction("VisitCheck", "Security");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating task status: " + ex.Message);
                return View();
            }
        }

    }
}