using Microsoft.AspNetCore.Mvc;
using myTasks.Interfaces;
using myTasks.Models;
using myTasks.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace mytasks.Controllers{

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    IUserService UserService;
    ITaskTokenService TokenService;
    public UserController(IUserService UserService, ITaskTokenService TokenService)
    {
        this.UserService = UserService;
        this.TokenService = TokenService;
    }
    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        return UserService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var User = UserService.GetById(id);
        if (User == null)
            return NotFound();
        return User;
    }

    // [HttpPost]
    // public ActionResult Post(User newUser)
    // {
    //     var newId = UserService.Add(newUser);

    //     return CreatedAtAction("Post", 
    //         new {id = newId}, UserService.GetById(newId));
    // }

    [HttpPost]
    public ActionResult<string> Login([FromBody] User user)
    {
        var myUser = this.UserService.GetAll().FirstOrDefault(c => c.Id == user.Id);
        if(myUser == null)
            return Unauthorized();
        
        var claims = new List<Claim>
        {
            new Claim("UserType", "user"),
            new Claim("Id",user.Id.ToString()),
        };

        if(user.UserType == UserType.ADMIN)
            claims.Add(new Claim("UserType","admin"));

        var token = TokenService.GetToken(claims);

        return new OkObjectResult(TokenService.WriteToken(token));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,User newUser)
    {
        var result = UserService.Update(id, newUser);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
        public ActionResult Delete(int id)
    {
        var result = UserService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    } 
}
}