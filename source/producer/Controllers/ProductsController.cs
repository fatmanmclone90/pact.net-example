using Microsoft.AspNetCore.Mvc;

namespace producer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet(Name = "GetProducts")]
    public IActionResult Get()
    {
        return new OkObjectResult(
            new List<Product>{
                    new()
                    {
                        Id = 1,
                        Description = "fork"
                    }
        });
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public IActionResult Get(int id)
    {
        return new OkObjectResult(
            new Product
            {
                Id = id,
                Description = "fork"
            });
    }
}
