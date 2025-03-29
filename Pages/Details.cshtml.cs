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
        public DetailModel(PortfolioDbContext context, ILogger<ProjectsModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Project ID is missing.";
                return RedirectToPage("/Projects");
            }

            projectdetail = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id)
                ?? throw new InvalidOperationException("Project not found.");//catch null error

            if (projectdetail == null)
            {
                TempData["Error"] = "Database Error";
                return RedirectToPage("/Projects");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var projectToDelete = await _context.Projects.FindAsync(id);

            if (projectToDelete != null)
            {
                _context.Projects.Remove(projectToDelete);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Project deleted successfully.";
                return RedirectToPage("/Projects");
            }

            TempData["Error"] = "Project not found.";
            return RedirectToPage("/Projects");
        }
    }
}