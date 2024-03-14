using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pharmacix.Models.Classes.AcceptModels;
using Pharmacix.Services;

namespace Pharmacix.Controllers;

public class AuthController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly UserRepository _userRepository;

    public AuthController(IConfiguration configuration, UserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }
    
    [HttpPost]
    public ActionResult Login([FromBody]LoginModel userData)
    {
        var user = _userRepository.GetByUsername(userData?.Username) ?? null;
        
        // If the user is not found
        if (user is null) 
            return Unauthorized();

        // If user's password is correct
        if (!_userRepository.IsPasswordCorrect(userData.Username, userData.Password)) 
            return Problem("Incorrect password provided");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Id", user.Id.ToString()),
            new Claim("Username", userData.Username),
        };
        
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));
        
        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        
        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        });
        
        return Ok(new { token });
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok();
    }
}