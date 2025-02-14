using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Common.Interfaces
{
    public interface ISubscriptionsRepository
    {
        Task CreateSubscription(Subscription subscription);
    }
}