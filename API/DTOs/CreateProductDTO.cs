using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateProductDTO
{
    [Required]
    public string Name { get; set; }=string.Empty;
    [Required]
    public  string Description { get; set; }=string.Empty;
    [Range(0.01,double.MaxValue,ErrorMessage ="Price Must be Greater Than 0")]
    public decimal Price { get; set; }
    [Required]
    public string PictureUrl { get; set; }=string.Empty;
    [Required]
    public string Type { get; set; }=string.Empty;
    [Required]
    public string Brand { get; set; }=string.Empty;
    [Range(0.01,int.MaxValue,ErrorMessage ="Quantity Must be at least 1")]
    public int QuantityInStock{get;set;}
}
