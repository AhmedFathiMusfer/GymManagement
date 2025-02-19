using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Contracts.Subscriptions;
using System.Net.Http.Headers;
using MediatR;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.OutputCaching;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;

using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;
using GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

namespace GymManagement.Api.Controllers
{

    [Route("api/[controller]")]
    public class SubscriptionController : ApiController
    {
        private readonly IMediator _mediator;


        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;

        }
        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
        {
            if (!DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var subscriptionType))
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: "Invaild Subscription Type"
                );
            }

            CreateSubscriptionCommand subscriptionCommand = new(request.AdminId, subscriptionType);

            var createSubscriptionResult = await _mediator.Send(subscriptionCommand);
            return createSubscriptionResult.Match(
            subscription => CreatedAtAction(
                nameof(GetSubscription),
                new { subscriptionId = subscription.Id },
                new SubscriptionResponse(
                    subscription.Id,
                    ToDto(subscription.SubscriptionType))),
            Problem);


        }
        [HttpGet("{subscriptionId:guid}")]
        public async Task<IActionResult> GetSubscription(Guid subscriptionId)
        {

            var GetSubscription = new GetSubscriptionQuery(subscriptionId);
            var getSubscriptionsResult = await _mediator.Send(GetSubscription);

            return getSubscriptionsResult.Match(
           subscription => Ok(new SubscriptionResponse(
               subscription.Id,
               ToDto(subscription.SubscriptionType))),
           Problem);


        }
        [HttpDelete("{subscriptionId:guid}")]
        public async Task<IActionResult> DeleteSubscription(Guid subscriptionId)
        {
            var deleteSubscription = new DeleteSubscriptionCommand(subscriptionId);
            var deleteSubscriptionResult = await _mediator.Send(deleteSubscription);
            return deleteSubscriptionResult.Match(
           _ => NoContent(),
           Problem);
        }
        private static SubscriptionType ToDto(DomainSubscriptionType subscriptionType)
        {
            return subscriptionType.Name switch
            {
                nameof(DomainSubscriptionType.Free) => SubscriptionType.Free,
                nameof(DomainSubscriptionType.Starter) => SubscriptionType.Starter,
                nameof(DomainSubscriptionType.Pro) => SubscriptionType.Pro,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}