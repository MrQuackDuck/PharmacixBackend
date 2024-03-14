using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacix.Models.Classes;
using Pharmacix.Models.Classes.AcceptModels.User;
using Pharmacix.Services;

namespace Pharmacix.Controllers;

public class UserController : Controller
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
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
    public ActionResult<bool> Create(CreateUserModel model)
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
    public ActionResult<bool> Delete(int id)
    {
        var user = _userRepository.GetById(id);

        if (user is null) return NotFound("Provided user wasn't found");
        
        bool success = _userRepository.Delete(id);
        
        if (success) return Ok();
        else return BadRequest();
    }
}