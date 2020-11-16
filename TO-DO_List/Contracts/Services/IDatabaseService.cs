using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TO_DO_List.Contracts.Services
{
    public interface IDatabaseService
    {
        public bool EnsureCreated();
        public bool EnsureDeleted();
        public void SeedRoles();
        public void SeedUsers();
        public void SeedTasks();
    }
}
