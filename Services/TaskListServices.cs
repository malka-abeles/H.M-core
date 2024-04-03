using System.Collections.Generic;
// using TaskList.Controllers;
// using TaskList.Controllers;
// using TaskList.Interface;
using myTasks.Services;
using myTasks.Interfaces;
// using Tasklist.Modelssss;
using myTasks.Models;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace myTasks.Services
{
    public class TasksService : ITaskListService
    {
        private List<Task> MyTaskList;

        private string fileName = "./data/Task.json";
        public TasksService()
        {
            this.fileName = Path.Combine("./data/Task.json");

            using (var jsonFile = File.OpenText(fileName))
            {
                MyTaskList = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
                );
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(fileName, JsonSerializer.Serialize(MyTaskList));
        }

        public List<Task> GetAll(int userId) => MyTaskList.Where(i => i.ownerId == userId).ToList();

        public Task GetById(int id)
        {
            return MyTaskList.FirstOrDefault(t => t.id == id);
        }

        public Task Add(Task c)
        {
            if (MyTaskList.Count == 0) c.id = 1;
            else c.id = MyTaskList.Max(p => p.id) + 1;
            MyTaskList.Add(c);
            saveToFile();
            return c;
        }


        public Task Update(int id, Task c)
        {
            if (id != c.id) return null;
            var newchore = GetById(id);
            if (newchore == null) return null;
            int index = MyTaskList.IndexOf(newchore);
            MyTaskList[index] = c;
            saveToFile();
            return c;
        }


        public void Delete(int id)
        {
            var newchore = GetById(id);
            if (newchore == null) return;
            int index = MyTaskList.IndexOf(newchore);
            MyTaskList.RemoveAt(index);
            saveToFile();
        }
        public int Count => MyTaskList.Count();

        public void DeleteByUserId(int UserId)
        {
            MyTaskList = new List<Task>(MyTaskList.Where(i => i.ownerId != UserId));
            saveToFile();
        }

        
    }

    public static class TaskUtils
    {
        public static void AddTask(this IServiceCollection services)
        {
            services.AddSingleton<ITaskListService, TasksService>();
        }
    }

}