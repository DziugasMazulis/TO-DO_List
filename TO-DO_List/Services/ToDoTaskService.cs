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

        public ToDoTaskService(UserManager<User> userManager,
            IToDoTaskRepository toDoTaskRepository)
        {
            _userManager = userManager;
            _toDoTaskRepository = toDoTaskRepository;
        }

        public async Task<IEnumerable<ToDoTaskViewModel>> GetToDoTasks()
        {
            var result = await _toDoTaskRepository.GetToDoTasks();

            if (result != null)
            {
                var toDoTasksViewModel = new List<ToDoTaskViewModel>();

                foreach (var task in result)
                {
                    var toDoTaskUser = await _userManager.FindByIdAsync(task.UserId);

                    if (toDoTaskUser != null)
                    {
                        var toDoTaskViewModel = MapModelToViewModel(task, toDoTaskUser.UserName);

                        toDoTasksViewModel.Add(toDoTaskViewModel);
                    }
                }

                return toDoTasksViewModel;
            }

            return null;
        }

        public async Task<IEnumerable<ToDoTaskViewModel>> GetToDoTasksByUser(ClaimsPrincipal user)
        {
            var currUser = await _userManager.GetUserAsync(user);

            if (currUser != null)
            {
                var toDoTasks = await _toDoTaskRepository.GetToDoTaskByUserId(currUser.Id);

                if (toDoTasks != null)
                {
                    var toDoTasksViewModel = new List<ToDoTaskViewModel>();

                    foreach (var task in toDoTasks)
                    {
                        var toDoTaskViewModel = MapModelToViewModel(task, task.User.UserName);
                        toDoTasksViewModel.Add(toDoTaskViewModel);
                    }

                    return toDoTasksViewModel;
                }
            }

            return null;
        }
        public async Task<ToDoTaskViewModel> AddToDoTask(ClaimsPrincipal user, ToDoTaskDto toDoTaskDto)
        {
            var currUser = await _userManager.GetUserAsync(user);

            if (currUser != null)
            {
                var toDoTask = new ToDoTask
                {
                    IsCompleted = toDoTaskDto.IsCompleted,
                    Title = toDoTaskDto.Title,
                    User = currUser
                };

                toDoTask = await _toDoTaskRepository.AddToDoTask(toDoTask);

                if(toDoTask != null)
                {
                    var toDoTaskViewModel = MapModelToViewModel(toDoTask, toDoTask.User.UserName);

                    return toDoTaskViewModel;
                }
            }

            return null;
        }

        public async Task<ToDoTaskViewModel> UpdateToDoTask(ClaimsPrincipal user, int id, ToDoTaskDto toDoTask)
        {
            var result = await _toDoTaskRepository.GetToDoTask(id);

            if (result != null)
            {
                var currUser = await _userManager.GetUserAsync(user);

                if (currUser != null &&
                    result.User != null &&
                    result.User.UserName == currUser.UserName)
                {
                    result.IsCompleted = toDoTask.IsCompleted;
                    result.Title = toDoTask.Title;

                    result = await _toDoTaskRepository.UpdateToDoTask(result);

                    if(result != null)
                    {
                        var toDoTaskViewModel = MapModelToViewModel(result, result.User.UserName);

                        return toDoTaskViewModel;
                    }
                }
            }

            return null;
        }
        public async Task<ToDoTaskViewModel> DeleteToDoTask(ClaimsPrincipal user, int id)
        {
            var toDoTask = await _toDoTaskRepository.GetToDoTask(id);

            if (toDoTask != null)
            {
                var currUser = await _userManager.GetUserAsync(user);

                if (currUser != null)
                {
                    var currUserRoles = await _userManager.GetRolesAsync(currUser);
                    
                    if(currUserRoles != null)
                    {
                        string userName;

                        if (currUserRoles.Contains(Constants.Admin))
                        {
                            var toDoTaskUser = await _userManager.FindByIdAsync(toDoTask.UserId);
                            userName = toDoTaskUser.UserName;
                        }
                        else if (toDoTask.User != null &&
                            toDoTask.User.UserName == currUser.UserName)
                        {
                            userName = toDoTask.User.UserName;
                        }
                        else
                        {
                            return null;
                        }

                        toDoTask = await _toDoTaskRepository.DeleteToDoTask(id);

                        if (toDoTask != null)
                        {
                            var toDoTaskViewModel = MapModelToViewModel(toDoTask, userName);

                            return toDoTaskViewModel;
                        }
                    }
                }
            }

            return null;
        }

        //when automapper quits on you
        private static ToDoTaskViewModel MapModelToViewModel(ToDoTask toDoTask, string userName)
        {
            var toDoTaskViewModel = new ToDoTaskViewModel();

            toDoTaskViewModel.ID = toDoTask.ID;
            toDoTaskViewModel.IsCompleted = toDoTask.IsCompleted;
            toDoTaskViewModel.Title = toDoTask.Title;
            toDoTaskViewModel.User = userName;

            return toDoTaskViewModel;
        }
    }
}
