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

namespace GymManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;


        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;

        }
        [HttpPost]
        public async Task<ActionResult> CreateSubscription(CreateSubscriptionRequest request)
        {
            if (!DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var subscriptionType))
            {
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: "Invaild Subscription Type"
                );
            }

            CreateSubscriptionCommand subscriptionCommand = new(request.AdminId, subscriptionType);

            var CreateSubscriptionResult = await _mediator.Send(subscriptionCommand);
            return CreateSubscriptionResult.MatchFirst(
                 subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubscriptionType)),
                 Error => Problem());


        }
        [HttpGet("{subscriptionId:guid}")]
        public async Task<ActionResult> GetSubscription(Guid subscriptionId)
        {

            var GetSubscription = new GetSubscriptionQuery(subscriptionId);
            var GetSubscriptionResult = await _mediator.Send(GetSubscription);

            return GetSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(subscription.Id,
                Enum.Parse<SubscriptionType>(subscription.SubscriptionType.Name))),
                error => Problem()

            );
        }
    }
}