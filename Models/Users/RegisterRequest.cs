using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

public class RegisterRequest {

    [Required]
    public string Username {get; set;}
    [Required]
    public string Password {get; set;}
    [Required]
    public string ConfirmPassword {get; set;}
    [Required]
    public string FirstName {get; set;}
    [Required]
    public string LastName {get; set;}
}