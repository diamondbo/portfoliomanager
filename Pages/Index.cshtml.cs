using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace portfoliomanager.Pages;


public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string? Token { get; set; }
    public DateTime? Issuedat { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet(string? token)
    {
        if (token != null)
        {
            Token = token;
            var handler = new JwtSecurityTokenHandler();
          if(handler.CanReadToken(token) == true)
          {
            var jwttoken = handler.ReadJwtToken(token);

            Issuedat = jwttoken.ValidTo;
          }
          else
          {
            Issuedat = null;
          }
        }
        
    }
}
