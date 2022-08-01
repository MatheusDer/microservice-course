using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Platform platform)
        {
            await _context.Platforms.AddAsync(platform);
        }

        public async Task<IEnumerable<Platform>> GetAll()
            => await _context.Platforms.ToListAsync();

        public async Task<Platform?> GetByIdAsync(int id)
            => await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);

        public bool SaveChanges()
            => (_context.SaveChanges() >= 0);
    }
}