using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TO_DO_List.Models;

namespace TO_DO_List.Data
{
    public class ApplicationContext : IdentityDbContext
    {
        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)
                .WithOne(t => t.User)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        }
    }
}
