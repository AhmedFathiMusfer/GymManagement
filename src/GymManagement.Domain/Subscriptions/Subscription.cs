using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GymManagement.Domain.Subscriptions
{
    public class Subscription
    {
        public Guid Id { get; }

        public SubscriptionType SubscriptionType { get; }

        private readonly Guid _adminId;

        private readonly List<Guid> _gymIds = [];

        public Subscription(
            SubscriptionType subscriptionType,
            Guid adminId,
             Guid? id = null
        )
        {
            Id = id ?? Guid.NewGuid();
            _adminId = adminId;
            SubscriptionType = subscriptionType;

        }
        private Subscription()
        {

        }
        public bool HasGym(Guid gymId)
        {
            var gym = _gymIds.FirstOrDefault(g => g == gymId);
            if (gym == null)
            {
                return false;
            }
            return true;
        }
        public void DeleteGym(Guid gymId)
        {
            _gymIds.Remove(gymId);
        }
    }
}