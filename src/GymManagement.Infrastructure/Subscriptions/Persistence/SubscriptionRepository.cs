using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Subscriptions.Persistence
{
    public class SubscriptionRepository : ISubscriptionsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SubscriptionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateSubscription(Subscription subscription)
        {
            await _dbContext.subscriptions.AddAsync(subscription);

            return;
        }

        public async Task<Subscription?> GetSubscription(Guid subscriptionId)
        {
            return await _dbContext.subscriptions.FirstOrDefaultAsync(s => s.Id == subscriptionId);
        }

        public async Task RemoveSubscription(Subscription subscription)
        {
            _dbContext.subscriptions.Remove(subscription);

        }

        public async Task UpdateSubscription(Subscription subscription)
        {
            _dbContext.subscriptions.Update(subscription);
        }
    }
}