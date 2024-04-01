using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.Interface;
using Tasklist.Modelssss;
using Task = Tasklist.Modelssss.Task;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace TaskList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskListController : ControllerBase
    {
        ITaskListService TaskListService;

        private int userId;
        public TaskListController(ITaskListService TaskListService,IHttpContextAccessor httpContextAccessor)
        {
            this.TaskListService = TaskListService;
            this.userId=int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value);
        }

        [HttpGet]
        [Authorize(Policy = "user")]
        public ActionResult<List<Task>> Get()
        {
            // int id=int.Parse(User.FindFirst("id")?.Value);
                        // System.Console.WriteLine(id);

            return TaskListService.GetAll(this.userId);
            // System.Console.WriteLine(newlist.Size());
            // return newlist;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "user")]
        public ActionResult<Task> Get(int id)
        {
            var task = TaskListService.GetById(id);
            if (task == null)
                return NotFound();
            if (!chekAuthorization(id))
                return Unauthorized();
            return task;
        }

        [HttpPost]
        [Authorize(Policy = "user")]
        public ActionResult Post(Task newTask)
        {
            var newId = TaskListService.Add(newTask);
            newTask.ownerId = int.Parse(User.FindFirst("id").Value);
            return CreatedAtAction(nameof(Post), new { id = newId }, newTask);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "user")]
        public ActionResult Put(int id, Task newTask)
        {

            newTask.ownerId = int.Parse(User.FindFirst("id").Value);
            var result = TaskListService.Update(id, newTask);
            if (result == null)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "user")]
        public ActionResult delete(int id)
        {
            var task = TaskListService.GetById(id);
            if (task == null)
                return NotFound();
            if (!chekAuthorization(id))
                return Unauthorized();
            TaskListService.Delete(id);
            return NoContent();
        }


        private bool chekAuthorization(int taskId)
        {
            return TaskListService.GetById(taskId).ownerId == int.Parse(User.FindFirst("id").Value);
        }

    }
}
