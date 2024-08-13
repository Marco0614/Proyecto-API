using Microsoft.AspNetCore.Mvc;
using ProyectoFE.DTOs;
using ProyectoFE.RestApi;

namespace ProyectoFE.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ApiRest _apiRest;
        public InventarioController(ApiRest apiRest)
        {
            _apiRest = apiRest;

        }

        public async Task<IActionResult> Index()
        {
            var inventario = await _apiRest.GetInventarioAsync();

            return View(inventario);

        }

        public async Task<IActionResult> Details(int id)
        {
            var inventario = await _apiRest.GetInventarioIDAsync(id);

            if (inventario == null)
            {
                return NotFound();
            }
            return View(inventario);
        }

        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(InventarioDTO inventarioDTO)
        {
            if (ModelState.IsValid)
            {
                var Crearinventario = await _apiRest.PostInventarioAsync(inventarioDTO);

                if (Crearinventario)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "No se pudo actualizar el autor.");
            }
            return View(inventarioDTO);
        }


        public async Task<IActionResult> edit(int id)
        {
            var Editarinventario = await _apiRest.GetInventarioIDAsync(id);
            if (Editarinventario == null)
            {
                return NotFound();
            }
            return View(Editarinventario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> edit(int id, InventarioDTO inventarioDTO)
        {
            if (id != inventarioDTO.IdMovimiento)
            {
                return NoContent();
            }

            if (ModelState.IsValid)
            {
                var edit = await _apiRest.PutInventarioAsync(inventarioDTO);
                if (edit)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "No se pudo actualizar el autor.");
            }
            return View(inventarioDTO);
        }

        public async Task<IActionResult> delete(int id)
        {
            var delete = await _apiRest.GetInventarioIDAsync(id);
            if (delete == null)
            {
                return NotFound();
            }
            return View(delete);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deleteConformed(int id)
        {
            var deleteConf = await _apiRest.DeleteInventarioAsync(id);
            if (deleteConf)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(delete), new { id });
        }
    }
}
