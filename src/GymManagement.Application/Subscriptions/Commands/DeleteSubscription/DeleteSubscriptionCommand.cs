using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription
{
    public record DeleteSubscriptionCommand(Guid subscriptionId) : IRequest<ErrorOr<Deleted>>;


}