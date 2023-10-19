using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppCondominio.Models;
using WebAppCondominio.FirebaseAuth;
using static Google.Cloud.Firestore.V1.StructuredAggregationQuery.Types.Aggregation.Types;
using Firebase.Storage;
using System.Collections.Generic;

namespace WebAppCondominio.Controllers
{
    public class VisitasController : Controller
    {
        // GET: VisitasController
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                return RedirectToAction("Index", "Error");

            return await GetVisitas();
        }

        private async Task<IActionResult> GetVisitas()
        {
            List<Visitas> visitasList = new List<Visitas>();
            Query query = FirestoreDb.Create("condominio-cc812").Collection("Visitas");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                visitasList.Add(new Visitas
                {
                    Cedula = data["id"].ToString(),
                    Nombre = data["name"].ToString(),
                    Vehiculo = data["vehicle"].ToString(),
                    Marca = data["brand"].ToString(),
                    Modelo = data["model"].ToString(),
                    Color = data["color"].ToString(),
                    Fecha = data["date"].ToString()
                });
            }

            ViewBag.Visitas = visitasList;

            return View("List");
        }



        public ActionResult Visitas()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        // GET: VisitasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VisitasController/Create
        public ActionResult Create()
        {
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
                        .Collection("Visitas").AddAsync(new Dictionary<string, object>
                            {
                                { "Cedula",id },
                                { "Nombre", name },
                                { "Vehiculo", vehicle },
                                { "Marca",brand },
                                { "Modelo", model },
                                { "Color", color },
                                { "Fecha", date },
                            });

                return await GetVisitas();
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

        // GET: VisitasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VisitasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VisitasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VisitasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
