using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        #region PLATFORMS
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);
        #endregion

        #region COMMANDS
        IEnumerable<Command> GetByPlatformId(int platformId);
        Command? GetById(int platformId, int commandId);
        void Create(int platformId, Command command);
        #endregion
    }
}