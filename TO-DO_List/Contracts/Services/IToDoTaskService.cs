using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TO_DO_List.Models.Dto;
using TO_DO_List.ViewModels;

namespace TO_DO_List.Contracts.Services
{
    public interface IToDoTaskService
    {
        public Task<IEnumerable<ToDoTaskViewModel>> GetToDoTasks();
        public Task<IEnumerable<ToDoTaskViewModel>> GetToDoTasksByUser(ClaimsPrincipal user);
        public Task<ToDoTaskViewModel> AddToDoTask(ClaimsPrincipal user, ToDoTaskDto toDoTaskDto);
        public Task<ToDoTaskViewModel> UpdateToDoTask(ClaimsPrincipal user, int id, ToDoTaskDto toDoTask);
        public Task<ToDoTaskViewModel> DeleteToDoTask(ClaimsPrincipal user, int id);
    }
}
