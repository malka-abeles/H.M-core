// using Microsoft.AspNetCore.Mvc;
// using myTasks.Interfaces;
// using myTasks.Models;
// using System.Collections.Generic;

// namespace mytasks.Controllers{

// [ApiController]
// [Route("[controller]")]
// public class UserController : ControllerBase
// {
//     IUserService UserService;
//     public UserController(IUserService UserService)
//     {
//         this.UserService = UserService;
//     }
//     [HttpGet]
//     public ActionResult<List<User>> Get()
//     {
//         return UserService.GetAll();
//     }

//     [HttpGet("{id}")]
//     public ActionResult<User> Get(int id)
//     {
//         var User = UserService.GetById(id);
//         if (User == null)
//             return NotFound();
//         return User;
//     }

//     [HttpPost]
//     public ActionResult Post(User newUser)
//     {
//         var newId = UserService.Add(newUser);

//         return CreatedAtAction("Post", 
//             new {id = newId}, UserService.GetById(newId));
//     }

//     [HttpPut("{id}")]
//     public ActionResult Put(int id,User newUser)
//     {
//         var result = UserService.Update(id, newUser);
//         if (!result)
//         {
//             return BadRequest();
//         }
//         return NoContent();
//     }
//     [HttpDelete("{id}")]
//         public ActionResult Delete(int id)
//     {
//         var result = UserService.Delete(id);
//         if (!result)
//         {
//             return BadRequest();
//         }
//         return NoContent();
//     } 
// }
// }