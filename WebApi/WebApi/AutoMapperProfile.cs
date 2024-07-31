
using AutoMapper;
using WebApi.Models;
using WebApi.DTOs;

namespace BubliotecaLab
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Dentro del constructor se definen los mapeos entre clases

            // CreateMap<TOrigen, TDestino>() define un mapeo entre la clase TOrigen y la clase TDestino
            // En este caso, se está mapeando la clase Autor a la clase AutorDto, y viceversa (ReverseMap)

            CreateMap<Inventario, InventarioDTO>().ReverseMap();
            CreateMap<Producto,ProductoDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Proveedore, ProveedoreDTO>().ReverseMap();



            // Esto significa que AutoMapper puede convertir automáticamente un Autor en un AutorDto
            // y viceversa, siempre que las propiedades tengan los mismos nombres y tipos compatibles.
        }
    }
}
