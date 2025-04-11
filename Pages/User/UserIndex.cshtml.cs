using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace portfoliomanager.Pages
{
    public class UserIndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if(HttpContext.Session.GetString("token")==null)
            {
                TempData["Error"]="You have to be logged in to view project details";
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}