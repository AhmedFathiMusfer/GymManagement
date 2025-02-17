using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Domain.Subscriptions;
using MediatR;
using Microsoft.VisualBasic;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription
{
    public record CreateSubscriptionCommand(Guid AdminId,
   SubscriptionType SubscriptionType) : IRequest<ErrorOr<Subscription>>;

}