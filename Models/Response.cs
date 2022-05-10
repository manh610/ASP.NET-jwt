using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class Response
{
    [Required]
    public Object data {get; set;}
    [Required]
    public string message {get; set;}
}