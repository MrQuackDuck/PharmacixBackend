using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacix.Models.Classes;
using Pharmacix.Models.Classes.AcceptModels.User;
using Pharmacix.Services;

namespace Pharmacix.Controllers;

[Route("API/[controller]/[action]")]
public class UserController : Controller
{
    private readonly UserRepository _userRepository;
    private readonly UserAccessor _userAccessor;

    public UserController(UserRepository userRepository, UserAccessor userAccessor)
    {
        _userRepository = userRepository;
        _userAccessor = userAccessor;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<List<User>> GetAll()
    {
        return _userRepository.GetAll();
    }
    
    [HttpGet]
    [Authorize]
    public ActionResult<User> GetById(int id)
    {
        var target = _userRepository.GetById(id) ?? null;
        if (target is null) return NotFound();
        return target;
    }

    [HttpPost]
    [Authorize]
    public ActionResult<bool> Create([FromBody]CreateUserModel model)
    {
        if (model is null) return BadRequest();

        var success = this._userRepository.Create(
            new User()
            {
                Username = model.Username,
                PasswordHash = _userRepository.EncryptSha256(model.Password)
            });

        if (success) return Ok();
        else return BadRequest();
    }
    
    [HttpDelete]
    [Authorize]
    [Route("API/User/Delete/{id}")]
    public ActionResult<bool> Delete(int id)
    {
        var user = _userRepository.GetById(id);

        if (user is null) return NotFound("Provided user wasn't found");
        if (_userAccessor.GetCurrentUser().Id == user.Id) return BadRequest("You can't delete yourself!");
        
        bool success = _userRepository.Delete(user);
        
        if (success) return Ok();
        else return BadRequest();
    }
}