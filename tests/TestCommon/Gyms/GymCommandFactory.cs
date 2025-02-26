
using GymManagement.Application.Gyms.Commands.CreateGym;
using TestCommon.TestConstants;

namespace TestCommon.Gyms
{
    public static class GymCommandFactory
    {
        public static CreateGymCommand CreateCreateGymCommand(
           string Name = Constants.Gyms.GymName,
           Guid? SubscriptionId = null
        )
        {
            return new CreateGymCommand(Name, SubscriptionId ?? Constants.Subscriptions.Id);
        }
    }
}