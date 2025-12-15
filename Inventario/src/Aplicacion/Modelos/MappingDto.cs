using Aplicacion.Modelos;
using Aplicacion.Modelos;
using AutoMapper;
using Dominio.Modelos.Entidades;

namespace Aplicacion.Mapping;

public class MappingDto : Profile
{
    public MappingDto()
    {
        
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        CreateMap<CategoriaCrearDto, Categoria>();
        CreateMap<CategoriaActualizarDto, Categoria>();

        CreateMap<Producto, ProductoDto>().ReverseMap();
        CreateMap<ProductoCrearDto, Producto>();
        CreateMap<ProductoActualizarDto, Producto>();

        CreateMap<Proveedor, ProveedorDto>().ReverseMap();
        CreateMap<ProveedorCrearDto, Proveedor>();
        CreateMap<ProveedorActualizarDto, Proveedor>();

        CreateMap<ProductoProveedorLote, OfertaDto>().ReverseMap();
        CreateMap<OfertaCrearDto, ProductoProveedorLote>();
        CreateMap<OfertaActualizarDto, ProductoProveedorLote>();
    }
}
