using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.InteropServices;
using portfoliomanager.Pages;

namespace portfoliomanager.Models
{
    public class Userdb
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Passwordhash { get; set; }
        [Required]
        public string? Email { get; set; }
        public bool Admin { get; set; } = false;
    }
}