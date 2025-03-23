using System.ComponentModel.DataAnnotations;
using System.Globalization;
using portfoliomanager.Pages;

namespace portfoliomanager.Models
{
    public class Userdb
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool IsReviewer { get; set; } = false;
    }
}