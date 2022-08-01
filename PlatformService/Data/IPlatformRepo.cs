using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();

        Task CreateAsync(Platform platform);
        Task<Platform?> GetByIdAsync(int id);
        Task<IEnumerable<Platform>> GetAllAsync();
    }
}