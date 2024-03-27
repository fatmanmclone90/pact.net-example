
namespace consumer1;

public interface IProviderService
{
    Task<List<Product>?> Get();
}