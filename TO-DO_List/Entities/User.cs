using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TO_DO_List.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<ToDoTask> ToDoTasks { get; set; }
    }
}
