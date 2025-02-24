using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Common;

namespace GymManagement.Domain.Admins.Events
{
    public record SubscriptionDeletedEvent(Guid sbscriptionId) : IDomainEvent;

}