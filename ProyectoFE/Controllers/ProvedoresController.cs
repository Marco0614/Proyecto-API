using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using ProyectoFE.DTOs;
using ProyectoFE.RestApi;

namespace ProyectoFE.Controllers
{
    public class ProvedoresController : Controller
    {
        private readonly ApiRest _apiRest;
        public ProvedoresController(ApiRest apiRest) 
        {
            _apiRest = apiRest;

        }
        public async Task <IActionResult> Index()
        {
            var provedores = await _apiRest.GetProvedoresAsync();

            return View(provedores);

        }

        public async Task<IActionResult> Details(int id)
        {
            var provedores = await _apiRest.GetProveedoresIDAsync(id);

            if (provedores == null)
            {
                return NotFound();
            }
            return View(provedores);
        }

        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(ProvedoresDTO provedoresDTO)
        {
            if (ModelState.IsValid)
            {
                var CrearProvedor = await _apiRest.PostProvedoresAsync(provedoresDTO);

                if (CrearProvedor)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "No se pudo actualizar el autor.");
            }
            return View(provedoresDTO);
        }

        
        public async Task<IActionResult> edit(int id)
        {
            var editProvedor = await _apiRest.GetProveedoresIDAsync(id);
            if (editProvedor == null)
            {
                return NotFound();
            }
            return View(editProvedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> edit(int id, ProvedoresDTO provedoresDTO)
        {
            if(id != provedoresDTO.IdProveedor)
            {
                return NoContent();
            }

            if (ModelState.IsValid)
            {
                var edit = await _apiRest.PutProvedoresAsync(provedoresDTO);
                if (edit)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "No se pudo actualizar el autor.");
            }
            return View(provedoresDTO);
        }

        public async Task<IActionResult> delete(int id)
        {
            var delete = await _apiRest.GetProveedoresIDAsync(id);
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
            var deleteConf = await _apiRest.DeleteProvedoresAsync(id);
            if (deleteConf)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(delete), new { id });
        }

    }
}
