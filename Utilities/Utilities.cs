using Microsoft.Extensions.DependencyInjection;
using TaskList.Interface;
using TaskList.Services;
using myTasks.Interfaces;
using myTasks.Services;
using System;
// using myTasks.Services;

namespace Utilities{
    public static  class Utilities{
        public static void AddTask(this IServiceCollection services)
        {
            services.AddSingleton<ITaskListService, TasksService>();
        }

        public static void AddUser(this IServiceCollection services)
        {
            services.AddSingleton<IUserService,UserService>();
        }
    }
}
