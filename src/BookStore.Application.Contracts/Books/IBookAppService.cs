using BookStore.GlobalDtos;
using System;
using Volo.Abp.Application.Services;

namespace BookStore.Books
{
    public interface IBookAppService : ICrudAppService<
            BookDto,
            Guid,
            PagedSortedAndFilteredResultRequestDto,
            CreateUpdateBookDto
        >
    {
    }
}
