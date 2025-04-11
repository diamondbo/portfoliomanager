using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using portfoliomanager.Models;
using portfoliomanager.PortFolioDbContexts;

namespace portfoliomanager.Pages
{
    public class ProjectsModel : PageModel
    {
        private readonly ILogger<ProjectsModel> _logger;
        private readonly PortfolioDbContext _context;
        public Projectdb projectdb { get; set; }= new();
        [BindProperty]
        public string? Category { get; set; }
        
        public ProjectsModel(ILogger<ProjectsModel> logger, PortfolioDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public List<Projectdb> projects { get; set; }= new();
        public string? Identifier { get; set; }
        public string? Isd { get; set; }
        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                TempData["Error"] = "Please login to continue.";
               Response.Redirect("/Login");
            }
           else
            {
               var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(HttpContext.Session.GetString("token"));
                Identifier = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                Isd = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                
            }            
            projects = await _context.Projects.Where(u => u.ProjectOwnerId == Isd).ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync(Projectdb projectdb, string Category)
        {   
               var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(HttpContext.Session.GetString("token"));
                Isd = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (ModelState.IsValid)
            {
                projectdb.ProjectOwnerId=Isd!;
                projectdb.ProjectCategory = Category;
                _context.Projects.Add(projectdb);
                TempData["Success"] = "Project added successfully.";
                await _context.SaveChangesAsync();
                return RedirectToPage("/User/Projects");
             
            }
            else
            {
                TempData["Error"] = "Please fill in all the fields.";
                return Page();
            }
        }
    }
}