using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TO_DO_List.Contracts.Services;
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

        [Authorize(Roles="admin")]
        [HttpGet("GetTasks")]
        public async Task<ActionResult> GetToDoTasks()
        {
            try
            {
                var result = await _toDoTaskService.GetToDoTasks();

                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [Authorize(Roles="user")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTaskViewModel>>> GetUserToDoTasks()
        {
            try
            {
                var result = await _toDoTaskService.GetToDoTasksByUser(HttpContext.User);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [Authorize(Roles="user")]
        [HttpPost]
        public async Task<ActionResult<ToDoTaskViewModel>> CreateToDoTask(ToDoTaskDto toDoTask)
        {
            try
            {
                if (toDoTask == null)
                {
                    return BadRequest();
                }

                var result = await _toDoTaskService.AddToDoTask(HttpContext.User, toDoTask);

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [Authorize(Roles="user")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ToDoTaskViewModel>> UpdateToDoTask(int id, ToDoTaskDto toDoTask)
        {
            try
            {
                var result = await _toDoTaskService.UpdateToDoTask(HttpContext.User, id, toDoTask);

                if (result == null)
                {
                    return NotFound($"Task with Id = {id} not found");
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [Authorize(Roles="admin, user")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ToDoTaskViewModel>> DeleteEmployee(int id)
        {
            try
            {
                var result = await _toDoTaskService.DeleteToDoTask(HttpContext.User, id);

                if (result == null)
                {
                    return NotFound($"Task with Id = {id} not found");
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
