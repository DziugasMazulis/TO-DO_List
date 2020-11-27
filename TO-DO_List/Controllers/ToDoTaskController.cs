using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Data;
using TO_DO_List.Models.Dto;
using TO_DO_List.ViewModels;

namespace TO_DO_List.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoTaskController : ControllerBase
    {
        private readonly IToDoTaskService _toDoTaskService;

        public ToDoTaskController(IToDoTaskService toDoTaskService)
        {
            _toDoTaskService = toDoTaskService;
        }

        /// <summary>
        /// Gets all auhtorized existing ToDo tasks.
        /// </summary>
        /// <returns>To admin - all ToDo tasks, to user - all user's ToDo tasks</returns>
        [HttpGet("GetTasks")]
        public async Task<ActionResult> GetToDoTasks()
        {
            try
            {
                var result = await _toDoTaskService.GetToDoTasks(HttpContext.User);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Constants.ErrorRetrieveDatabase);
            }
        }

        /// <summary>
        /// Create a ToDo task.
        /// </summary>
        /// <param name="toDoTask">ToDo task request model</param>
        /// <returns>A newly created ToDo task</returns>
        [Authorize(Roles="user")]
        [HttpPost]
        public async Task<ActionResult<ToDoTaskResponse>> CreateToDoTask(ToDoTaskRequest toDoTask)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = await _toDoTaskService.AddToDoTask(HttpContext.User, toDoTask);

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Constants.ErrorRetrieveDatabase);
            }
        }

        /// <summary>
        /// Updates chosed ToDo task.
        /// </summary>
        /// <param name="id">Chosen ToDo task's ID</param>
        /// <param name="toDoTask">ToDo task's request model to be changed into containing of ToDo task's title and completion status</param>
        /// <returns>Changed ToDo task</returns>
        [Authorize(Roles="user")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ToDoTaskResponse>> UpdateToDoTask(int id, ToDoTaskRequest toDoTask)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = await _toDoTaskService.UpdateToDoTask(HttpContext.User, id, toDoTask);

                if (result == null)
                    return NotFound(string.Format(Constants.TaskNotFound, id));

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Constants.ErrorUpdateDatabase);
            }
        }

        /// <summary>
        /// Deletes a chosen ToDo task.
        /// </summary>
        /// <param name="id">ID of a chosen ToDo task to be deleted</param>
        /// <returns>A deleted ToDo task's response model</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ToDoTaskResponse>> DeleteToDoTask(int id)
        {
            try
            {
                var result = await _toDoTaskService.DeleteToDoTask(HttpContext.User, id);

                if (result == null)
                    return NotFound(string.Format(Constants.TaskNotFound, id));

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Constants.ErrorDeleteDatabase);
            }
        }
    }
}
