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
        public BookAppService(IRepository<Book, Guid> repository) : base(repository)
        {
            GetPolicyName = BookStorePermissions.Books.Default;
            GetListPolicyName = BookStorePermissions.Books.Default;
            GetPolicyName = BookStorePermissions.Books.Default;
            CreatePolicyName = BookStorePermissions.Books.Create;
            UpdatePolicyName = BookStorePermissions.Books.Edit;
            DeletePolicyName = BookStorePermissions.Books.Delete;
        }

        public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedSortedAndFilteredResultRequestDto input)
        {
            await CheckGetListPolicyAsync();

            var query = await Repository.GetQueryableAsync();

            // Apply filtering
            query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(), b => b.Name.Contains(input.Filter));

            // Apply sorting
            query = query.OrderBy(input.Sorting ?? "Name");

            // Apply paging
            var totalCount = await AsyncExecuter.CountAsync(query);
            query = query.PageBy(input.SkipCount, input.MaxResultCount);

            // Execute the query and get the result
            var entities = await AsyncExecuter.ToListAsync(query);

            var entityDtos = ObjectMapper.Map<List<Book>, List<BookDto>>(entities);

            return new PagedResultDto<BookDto>(totalCount, entityDtos);
        }
    }
}
