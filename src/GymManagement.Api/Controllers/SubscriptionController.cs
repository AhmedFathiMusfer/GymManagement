using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GymManagement.Contracts.Subscriptions;
using System.Net.Http.Headers;
using MediatR;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;

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
            CreateSubscriptionCommand subscriptionCommand = new(request.AdminId, request.SubscriptionType.ToString());

            var CreateSubscriptionResult = await _mediator.Send(subscriptionCommand);
            return CreateSubscriptionResult.MatchFirst(
                 subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubscriptionType)),
                 Error => Problem());


        }
    }
}