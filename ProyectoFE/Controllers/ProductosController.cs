using Microsoft.AspNetCore.Mvc;
using ProyectoFE.DTOs;
using ProyectoFE.RestApi;

namespace ProyectoFE.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApiRest _apiRest;
        public ProductosController(ApiRest apiRest)
        {
            _apiRest = apiRest;

        }

        public async Task<IActionResult> Index()
        {
            var productos = await _apiRest.GetProductosAsync();

            return View(productos);

        }

        public async Task<IActionResult> Details(int id)
        {
            var productos = await _apiRest.GetProductosIDAsync(id);

            if (productos == null)
            {
                return NotFound();
            }
            return View(productos);
        }

        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(ProductosDTO productosDTO)
        {
            if (ModelState.IsValid)
            {
                var Crearproductos = await _apiRest.PostProductoAsync(productosDTO);

                if (Crearproductos)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "No se pudo actualizar el autor.");
            }
            return View(productosDTO);
        }


        public async Task<IActionResult> edit(int id)
        {
            var Editarproductos = await _apiRest.GetProductosIDAsync(id);
            if (Editarproductos == null)
            {
                return NotFound();
            }
            return View(Editarproductos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> edit(int id, ProductosDTO productosDTO)
        {
            if (id != productosDTO.IdProducto)
            {
                return NoContent();
            }

            if (ModelState.IsValid)
            {
                var edit = await _apiRest.PutProductosAsync(productosDTO);
                if (edit)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "No se pudo actualizar el autor.");
            }
            return View(productosDTO);
        }

        public async Task<IActionResult> delete(int id)
        {
            var delete = await _apiRest.GetProductosIDAsync(id);
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
            var deleteConf = await _apiRest.DeleteProductoAsync(id);
            if (deleteConf)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(delete), new { id });
        }
    }
}
