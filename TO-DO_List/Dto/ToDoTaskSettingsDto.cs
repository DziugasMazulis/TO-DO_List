using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TO_DO_List.Models.Dto
{
    public class ToDoTaskSettingsDto
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public string User { get; set; }
    }
}
