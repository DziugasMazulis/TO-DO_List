using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TO_DO_List.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public virtual ICollection<ToDoTask> Tasks { get; set; }
    }
}
