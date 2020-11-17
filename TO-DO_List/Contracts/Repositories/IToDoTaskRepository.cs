using System.Collections.Generic;
using System.Threading.Tasks;
using TO_DO_List.Models;

namespace TO_DO_List.Contracts.Repositories
{
    public interface IToDoTaskRepository
    {
        Task<ToDoTask> AddToDoTask(ToDoTask toDoTask);
        Task<ToDoTask> DeleteToDoTask(int toDoTaskId);
        Task<ToDoTask> GetToDoTask(int toDoTaskId);
        Task<IEnumerable<ToDoTask>> GetToDoTaskByUserId(string id);
        Task<IEnumerable<ToDoTask>> GetToDoTasks();
        Task<ToDoTask> UpdateToDoTask(ToDoTask toDoTask);
    }
}
