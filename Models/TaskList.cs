using System;

// namespace Tasklist.Modelssss
namespace myTasks.Models
{
    public class Task
    {
        public int id { get; set; }

        public String name { get; set; }

        public bool isDone { get; set; }

        public int ownerId { get; set; }
    }
}
