using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Gyms
{
    public class GymRepository : IGymRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GymRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddGymAsync(Gym gym)
        {
            await _dbContext.gyms.AddAsync(gym);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var gym = _dbContext.gyms.FirstOrDefault(g => g.Id == id);
            if (gym == null)
            {
                return false;
            }
            return true;

        }

        public async Task<Gym?> GetByIdAsync(Guid id)
        {
            return _dbContext.gyms.FirstOrDefault(g => g.Id == id);
        }

        public async Task<List<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId)
        {
            return await _dbContext.gyms.Where(g => g.SubscriptionId == subscriptionId).ToListAsync();
        }

        public async Task RemoveGymAsync(Gym gym)
        {
            _dbContext.gyms.Remove(gym);

        }

        public async Task RemoveRangeAsync(List<Gym> gyms)
        {
            _dbContext.gyms.RemoveRange(gyms);
        }

        public async Task UpdateGymAsync(Gym gym)
        {
            _dbContext.gyms.Update(gym);
        }
    }
}