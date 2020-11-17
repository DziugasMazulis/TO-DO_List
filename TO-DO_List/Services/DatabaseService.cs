using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TO_DO_List.Contracts.Services;
using TO_DO_List.Data;
using TO_DO_List.Models;
using TO_DO_List.Models.Dto;
using TO_DO_List.Settings;

namespace TO_DO_List.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SeedDataSettings _seedDataSettings;
        private readonly ApplicationContext _applicationContext;

        public DatabaseService(RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IOptions<SeedDataSettings> seedDataSettings,
            ApplicationContext applicationContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _seedDataSettings = seedDataSettings.Value;
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SeedRoles()
        {
            foreach(UserDto userDto in _seedDataSettings.Users)
            {
                //exceptions handling
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
            foreach(UserDto userDto in _seedDataSettings.Users)
            {
                //exceptions handling
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

            foreach (ToDoTaskSettingsDto toDoTaskDto in _seedDataSettings.ToDoTasks)
            {
                //exceptions handling
                var user = _userManager.FindByEmailAsync(toDoTaskDto.User).Result;
               
                if (user != null)
                {
                    var userRoles = _userManager.GetRolesAsync(user);
                    if(!userRoles.Result.Contains(Constants.Admin))
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
