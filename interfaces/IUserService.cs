// using User = H.M.Models.User;
using myTasks.Models;
using System.Collections.Generic;

namespace myTasks.Interfaces{

public interface IUserService
{
    List<User> GetAll();

    User GetById(int id);
    
    int Add(User newUser);
 
    bool Update(int id, User newUser);
    
    bool Delete(int id);
}
}