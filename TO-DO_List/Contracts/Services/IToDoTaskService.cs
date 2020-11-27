using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TO_DO_List.Models.Dto;
using TO_DO_List.ViewModels;

namespace TO_DO_List.Contracts.Services
{
    public interface IToDoTaskService
    {
        Task<IEnumerable<ToDoTaskResponse>> GetToDoTasks(ClaimsPrincipal user);
        Task<ToDoTaskResponse> AddToDoTask(ClaimsPrincipal user, ToDoTaskRequest toDoTaskDto);
        Task<ToDoTaskResponse> UpdateToDoTask(ClaimsPrincipal user, int id, ToDoTaskRequest toDoTask);
        Task<ToDoTaskResponse> DeleteToDoTask(ClaimsPrincipal user, int id);
    }
}
