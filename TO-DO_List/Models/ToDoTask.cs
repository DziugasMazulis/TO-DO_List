using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TO_DO_List.Models
{
    public class ToDoTask
    {
        //guid?
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public virtual User User { get; set; }
    }
}
