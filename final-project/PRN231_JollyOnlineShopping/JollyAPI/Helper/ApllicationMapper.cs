using AutoMapper;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;

namespace JollyAPI.Helper
{
    public class ApllicationMapper : Profile
    {
        public ApllicationMapper()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<Image, ImageUploadDTO>().ReverseMap();
            CreateMap<Color, ColorDTO>().ReverseMap();
            CreateMap<Rating, RatingDTO>().ReverseMap();
            CreateMap<Size, SizeDTO>().ReverseMap();
            CreateMap<Blog, BlogDTO>().ReverseMap();
        }
    }
}
