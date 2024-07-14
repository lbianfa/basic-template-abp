using BookStore.GlobalDtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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
