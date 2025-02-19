
using ErrorOr;
using GymManagement.Domain.Gyms;
using Throw;

namespace GymManagement.Domain.Subscriptions
{
    public class Subscription
    {
        public Guid Id { get; }
        private readonly int _maxGyms;

        public SubscriptionType SubscriptionType { get; }


        public Guid AdminId { get; }
        private readonly List<Guid> _gymIds = [];

        public Subscription(
            SubscriptionType subscriptionType,
            Guid adminId,
             Guid? id = null
        )
        {
            Id = id ?? Guid.NewGuid();
            AdminId = adminId;
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
            _gymIds.Throw().IfNotContains(gymId);
            _gymIds.Remove(gymId);
        }

        public ErrorOr<Success> AddGym(Gym gym)
        {
            _gymIds.Throw().IfContains(gym.Id);

            if (_gymIds.Count >= _maxGyms)
            {
                return SubscriptionErrors.CannotHaveMoreGymsThanTheSubscriptionAllows;
            }

            _gymIds.Add(gym.Id);

            return Result.Success;
        }

        public int GetMaxGyms() => SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 1,
            nameof(SubscriptionType.Starter) => 1,
            nameof(SubscriptionType.Pro) => 3,
            _ => throw new InvalidOperationException()
        };

        public int GetMaxRooms() => SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 1,
            nameof(SubscriptionType.Starter) => 3,
            nameof(SubscriptionType.Pro) => int.MaxValue,
            _ => throw new InvalidOperationException()
        };

        public int GetMaxDailySessions() => SubscriptionType.Name switch
        {
            nameof(SubscriptionType.Free) => 4,
            nameof(SubscriptionType.Starter) => int.MaxValue,
            nameof(SubscriptionType.Pro) => int.MaxValue,
            _ => throw new InvalidOperationException()
        };
    }
}