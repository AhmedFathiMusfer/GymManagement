using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Gyms;
using TestCommon.TestConstants;

namespace TestCommon.Gyms
{
    public static class GymFactory
    {
        public static Gym CreateGym(
            string name = Constants.Gyms.GymName,
            int maxRoom = Constants.Subscriptions.MaxRoomsFreeTire,
            Guid? Id = null
        )
        {
            return new Gym(name,
             maxRoom,
              Constants.Subscriptions.Id,
              Id ?? Constants.Gyms.Id
            );
        }
    }
}