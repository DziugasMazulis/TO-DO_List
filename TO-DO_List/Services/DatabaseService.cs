using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Data;
using TO_DO_List.Models;
using TO_DO_List.Models.Dto;
using TO_DO_List.Settings;

namespace TO_DO_List.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ILogger<DatabaseService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<SeedDataSettings> _seedDataSettings;
        private readonly ApplicationContext _applicationContext;

        public DatabaseService(ILogger<DatabaseService> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IOptions<SeedDataSettings> seedDataSettings,
            ApplicationContext applicationContext)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _seedDataSettings = seedDataSettings;
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SeedRoles()
        {
            var seedData = _seedDataSettings.Value;

            foreach(UserDto userDto in seedData.Users)
            {
                //exceptions handling and logging
                if (!_roleManager.RoleExistsAsync(userDto.Role).Result)
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = userDto.Role
                    };
                    IdentityResult roleResult = _roleManager.
                    CreateAsync(role).Result;
                    //check role result
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SeedUsers()
        {
            var seedData = _seedDataSettings.Value;

            foreach(UserDto userDto in seedData.Users)
            {
                //exceptions handling and logging
                if (_userManager.FindByEmailAsync(userDto.Username).Result == null)
                {
                    User user = new User
                    {
                        UserName = userDto.Username,
                        Email = userDto.Username
                    };

                    IdentityResult result = _userManager.CreateAsync(user, userDto.Password).Result;

                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, userDto.Role).Wait();
                        _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userDto.Role)).Wait();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SeedTasks()
        {
            var seedData = _seedDataSettings.Value;

            foreach (ToDoTaskDto toDoTaskDto in seedData.ToDoTasks)
            {
                //exceptions handling and logging
                var user = _userManager.FindByEmailAsync(toDoTaskDto.User).Result;
               
                if (user != null)
                {
                    var userRoles = _userManager.GetRolesAsync(user);
                    //contants class
                    if(!userRoles.Result.Contains("admin"))
                    {
                        ToDoTask toDoTask = new ToDoTask
                        {
                            Title = toDoTaskDto.Title,
                            IsCompleted = toDoTaskDto.IsCompleted,
                            User = user
                        };

                        _applicationContext.ToDoTasks.Add(toDoTask);
                    }
                }
            }

            _applicationContext.SaveChanges();
        }

        /// <summary>
        /// Ensure that the database for the content exists. If not then the database is created.
        /// </summary>
        /// <returns>True if database is created, false if it already existed.</returns>
        public bool EnsureCreated() => _applicationContext.Database.EnsureCreated();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool EnsureDeleted() => _applicationContext.Database.EnsureDeleted();
    }
}
