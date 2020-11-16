using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TO_DO_List.Models;
using TO_DO_List.Models.Dto;

namespace TO_DO_List.Settings
{
    public class SeedDataSettings
    {
        public List<UserDto> Users { get; set; }
        public List<ToDoTaskDto> ToDoTasks { get; set; }
    }
}
