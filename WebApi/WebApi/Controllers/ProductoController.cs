using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class ProductoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductoController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // GET: api/producto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetProductosAsync()
        {
            try
            {
                var productos = await _unitOfWork.Producto.GetProductosAsync();
                var productosDTOs = _mapper.Map<List<ProductoDTO>>(productos);
                return Ok(productosDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener productos: {ex.Message}");
            }
        }

        // GET: api/producto/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductoByIdAsync(int id)
        {
            try
            {
                var producto = await _unitOfWork.Producto.GetProductoByIdAsync(id);
                if (producto == null)
                {
                    return NotFound($"Producto con ID {id} no encontrado");
                }

                var productoDTO = _mapper.Map<ProductoDTO>(producto);
                return Ok(productoDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener producto: {ex.Message}");
            }
        }

        // POST: api/producto
        [HttpPost]
        public async Task<IActionResult> CreateProductoAsync([FromBody] ProductoDTO productoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si el nombre del producto ya existe en la base de datos
                var existingProduct = await _unitOfWork.Producto.GetProductoByNombreAsync(productoDTO.Nombre);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("Nombre", $"Ya existe un producto con el nombre '{productoDTO.Nombre}'.");
                    return BadRequest(ModelState);
                }

                var response = await _unitOfWork.Producto.CreateProductoAsync(productoDTO);
                if (response == null)
                {
                    return Ok(response); // Devuelve el producto creado
                }
                return BadRequest("No se pudo crear el producto");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear producto: {ex.Message}");
            }
        }


        // PUT: api/producto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductoAsync(int id, [FromBody] ProductoDTO productoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _unitOfWork.Producto.UpdateProductoAsync(productoDTO);
                if (response == null)
                {
                    return Ok(response); // Devuelve el producto actualizado
                }
                return NotFound($"No se encontró el producto con ID {id}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar producto: {ex.Message}");
            }
        }


        // DELETE: api/producto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductoAsync(int id)
        {
            try
            {
                // Verificar si existe el producto
                var producto = await _unitOfWork.Producto.GetProductoByIdAsync(id);
                if (producto == null)
                {
                    return NotFound($"Producto con ID {id} no encontrado");
                }

                // Verificar si hay inventarios asociados
                var inventarios = await _unitOfWork.Inventario.GetInventariosByProductIdAsync(id);
                if (inventarios.Any())
                {
                    // Si hay inventarios asociados, puedes optar por devolver un BadRequest
                    return BadRequest($"No se puede eliminar el producto con ID {id} porque tiene inventarios asociados.");
                }

                // Si no hay inventarios asociados, proceder con la eliminación del producto
                var response = await _unitOfWork.Producto.DeleteProductoAsync(id);
                if (response == null)
                {
                    return Ok(); // Producto eliminado correctamente
                }

                return NotFound(); // No se pudo eliminar el producto
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar producto: {ex.Message}");
            }
        }



    }
}
      