using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Products;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();

            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggregate.Address>().ReverseMap();

            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LName))
                .ReverseMap()
                .ForMember(d => d.FName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LName, o => o.MapFrom(s => s.LastName));

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
                
               



        }
    }
}
