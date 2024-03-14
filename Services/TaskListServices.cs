using System.Collections.Generic;
using TaskList.Controllers;
using TaskList.Interface;
using Tasklist.Modelssss;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Json;


namespace TaskList.Services
{
     public class TasksService : ITaskListService
    {
        private List<Task> MyTaskList;

        private string fileName = "Task.json";
        public TasksService(/*IWebHostEnvinronment webHost*/)
        {
            this.fileName = Path.Combine(/*webHost.ContentRootPath,*/  "Task.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                MyTaskList = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(MyTaskList));
        }

        public List<Task> GetAll()=>MyTaskList;

        public Task GetById(int id) 
        {
        return MyTaskList.FirstOrDefault(p => p.id == id);
        }

        public Task Add(Task c)
        {
            if (MyTaskList.Count==0) c.id=1;
            else c.id = MyTaskList.Max(p => p.id)+1;
            MyTaskList.Add(c);
            saveToFile();
            return c;
        }


        public Task Update (int id,Task c)
        {
            if(id != c.id) return null;
            var newchore = GetById(id);
            if(newchore == null) return null;
            int index = MyTaskList.IndexOf(newchore);
            MyTaskList[index]=c;
            saveToFile();
            return c;
        }


        public void Delete (int id)
        {
             var newchore = GetById(id);
            if(newchore == null) return;
            int index = MyTaskList.IndexOf(newchore);
            MyTaskList.RemoveAt(index);
            saveToFile();
        }
        public int Count => MyTaskList.Count();
    }

    public static class TaskUtils
    {
        public static void AddTask(this IServiceCollection services)
        {
            services.AddSingleton<ITaskListService, TasksService>();
        }
    }

}