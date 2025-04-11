using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using portfoliomanager.Models;
using portfoliomanager.PortFolioDbContexts;

namespace portfoliomanager.Controllers
{
    
    public class UserController: Controller
    {
        private readonly PortfolioDbContext _context;
        public List<Userdb> UserList { get; set; } = new();
        public List<Projectdb> PList { get; set; } = new();
        public UserController(PortfolioDbContext context)
        {
            _context = context;
        }

        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                TempData["error"] = "You are not logged in.";
                TempData["Token"] = null;
                return RedirectToPage("/Index");
            }
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
        [HttpGet("/ViewUsers")]
        public IActionResult ViewUsers()
        {
            UserList = _context.Users.ToList();
            return Ok(UserList);
        }

        [HttpGet("/View_projects")]
        public IActionResult ViewProjects()
        {
            PList = _context.Projects.ToList();
            return Ok(PList);
        }
    }
}
