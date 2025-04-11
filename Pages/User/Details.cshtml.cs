using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using portfoliomanager.PortFolioDbContexts;
using portfoliomanager.Models;
using Microsoft.EntityFrameworkCore;

namespace portfoliomanager.Pages
{
    public class DetailModel : PageModel
    {
        private readonly PortfolioDbContext _context;
        private readonly ILogger<ProjectsModel> _logger;
        public Projectdb projectdetail { get; set; } = new();
        public string? Category { get; set; }
        public DetailModel(PortfolioDbContext context, ILogger<ProjectsModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if(HttpContext.Session.GetString("token")==null)
            {
                TempData["Error"]="You have to be logged in to view project details";
                return RedirectToPage("/Login");
            }

            projectdetail = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id)
                ?? throw new InvalidOperationException("Project not found.");//catch null error

            if (projectdetail == null)
            {
                TempData["Error"] = "Database Error";
                return RedirectToPage("/User/Projects");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var projectToDelete = await _context.Projects.FindAsync(id);

            if (projectToDelete != null)
            {
                _context.Projects.Remove(projectToDelete);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Project deleted successfully.";
                return RedirectToPage("/User/Projects");
            }

            TempData["Error"] = "Project not found.";
            return RedirectToPage("/User/Projects");
        }
        public async Task<IActionResult> OnPostEditAsync(int id, Projectdb projectdetail, string Category)
        {
            var projectToEdit = await _context.Projects.FindAsync(id);

            if (projectToEdit != null)
            {
                projectToEdit.ProjectName = projectdetail.ProjectName;
                projectToEdit.ProjectDescription = projectdetail.ProjectDescription;
                projectToEdit.ProjectCategory = Category;
                projectToEdit.ProjectUpdated = DateTime.Now;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Project updated successfully.";
                return RedirectToPage("/User/Projects");
            }

            TempData["Error"] = "Project not found.";
            return RedirectToPage("/User/Projects");
        }
    }
}