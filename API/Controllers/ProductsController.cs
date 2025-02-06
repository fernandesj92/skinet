using System;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController (IProductRepository repo) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type,string? sort)
    //if we dont use [ApiController] decorator on top then need to specify for each string parameter like this GetProducts([FromQuery]string? brand,[FromQuery] string? type,[FromQuery] string? sort)
    {
        return Ok(await repo.GetProductsAsync(brand,type,sort));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if(product==null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);
        if(await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new {id = product.Id},product);
        }
        return BadRequest("Problem Creating Product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult>UpdateProduct(int id,Product product)
    {
        if(product.Id!=id || !ProductExist(id))
            return BadRequest("Product Doesn't Exist");

        repo.UpdateProduct(product);
        if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Pronlem Updating the Product");

    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult>DeleteProduct(int id)
    {
        var product=await repo.GetProductByIdAsync(id);
        if(product==null) return NotFound();
        repo.DeleteProduct(product);
        if(await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem Deletig The Product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>>GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>>GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }

    private bool ProductExist(int id)
    {
        return repo.ProductExists(id);
    }
}
