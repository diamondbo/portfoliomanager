using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace portfoliomanager.Controllers
{
    public class UserController: Controller
    {
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
    }
}
