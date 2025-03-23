using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using portfoliomanager.Models;
using portfoliomanager.PortFolioDbContext;

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
        public async Task OnGetAsync()
        {
        projects = await _context.Projects.ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync(Projectdb projectdb, string Category)
        {
            if (ModelState.IsValid)
            {
                projectdb.ProjectCategory = Category;
                _context.Projects.Add(projectdb);
                TempData["Success"] = "Project added successfully.";
                await _context.SaveChangesAsync();
                return RedirectToPage("/Projects");
            }
            else
            {
                TempData["Error"] = "Please fill in all the fields.";
                return Page();
            }
        }
    }
}