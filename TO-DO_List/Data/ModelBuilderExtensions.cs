using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TO_DO_List.Models;

namespace TO_DO_List.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    ID = 1,
                    Username = "Username"
                }
            );

            modelBuilder.Entity<ToDoTask>().HasData(
                new ToDoTask
                {
                    ToDoTaskID = 1,
                    Title = "Title1",
                    Text = "Text1"
                },
                new ToDoTask
                {
                    ToDoTaskID = 2,
                    Title = "Title2",
                    Text = "Text2"
                }
            );
        }
    }
}
