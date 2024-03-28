using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pharmacix.Models.Classes;
using Pharmacix.Models.Classes.AcceptModels;
using Pharmacix.Models.Classes.AcceptModels.Misc;
using Pharmacix.Services;

namespace Pharmacix.Controllers;

[Route("API/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly UserRepository _userRepository;
    private readonly UserAccessor _userAccessor;

    public AuthController(IConfiguration configuration, UserRepository userRepository, UserAccessor userAccessor)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _userAccessor = userAccessor;
    }
    
    [HttpPost]
    public ActionResult Login([FromBody]LoginModel userData)
    {
        var user = _userRepository.GetByUsername(userData?.Username) ?? null;
        
        // If the user is not found
        if (user is null) 
            return Unauthorized("Incorrect username or password provided");

        // If user's password is correct
        if (!_userRepository.IsPasswordCorrect(userData.Username, userData.Password)) 
            return Unauthorized("Incorrect username or password provided");

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
    public ActionResult CurrentUser()
    {
        User user = _userAccessor.GetCurrentUser();

        if (user is null) return Unauthorized();
        
        return Ok(new { user.Id, user.Username });
    }
    
    [HttpPost]
    [Authorize]
    public ActionResult ChangePassword([FromBody]ChangePasswordModel model)
    {
        var oldPassword = model.OldPassword;
        var newPassword = model.NewPassword;

        var user = _userAccessor.GetCurrentUser();
        bool result = _userRepository.ChangePassword(user.Id, oldPassword, newPassword);
        
        if (result)
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }
        else return Forbid();
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok();
    }
}