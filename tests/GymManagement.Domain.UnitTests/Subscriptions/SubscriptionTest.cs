

using TestCommon.Gyms;
using TestCommon.Subscriptions;
using FluentAssertions;
using ErrorOr;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Domain.UnitTests.Subscriptions
{
    public class SubscriptionTest
    {
        [Fact]
        public void AddGym_WhenMoreThanSubscriptionAllowes_ShouldFail()
        {
            // Arrange
            var subscription = SubscriptionFactory.CreateSubscription();
            var gyms = Enumerable.Range(0, subscription.GetMaxGyms() + 1)
            .Select(_ => GymFactory.CreateGym(Id: Guid.NewGuid()))
            .DistinctBy(gym => gym.Id)
            .ToList();


            // Act
            var addGymResults = gyms.ConvertAll(subscription.AddGym);
            var allButLastGymResults = addGymResults[..^1];
            allButLastGymResults.Should().AllSatisfy(addGymResult => addGymResult.Value.Should().Be(Result.Success));

            var lastGymResult = addGymResults.Last();
            lastGymResult.IsError.Should().BeTrue();
            lastGymResult.FirstError.Should().Be(SubscriptionErrors.CannotHaveMoreGymsThanTheSubscriptionAllows);


        }
    }
}