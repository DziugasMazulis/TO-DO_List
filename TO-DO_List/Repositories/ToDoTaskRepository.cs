using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TO_DO_List.Contracts.Repositories;
using TO_DO_List.Data;
using TO_DO_List.Models;

namespace TO_DO_List.Repositories
{
    public class ToDoTaskRepository : IToDoTaskRepository
    {
        private readonly ApplicationContext _applicationContext;

        public ToDoTaskRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<ToDoTask> AddToDoTask(ToDoTask toDoTask)
        {
            var result = await _applicationContext.ToDoTasks.AddAsync(toDoTask);
            await _applicationContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ToDoTask> DeleteToDoTask(int toDoTaskId)
        {
            var result = await _applicationContext.ToDoTasks
                .FirstOrDefaultAsync(t => t.ID == toDoTaskId);
            if (result != null)
            {
                _applicationContext.ToDoTasks.Remove(result);
                await _applicationContext.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<ToDoTask> GetToDoTask(int toDoTaskId)
        {
            return await _applicationContext.ToDoTasks
                .FirstOrDefaultAsync(t => t.ID == toDoTaskId);
        }


        public async Task<IEnumerable<ToDoTask>> GetToDoTaskByUserId(string id)
        {
            return await _applicationContext.ToDoTasks
                .Where(x => x.User.Id.Equals(id))
                .ToListAsync();
        }

        public async Task<IEnumerable<ToDoTask>> GetToDoTasks()
        {
            return await _applicationContext.ToDoTasks.ToListAsync();
        }

        public async Task<ToDoTask> UpdateToDoTask(ToDoTask toDoTask)
        {
            var result = await _applicationContext.ToDoTasks
                .FirstOrDefaultAsync(t => t.ID == toDoTask.ID);

            if (result != null)
            {
                result.IsCompleted = toDoTask.IsCompleted;
                result.Title = toDoTask.Title;
                result.User = toDoTask.User;

                await _applicationContext.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
