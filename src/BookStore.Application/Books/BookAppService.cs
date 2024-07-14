using BookStore.Authors;
using BookStore.GlobalDtos;
using BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace BookStore.Books
{
    public class BookAppService : CrudAppService<
            Book,
            BookDto,
            Guid,
            PagedSortedAndFilteredResultRequestDto,
            CreateUpdateBookDto
        >,
        IBookAppService
    {
        private readonly IAuthorRepository _authorRepository;

        public BookAppService(
            IRepository<Book, Guid> repository, IAuthorRepository authorRepository) : base(repository)
        {
            _authorRepository = authorRepository;
            GetPolicyName = BookStorePermissions.Books.Default;
            GetListPolicyName = BookStorePermissions.Books.Default;
            GetPolicyName = BookStorePermissions.Books.Default;
            CreatePolicyName = BookStorePermissions.Books.Create;
            UpdatePolicyName = BookStorePermissions.Books.Edit;
            DeletePolicyName = BookStorePermissions.Books.Delete;
        }

        public override async Task<BookDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            var queryable = await Repository.GetQueryableAsync();

            var query = from book in queryable
                        join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                        where book.Id == id
                        select new { book, author };

            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Book), id);
            }

            var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
            bookDto.AuthorName = queryResult.author.Name;
            return bookDto;
        }

        public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedSortedAndFilteredResultRequestDto input)
        {
            await CheckGetListPolicyAsync();

            var queryable = await Repository.GetQueryableAsync();

            var query = from book in queryable
                        join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                        select new {book, author};

            // Apply filtering
            query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.book.Name.Contains(input.Filter));

            // Apply sorting
            query = query.OrderBy(NormalizeSorting(input.Sorting ?? ""));

            // Apply paging
            var totalCount = await AsyncExecuter.CountAsync(query);
            query = query.PageBy(input.SkipCount, input.MaxResultCount);

            // Execute the query and get the result
            var queryResult = await AsyncExecuter.ToListAsync(query);

            //var entityDtos = ObjectMapper.Map<List<Book>, List<BookDto>>(entities);

            var bookDtos = queryResult.Select(x =>
            {
                var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
                bookDto.AuthorName = x.author.Name;
                return bookDto;
            }).ToList();

            return new PagedResultDto<BookDto>(totalCount, bookDtos);
        }

        private static string NormalizeSorting(string sorting)
        {
            if (sorting.IsNullOrEmpty())
            {
                return $"book.{nameof(Book.Name)}";
            }

            if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.Replace(
                    "authorName",
                    "author.Name",
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return $"book.{sorting}";
        }
    }
}
