namespace TO_DO_List.Contracts.Services
{
    public interface IDatabaseService
    {
        bool EnsureCreated();
        bool EnsureDeleted();
        void SeedRoles();
        void SeedUsers();
        void SeedTasks();
    }
}
