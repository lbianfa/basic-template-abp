using Volo.Abp.Authorization.Permissions;

namespace BookStore.Permissions;

public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var bookStoreGroup = context.AddGroup(BookStorePermissions.GroupName);

        var booksPermission = bookStoreGroup.AddPermission(BookStorePermissions.Books.Default);

        booksPermission.AddChild(BookStorePermissions.Books.Create);
        booksPermission.AddChild(BookStorePermissions.Books.Edit);
        booksPermission.AddChild(BookStorePermissions.Books.Delete);
    }
}
