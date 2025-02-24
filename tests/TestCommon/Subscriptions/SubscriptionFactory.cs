

using GymManagement.Domain.Subscriptions;

using TestCommon.TestConstants;
namespace TestCommon.Subscriptions
{
    public class SubscriptionFactory
    {
        public static Subscription CreateSubscription(
            SubscriptionType? subscriptionType = null,
            Guid? Id = null,
            Guid? AdminId = null
        )
        {
            return new Subscription(
                subscriptionType ?? Constants.Subscriptions.DefaultSubscriptionType,
                AdminId ?? Constants.Admin.Id,
                Id ?? Constants.Subscriptions.Id
            );
        }
    }
}