
namespace consumer2;

public interface IProviderService
{
    Task<Product?> Get(int id);
}