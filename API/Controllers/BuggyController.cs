using System;
using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorised")]
    public IActionResult GetUnauthorised(){
        return Unauthorized();
    }

    [HttpGet("badrequest")]
    public IActionResult GetBadRequest(){
        return BadRequest("Not Good Request");
    }

    [HttpGet("notfound")]
    public IActionResult GetNotFound(){
        return NotFound();
    }

    [HttpGet("internalerror")]
    public IActionResult GetInternalError(){
        throw new Exception("This is a test Exeption");
    }
    
    [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDTO product){
        return Ok();
    }
}
