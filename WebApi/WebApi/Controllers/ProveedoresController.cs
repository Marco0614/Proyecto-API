using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.DTOs;
using WebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProveedoresController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // GET: api/proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedoreDTO>>> GetProveedoresAsync()
        {
            try
            {
                var proveedores = await _unitOfWork.Proveedor.GetProveedoresAsync();
                if (proveedores == null)
                {
                    throw new Exception("Proveedores no encontrados");
                }

                var proveedoresDTOs = _mapper.Map<List<ProveedoreDTO>>(proveedores);
                return Ok(proveedoresDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/proveedores/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProveedorByIdAsync(int id)
        {
            try
            {
                var proveedor = await _unitOfWork.Proveedor.GetProveedorByIdAsync(id);
                if (proveedor == null)
                {
                    throw new Exception($"Proveedor con ID {id} no encontrado");
                }

                var proveedorDTO = _mapper.Map<ProveedoreDTO>(proveedor);
                return Ok(proveedorDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/proveedores
        [HttpPost]
        public async Task<IActionResult> CreateProveedorAsync([FromBody] ProveedoreDTO proveedorDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si el proveedor ya existe por nombre
                var existingProveedor = await _unitOfWork.Proveedor.GetProveedorByNombreAsync(proveedorDTO.Nombre);
                if (existingProveedor != null)
                {
                    ModelState.AddModelError("Nombre", $"Ya existe un proveedor con el nombre '{proveedorDTO.Nombre}'.");
                    return BadRequest(ModelState);
                }

                var response = await _unitOfWork.Proveedor.CreateProveedorAsync(proveedorDTO);
                if (response == null)
                {
                    return Ok(response); // Devuelve el proveedor creado
                }
                throw new Exception("No se pudo crear el proveedor");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT: api/proveedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProveedorAsync(int id, [FromBody] ProveedoreDTO proveedorDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _unitOfWork.Proveedor.UpdateProveedorAsync(proveedorDTO);
                if (response == null)
                {
                    return Ok(response);

                }
                throw new Exception($"No se pudo actualizar el proveedor con ID {id}");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/proveedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedorAsync(int id)
        {
            try
            {
                // Verificar si existe el proveedor
                var proveedor = await _unitOfWork.Proveedor.GetProveedorByIdAsync(id);
                if (proveedor == null)
                {
                    return NotFound($"Proveedor con ID {id} no encontrado");
                }

                // Eliminar el proveedor, incluso si tiene productos asociados
                var response = await _unitOfWork.Proveedor.DeleteProveedorAsync(id);
                if (response == null)
                {
                    return Ok(); // Proveedor eliminado correctamente
                }

                return NotFound(); // No se pudo eliminar el proveedor
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar proveedor: {ex.Message}");
            }
        }
    }
}
