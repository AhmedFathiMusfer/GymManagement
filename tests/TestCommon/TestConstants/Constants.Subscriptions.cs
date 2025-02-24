using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Subscriptions;

namespace TestCommon.TestConstants
{
    public static partial class Constants
    {
        public static class Subscriptions
        {
            public static readonly SubscriptionType DefaultSubscriptionType = SubscriptionType.Free;
            public static readonly Guid Id = Guid.NewGuid();

            public const int MaxSessionsFreeTire = 3;
            public const int MaxRoomsFreeTire = 1;
            public const int MaxGymsFreeTire = 1;
        }
    }
}