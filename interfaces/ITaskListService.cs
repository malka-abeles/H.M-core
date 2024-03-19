using Tasklist.Modelssss;
using System.Collections.Generic;


namespace  TaskList.Interface
{
    public interface ITaskListService
    {
        List <Task> GetAll(int userId);

        public Task GetById(int id);

        public Task Add(Task c);

        public Task Update (int id,Task c);

        public void Delete (int id);

        public void DeleteByUserId (int UserId);
    }
    
}
