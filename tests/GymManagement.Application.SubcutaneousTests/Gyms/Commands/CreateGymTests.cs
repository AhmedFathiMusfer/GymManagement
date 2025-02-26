

using FluentAssertions;
using GymManagement.Application.SubcutaneousTests.Common;
using GymManagement.Domain.Subscriptions;
using MediatR;
using TestCommon.Gyms;

namespace GymManagement.Application.SubcutaneousTests.Gyms.Commands
{

    [Collection(MediatorFactoryCollection.CollectionName)]
    public class CreateGymTests(MediatorFactory mediatorFactory)
    {

        private readonly IMediator _mediator = mediatorFactory.CreateMediator();
        [Fact]
        public async Task CreateGym_WhenValidCommand_ShouldCreateGym()
        {
            // Arrange
            //create a subscription

            var subscription = await CreateSubscription();

            // Create a valid CreateGymCommand
            var createGymCommand = GymCommandFactory.CreateCreateGymCommand(SubscriptionId: subscription.Id);
            // send the create gym command to the mediator

            // Act

            var result = await _mediator.Send(createGymCommand);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.SubscriptionId.Should().Be(subscription.Id);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(31)]
        public async Task CreateGym_WhenCommandContainInvalidData_ShouldReturnValidationError(int gymNameLength)
        {
            string gymName = new('a', gymNameLength);

            var gymCommand = GymCommandFactory.CreateCreateGymCommand(Name: gymName);

            var result = await _mediator.Send(gymCommand);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Name");
        }
        private async Task<Subscription> CreateSubscription()
        {
            // 1. create a subscription command
            var createSubscriptionCommand = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();
            // 2. send the subscription command to the mediator
            var result = await _mediator.Send(createSubscriptionCommand);
            //3. check if the subscription was created successfully
            result.IsError.Should().BeFalse();
            return result.Value;
        }
    }
}