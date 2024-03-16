using Pharmacix.Models.Classes;

namespace Pharmacix.Services;

public class UserAccessor
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly UserRepository _userRepository;

    public UserAccessor(IHttpContextAccessor httpContext, UserRepository userRepository)
    {
        _httpContext = httpContext;
        _userRepository = userRepository;
    }
    
    public User GetCurrentUser()
    {
        var UserId = _httpContext.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value ?? null;
        if (UserId is null) return null;
        var targetUser = _userRepository.GetById(Int32.Parse(UserId)) ?? null;
        return targetUser;
    }
}