using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppCondominio.Controllers
{
    public class VisitasController : Controller
    {
        // GET: VisitasController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Visitas()
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

                return View("Index");
            }
            catch
            {
                return View();
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
