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
        /// Seed roles located in program configuration.
        /// </summary>
        public void SeedRoles()
        {
            foreach(UserSettings userDto in _seedDataSettings.Users)
            {
                var result = _roleManager.RoleExistsAsync(userDto.Role).Result;

                if (!result)
                {
                    var role = new IdentityRole();
                    role.Name = userDto.Role;

                    var roleResult = _roleManager.
                        CreateAsync(role).Result;

                    if (!roleResult.Succeeded)
                        throw new System.InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Seed users located in program configuration.
        /// </summary>
        public void SeedUsers()
        {
            foreach(UserSettings userDto in _seedDataSettings.Users)
            {
                var user = _userManager.FindByEmailAsync(userDto.Username).Result;
                if (user == null)
                {
                    user = new User();
                    user.UserName = userDto.Username;
                    user.Email = userDto.Username;

                    var identityUser = _userManager.CreateAsync(user, userDto.Password).Result;

                    if (identityUser.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, userDto.Role).Wait();
                        _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userDto.Role)).Wait();
                    }
                    else
                        throw new System.InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Seed tasks located in program configuration.
        /// </summary>
        public void SeedTasks()
        {

            foreach (ToDoTaskSettings toDoTaskDto in _seedDataSettings.ToDoTasks)
            {
                var user = _userManager.FindByEmailAsync(toDoTaskDto.User).Result;
               
                if (user != null)
                {
                    var userRoles = _userManager.GetRolesAsync(user);

                    if(userRoles != null &&
                        !userRoles.Result.Contains(Constants.Admin))
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
