using System.Collections.Generic;
using System.Threading.Tasks;
using TO_DO_List.Models;

namespace TO_DO_List.Contracts.Repositories
{
    public interface IToDoTaskRepository
    {
        Task<ToDoTask> AddToDoTaskAsync(ToDoTask toDoTask);
        Task<ToDoTask> DeleteToDoTaskAsync(int toDoTaskId);
        Task<ToDoTask> GetToDoTaskAsync(int toDoTaskId);
        Task<List<ToDoTask>> GetToDoTaskByUserIdAsync(string id);
        Task<List<ToDoTask>> GetToDoTasksAsync();
        Task<ToDoTask> UpdateToDoTaskAsync(ToDoTask toDoTask);
    }
}
