using AutoMapper;
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
        private readonly IMapper _mapper;

        public ToDoTaskRepository(ApplicationContext applicationContext,
            IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }

        public async Task<ToDoTask> AddToDoTaskAsync(ToDoTask toDoTask)
        {
            var result = await _applicationContext.ToDoTasks
                .AddAsync(toDoTask);
            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<ToDoTask> DeleteToDoTaskAsync(int toDoTaskId)
        {
            var result = await _applicationContext.ToDoTasks
                .FirstOrDefaultAsync(t => t.ID == toDoTaskId);

            if (result == null)
                return result;

            _applicationContext.ToDoTasks.Remove(result);
            await _applicationContext.SaveChangesAsync();

            return result;
        }

        public Task<ToDoTask> GetToDoTaskAsync(int toDoTaskId)
        {
            return _applicationContext.ToDoTasks
                .FirstOrDefaultAsync(t => t.ID == toDoTaskId);
        }


        public Task<List<ToDoTask>> GetToDoTaskByUserIdAsync(string id)
        {
            return _applicationContext.ToDoTasks
                .Where(x => x.User.Id.Equals(id))
                .ToListAsync();
        }

        public Task<List<ToDoTask>> GetToDoTasksAsync()
        {
            return _applicationContext.ToDoTasks
                .ToListAsync();
        }

        public async Task<ToDoTask> UpdateToDoTaskAsync(ToDoTask toDoTask)
        {
            var result = await _applicationContext.ToDoTasks
                .FirstOrDefaultAsync(t => t.ID == toDoTask.ID);

            if (result == null)
                return result;

            _mapper.Map(toDoTask, result);

            await _applicationContext.SaveChangesAsync();

            return result;
        }
    }
}
