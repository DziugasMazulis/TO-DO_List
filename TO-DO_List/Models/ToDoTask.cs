using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TO_DO_List.Models
{
    public class ToDoTask
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ToDoTaskID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public virtual User User { get; set; }
    }
}
