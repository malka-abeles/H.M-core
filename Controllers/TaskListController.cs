using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.Interface;
using Tasklist.Modelssss;
using Task = Tasklist.Modelssss.Task;

namespace TaskList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskListController : ControllerBase
    {
        ITaskListService TaskListService;
    public TaskListController(ITaskListService TaskListService)
    {
        this.TaskListService = TaskListService;
    }

    [HttpGet]
    public ActionResult<List<Task>> Get()
    {
        return TaskListService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Task> Get(int id)
    {
        var task = TaskListService.GetById(id);
        if (task == null)
            return NotFound();
        return task;
    }

    [HttpPost]
    public ActionResult Post(Task newTask)
    {
        var newId = TaskListService.Add(newTask);

        return CreatedAtAction("Post", 
            new {id = newId}, TaskListService.GetById(newId.id));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,Task newTask)
    {
        var result = TaskListService.Update(id, newTask);
        if (result==null)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult delete (int id)
    {
        TaskListService.Delete(id);
        return NoContent();
    } 

    }
}
