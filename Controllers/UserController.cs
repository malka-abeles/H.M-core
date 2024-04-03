using Microsoft.AspNetCore.Mvc;
using myTasks.Interfaces;
using myTasks.Models;
using myTasks.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
// using TaskList.Interface;
using System;



// namespace mytasks.Controllers
namespace myTasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUserService UserService;
        ITaskTokenService TokenService;
        ITaskListService TasksService;
        private int userId;
        // IHttpContextAccessor HttpContextAccessor;

        public UserController(IUserService UserService, ITaskTokenService TokenService, ITaskListService TasksService)
        {
            this.UserService = UserService;
            this.TokenService = TokenService;
            this.TasksService = TasksService;
        }

        [HttpPost("/login")]
        public ActionResult<string> Login([FromBody] User user)
        {
            var myUser = this.UserService.GetAll().FirstOrDefault(c =>c.Name==user.Name && c.Password == user.Password);
            if (myUser == null)
            {
                Console.WriteLine("myUser = null");
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
            new Claim("type", "user"),
            new Claim("id",myUser.Id.ToString()),
            };

            if (myUser.UserType == UserType.ADMIN){

                claims.Add(new Claim("type", "admin"));
                System.Console.WriteLine("admin is login");
            }
            var token = TokenService.GetToken(claims);

            return new OkObjectResult(TokenService.WriteToken(token));
        }


        [HttpGet]
        [Authorize(Policy = "admin")]
        public ActionResult<List<User>> GetAll()
        {
            var list = UserService.GetAll();
            if (list == null)
                return NotFound();
            return list;
        }

        [HttpGet("Id")]
        [Authorize(Policy = "user")]
        public ActionResult<User> Get()
        {
            this.userId = int.Parse(User.FindFirst("Id")?.Value);
            var user = UserService.GetById(userId);
            if (user == null)
                return NotFound();
            return user;
        }


        [HttpPost]
        [Authorize(Policy = "admin")]
        public ActionResult Post(User newUser)
        {
            var newId = UserService.Add(newUser);

            return CreatedAtAction("Post",
                new { id = newId }, UserService.GetById(newId));
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public ActionResult Delete(int id)
        {
            var result = UserService.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            TasksService.DeleteByUserId(id);
            return NoContent();
        }
    }
}