using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using portfoliomanager.Models;

using portfoliomanager.PortFolioDbContexts;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace portfoliomanager.Pages
{
    public class CreateProjectModel : PageModel
    {
        private readonly PortfolioDbContext _context;

        public CreateProjectModel(PortfolioDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Projectdb Project { get; set; } = new();
        [BindProperty]
        public string? Category { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddAsync(string Category)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(HttpContext.Session.GetString("token"));
            var Isd = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid input. Please check your data.";
                return Page();
            }
            var parsedstring= Guid.TryParse(Isd, out Guid Isd1);
            var usercheck = _context.Users.FirstOrDefault(u => u.Id == Isd1);
            Project.ProjectOwnerId=Isd!;
            
            Project.ProjectCategory = Category;
            _context.Projects.Add(Project);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Project created successfully.";

            return RedirectToPage("/User/Projects");
        }
    }
}