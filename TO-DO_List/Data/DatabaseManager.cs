using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TO_DO_List.Data
{
    public static class DatabaseManager
    {
        public static void InitialiseDatabase(ApplicationContext context)
        {
            if (!context.Database.EnsureCreated())
            {
                if (context.Admins.Any())
                    context.Admins.RemoveRange(context.Admins);
                if (context.Users.Any())
                    context.Users.RemoveRange(context.Users);
                if (context.ToDoTasks.Any())
                    context.ToDoTasks.RemoveRange(context.ToDoTasks);
            }
        }
    }
}
