using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GymManagement.Api.IntegrationTests.Common;
using GymManagement.Contracts.Subscriptions;
using TestCommon.TestConstants;
using Throw;
namespace GymManagement.Api.IntegrationTests.Controllers
{
    [Collection(GymManagementApiFactoryCollection.CollectionName)]
    public class CreateSubscriptionTests
    {
        private readonly HttpClient _httpClient;

        public CreateSubscriptionTests(GymManagementApiFactory apiFactory)
        {
            _httpClient = apiFactory.HttpClient;
            apiFactory.ResetDatabase();
        }
        [Theory]
        [MemberData(nameof(listSubscriptionType))]
        public async Task CreateSubscription_WhenValidSubscription_ShouldCreateSubscription(SubscriptionType subscriptionType)
        {
            // Arrange
            var createSubscriptionRequest = new CreateSubscriptionRequest(SubscriptionType: subscriptionType, AdminId: Constants.Admin.Id);

            // Act
            var response = await _httpClient.PostAsJsonAsync("api/Subscription", createSubscriptionRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            var SubscriptionResponse = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();
            response.Headers.Location!.PathAndQuery.Should().Be($"/api/Subscription/{SubscriptionResponse.Id}");
            SubscriptionResponse.Should().NotBeNull();
            SubscriptionResponse!.SubscriptionType.Should().Be(subscriptionType);

        }
        public static TheoryData<SubscriptionType> listSubscriptionType()
        {
            var listSubscriptionType = Enum.GetValues<SubscriptionType>().ToList();
            var TheoryData = new TheoryData<SubscriptionType>();
            foreach (var subscriptionType in listSubscriptionType)
            {
                TheoryData.Add(subscriptionType);
            }
            return TheoryData;
        }

    }
}