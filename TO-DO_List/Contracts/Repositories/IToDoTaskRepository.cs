using System.Collections.Generic;
using System.Threading.Tasks;
using TO_DO_List.Models;

namespace TO_DO_List.Contracts.Repositories
{
    public interface IToDoTaskRepository
    {
        public Task<ToDoTask> AddToDoTask(ToDoTask toDoTask);
        public Task<ToDoTask> DeleteToDoTask(int toDoTaskId);
        public Task<ToDoTask> GetToDoTask(int toDoTaskId);
        public Task<IEnumerable<ToDoTask>> GetToDoTaskByUserId(string id);
        public Task<IEnumerable<ToDoTask>> GetToDoTasks();
        public Task<ToDoTask> UpdateToDoTask(ToDoTask toDoTask);
    }
}
