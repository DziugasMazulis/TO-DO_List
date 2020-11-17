using System.ComponentModel.DataAnnotations;

namespace TO_DO_List.Models.Dto
{
    public class ToDoTaskRequest
    {
        [Required]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
