using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using portfoliomanager.Models;
using portfoliomanager.PortFolioDbContexts;

namespace portfoliomanager.Pages
{
    public class RegisterUserModel : PageModel
    {
        private readonly ILogger<RegisterUserModel> _logger;
        private readonly PortfolioDbContext _context;
        private readonly PasswordHasher<Userdb> _passwordHasher;
        public Userdb user { get; set; } = new();
        public RegisterUserModel(ILogger<RegisterUserModel> logger, PortfolioDbContext context)
        {
            _logger = logger;
            _context = context;
            _passwordHasher = new PasswordHasher<Userdb>();
        }
        public async Task<IActionResult> OnPostAsync(Userdb user)
        {
            if (user == null)
            {
                TempData["Error"] = "User not found";
                return Page();
            }
            if ( await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username || u.Email == user.Email) != null )
            {
                TempData["Error"] = "Username or Email already exists";
                return Page();
            }
            var pass = _passwordHasher.HashPassword(user, user.Passwordhash!);
            user.Passwordhash = pass;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            TempData["Success"]= user.LastName + " has been registered";
            return RedirectToPage("Login");
        }
        public void OnGet()
        {
        }
    }
}