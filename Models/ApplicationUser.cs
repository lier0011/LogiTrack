using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}