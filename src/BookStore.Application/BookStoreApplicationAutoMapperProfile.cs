using AutoMapper;
using BookStore.Books;
using System;

namespace BookStore;

public class BookStoreApplicationAutoMapperProfile : Profile
{
    public BookStoreApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<CreateUpdateBookDto, Book>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse<BookType>(src.Type)));
    }
}
