//using AutoMapper;
//using KitabhChauta.Dto;
//using KitabhChauta.Model;

//namespace KitabhChauta
//{
//    public class AutoMapperProfile : Profile
//    {
//        public AutoMapperProfile()
//        {
//            // Map CreateBookDto to Book
//            CreateMap<CreateBookDto, Book>()
//                .ForMember(dest => dest.Author, opt => opt.Ignore()) // Ignore navigation property
//                .ForMember(dest => dest.Genre, opt => opt.Ignore()) // Ignore navigation property
//                .ForMember(dest => dest.Publisher, opt => opt.Ignore()); // Ignore navigation property

//            // Map Book to BookDto
//            CreateMap<Book, BookDto>();

//            // Map UpdateBookDto to Book
//            CreateMap<UpdateBookDto, Book>()
//                .ForMember(dest => dest.Author, opt => opt.Ignore()) // Ignore navigation property
//                .ForMember(dest => dest.Genre, opt => opt.Ignore()) // Ignore navigation property
//                .ForMember(dest => dest.Publisher, opt => opt.Ignore()); // Ignore navigation property
//        }
//    }
//}