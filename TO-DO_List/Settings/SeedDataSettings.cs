using System.Collections.Generic;
using TO_DO_List.Models.Dto;

namespace TO_DO_List.Settings
{
    public class SeedDataSettings
    {
        public List<UserSettings> Users { get; set; }
        public List<ToDoTaskSettings> ToDoTasks { get; set; }
    }
}
