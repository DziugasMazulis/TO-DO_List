using System.ComponentModel.DataAnnotations.Schema;

namespace TO_DO_List.Models
{
    public class ToDoTask
    {
        //guid?
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
