using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Application.Gyms.Commands.AddTrainer;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Application.Gyms.Commands.DeleteGym;
using GymManagement.Application.Gyms.Queries.GetGym;
using GymManagement.Application.Gyms.Queries.ListGym;
using GymManagement.Contracts.Gyms;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace GymManagement.Api.Controllers
{
    [ApiController]
    [Route("api/Subscription/{subscriptionId:guid}/Gyms")]
    public class GymController : ControllerBase
    {
        private readonly IMediator _mediator;
        public GymController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<ActionResult> CreateGym(CreateGymRequset requset, Guid subscriptionId)
        {
            var createGym = new CreateGymCommand(Name: requset.Name, SubscriptionId: subscriptionId);
            var gym = await _mediator.Send(createGym);
            return gym.MatchFirst(
                Gym => Ok(Gym),
                error => Problem(error.Code)
            );
        }
        [HttpGet("{gymId:guid}")]
        public async Task<ActionResult> GetGym(Guid gymId, Guid subscriptionId)
        {
            var GetGym = new GetGymQuery(gymId: gymId, subscriptionId: subscriptionId);
            var gym = await _mediator.Send(GetGym);
            return gym.MatchFirst(
                Gym => Ok(Gym),
                error => Problem(error.Code)
            );
        }
        [HttpGet]
        public async Task<ActionResult> GetGyms(Guid subscriptionId)
        {
            var ListGym = new ListGymQuery(subscriptionId: subscriptionId);
            var gym = await _mediator.Send(ListGym);
            return gym.MatchFirst(
                listGym => Ok(listGym),
                error => Problem(error.Code)
            );
        }
        [HttpPost("{gymId:guid}/Trainer")]
        public async Task<ActionResult> AddTrainer(Guid gymId, Guid trainerId, Guid subscriptionId)
        {
            var addTrainer = new AddTrainerCommand(gymId: gymId, subscriptionId: subscriptionId, trainerId: trainerId);
            var gym = await _mediator.Send(addTrainer);
            return gym.MatchFirst(
                addTrainer => Ok(addTrainer),
                error => Problem(error.Code)
            );
        }
        [HttpDelete("{gymId:guid}")]
        public async Task<ActionResult> DeleteGym(Guid subscriptionId, Guid gymId)
        {
            var DeleteGym = new DeleteGymCommand(GymId: gymId, SubscriptionId: subscriptionId);
            var Result = await _mediator.Send(DeleteGym);
            return Result.MatchFirst(
              result => Ok(result),
              error => Problem(error.Code)
          );

        }
    }
}