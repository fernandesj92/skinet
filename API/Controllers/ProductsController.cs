using System;
using Core.Entities;
using Core.Interface;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController (IGenericRepository<Product> repo) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type,string? sort)
    //if we dont use [ApiController] decorator on top then need to specify for each string parameter like this GetProducts([FromQuery]string? brand,[FromQuery] string? type,[FromQuery] string? sort)
    {
        //Create Spec where expression
        var spec = new ProductSpecification(brand,type,sort);
        
        //pass spec where expression to generic repo
        var products= await repo.ListAsync(spec);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsynd(id);
        if(product==null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        if(await repo.SaveAllAsync())
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

        repo.Update(product);
        if(await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Pronlem Updating the Product");

    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult>DeleteProduct(int id)
    {
        var product=await repo.GetByIdAsynd(id);
        if(product==null) return NotFound();
        repo.Remove(product);
        if(await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem Deletig The Product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>>GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>>GetTypes()
    {
        var spec=new TypeListSpecification();
        return Ok(await repo.ListAsync(spec));
    }

    private bool ProductExist(int id)
    {
        return repo.Exists(id);
    }
}
