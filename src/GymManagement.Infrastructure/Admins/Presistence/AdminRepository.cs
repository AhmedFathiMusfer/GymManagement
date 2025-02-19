
using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Common.Persistence;
using GymManagement.Domain.Admins;
using Microsoft.EntityFrameworkCore;


namespace GymManagement.Infrastructure.Admins.Presistence
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Admin?> GetByIdAsync(Guid adminId)
        {
            return await _dbContext.admins.FirstOrDefaultAsync(a => a.Id == adminId);
        }

        public Task UpdateAsync(Admin admin)
        {
            _dbContext.admins.Update(admin);
            return Task.CompletedTask;
        }


    }
}