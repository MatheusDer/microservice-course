using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public void Create(int platformId, Command command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
                throw new ArgumentNullException(nameof(platform));
            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
            => _context.Platforms.ToList();

        public Command? GetById(int platformId, int commandId)
            => _context.Commands
                .FirstOrDefault(c => c.Id == commandId && c.PlatformId == platformId);

        public IEnumerable<Command> GetByPlatformId(int platformId)
            => _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform!.Name);

        public bool PlatformExists(int platformId)
            => _context.Platforms.Any(p => p.Id == platformId);

        public bool SaveChanges()
            => _context.SaveChanges() >= 0;
    }
}