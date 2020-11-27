using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TO_DO_List.Contracts.Repositories;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Data;
using TO_DO_List.Models;
using TO_DO_List.Models.Dto;
using TO_DO_List.ViewModels;

namespace TO_DO_List.Services
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly UserManager<User> _userManager;
        private readonly IToDoTaskRepository _toDoTaskRepository;
        private readonly IMapper _mapper;

        public ToDoTaskService(UserManager<User> userManager,
            IToDoTaskRepository toDoTaskRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _toDoTaskRepository = toDoTaskRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ToDoTaskResponse>> GetToDoTasks(ClaimsPrincipal user)
        {
            var currUser = await _userManager.GetUserAsync(user);

            if (currUser == null)
                return null;

            var currUserRoles = await _userManager.GetRolesAsync(currUser);

            if (currUserRoles == null)
                return null;

            List<ToDoTask> result = null;

            if (currUserRoles.Contains(Constants.Admin))
            {
                result = await _toDoTaskRepository.GetToDoTasksAsync();
            }
            else if (currUserRoles.Contains(Constants.User))
            {
                result = await _toDoTaskRepository.GetToDoTaskByUserIdAsync(currUser.Id);
            }

            if (result == null)
                return null;
         
            var toDoTaskResponse = _mapper.Map<List<ToDoTask>, List<ToDoTaskResponse>>(result);

            return toDoTaskResponse;
        }

        public async Task<ToDoTaskResponse> AddToDoTask(ClaimsPrincipal user, ToDoTaskRequest toDoTaskRequest)
        {
            var currUser = await _userManager.GetUserAsync(user);

            if (currUser == null)
                return null;

            var toDoTask = _mapper.Map<ToDoTask>(toDoTaskRequest, opt =>
                opt.AfterMap((src, dest) => dest.User = currUser));
            toDoTask = await _toDoTaskRepository.AddToDoTaskAsync(toDoTask);

            if (toDoTask == null)
                return null;

            var toDoTaskResponse = _mapper.Map<ToDoTaskResponse>(toDoTask);

            return toDoTaskResponse;
        }

        public async Task<ToDoTaskResponse> UpdateToDoTask(ClaimsPrincipal user, int id, ToDoTaskRequest toDoTaskRequest)
        {
            var result = await _toDoTaskRepository.GetToDoTaskAsync(id);

            if (result == null)
                return null;
         
            var currUser = await _userManager.GetUserAsync(user);

            if (currUser == null ||
                result.User == null ||
                result.User.UserName != currUser.UserName)
                return null;

            var toDoTask = _mapper.Map<ToDoTask>(toDoTaskRequest, opt =>
                opt.AfterMap((src, dest) => dest.ID = id));

            result = await _toDoTaskRepository.UpdateToDoTaskAsync(toDoTask);

            if (result == null)
                return null;
         
            var toDoTaskResponse = _mapper.Map<ToDoTaskResponse>(result);

            return toDoTaskResponse;
        }
        public async Task<ToDoTaskResponse> DeleteToDoTask(ClaimsPrincipal user, int id)
        {
            var toDoTask = await _toDoTaskRepository.GetToDoTaskAsync(id);

            if (toDoTask == null)
                return null;

            var currUser = await _userManager.GetUserAsync(user);

            if (currUser == null)
                return null;

            var currUserRoles = await _userManager.GetRolesAsync(currUser);

            if (currUserRoles == null)
                return null;

            if (!(toDoTask.User.UserName == currUser.UserName && currUserRoles.Contains(Constants.User)) &&
                !currUserRoles.Contains(Constants.Admin))
                return null;

            var toDoTaskResult = await _toDoTaskRepository.DeleteToDoTaskAsync(id);

            if (toDoTaskResult == null)
                return null;

            var toDoTaskResponse = _mapper.Map<ToDoTaskResponse>(toDoTask);

            return toDoTaskResponse;
        }
    }
}
