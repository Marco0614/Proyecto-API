using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public InventarioController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Inventario/Lista
        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<IEnumerable<InventarioDTO>>> Lista()
        {
            try
            {
                var inventarios = await _unitOfWork.Inventario.GetInventarioAsync();
                if (inventarios == null)
                {
                    throw new Exception("Inventario no encontrado");
                }

                var inventarioDTOs = _mapper.Map<List<InventarioDTO>>(inventarios);

                return Ok(inventarioDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Inventario/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var inventario = await _unitOfWork.Inventario.GetInventarioID(id);

                if (inventario == null)
                {
                    throw new Exception("Inventario no encontrado");
                }

                var inventarioDTO = _mapper.Map<InventarioDTO>(inventario);

                return Ok(inventarioDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Inventario
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] InventarioDTO inventario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _unitOfWork.Inventario.CreateInventario(inventario);

                if (response != null)
                {
                    return BadRequest("No se pudo crear el inventario");
                   
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al intentar crear el inventario: {ex.Message}");
            }
        }

        // PUT: api/Inventario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] InventarioDTO inventarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _unitOfWork.Inventario.UpdateInventario(inventarioDto);

                if (response == null)
                {
                    return BadRequest("No se puede actualizar inventario");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Inventario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var inventario = await _unitOfWork.Inventario.GetInventarioID(id);

                if (inventario == null)
                {
                    return NotFound("Inventario no encontrado");
                }

                var response = await _unitOfWork.Inventario.DeleteInventario(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al intentar eliminar el inventario: {ex.Message}");
            }
        }
    }
}
