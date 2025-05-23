using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using portfoliomanager.Models;
using portfoliomanager.PortFolioDbContexts;


namespace portfoliomanager.Pages
{
    public class LoginModel(IConfiguration configuration, PortfolioDbContext _context) : PageModel
    {
        private readonly PasswordHasher<Userdb> _passwordHasher = new();
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public string? Password { get; set; }
        public static HttpClient client = new HttpClient();
        public void OnGet()
        {
            var Username = HttpContext.Session.GetString("token");
            if (Username != null)
            {
                TempData["Error"]="You are already logged in";
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var authuser = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (authuser == null)
            {
                TempData["Error"]="Invalid email";
                return Page();
            }
            var result = _passwordHasher.VerifyHashedPassword(authuser, authuser.Passwordhash!, Password!); 
            if (result == PasswordVerificationResult.Success)
            {
                var tok = CreateToken(authuser);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tok);
                HttpContext.Session.SetString("token", tok);
                HttpContext.Session.SetString("Username", authuser.Username!);
                if (authuser.Admin == true)
                {
                    HttpContext.Session.SetString("Role", "Admin");
                    return RedirectToPage("/User/UserIndex");
                }
                return RedirectToPage("/User/UserIndex");
            }
            else
            {
                TempData["Error"]="Invalid password";
                return Page();
            }
        }
        public string CreateToken(Userdb user)
        {
            var role = "";
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Admin == true)
            {
                role = "Admin";
            }
            else
            {
                role = "User";
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.LastName + " " + user.FirstName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role),
            };
            var Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}