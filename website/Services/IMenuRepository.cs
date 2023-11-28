namespace website.Services;

public interface IMenuRepository
{
    Task<MenuItem[]> GetMenuItemsAsync();
}
