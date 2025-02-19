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

    [Route("api/Subscription/{subscriptionId:guid}/Gyms")]
    public class GymController : ApiController
    {
        private readonly IMediator _mediator;
        public GymController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> CreateGym(CreateGymRequset requset, Guid subscriptionId)
        {
            var createGym = new CreateGymCommand(Name: requset.Name, SubscriptionId: subscriptionId);
            var gym = await _mediator.Send(createGym);
            return gym.Match(
            gym => CreatedAtAction(
                nameof(GetGym),
                new { subscriptionId, GymId = gym.Id },
                new GymResponse(gym.Id, gym.Name)),
            Problem);
        }
        [HttpGet("{gymId:guid}")]
        public async Task<IActionResult> GetGym(Guid gymId, Guid subscriptionId)
        {
            var GetGym = new GetGymQuery(gymId: gymId, subscriptionId: subscriptionId);
            var getGymResult = await _mediator.Send(GetGym);
            return getGymResult.Match(
           gym => Ok(new GymResponse(gym.Id, gym.Name)),
           Problem);
        }
        [HttpGet]
        public async Task<IActionResult> GetGyms(Guid subscriptionId)
        {
            var ListGym = new ListGymQuery(subscriptionId: subscriptionId);
            var listGymsResult = await _mediator.Send(ListGym);
            return listGymsResult.Match(
           gyms => Ok(gyms.ConvertAll(gym => new GymResponse(gym.Id, gym.Name))),
           Problem);
        }
        [HttpPost("{gymId:guid}/Trainer")]
        public async Task<IActionResult> AddTrainer(Guid gymId, AddTrainerRequest request, Guid subscriptionId)
        {

            var addTrainer = new AddTrainerCommand(gymId: gymId, subscriptionId: subscriptionId, trainerId: request.TrainerId);
            var addTrainerResult = await _mediator.Send(addTrainer);

            return addTrainerResult.Match(
                success => Ok(),
                Problem);
        }
        [HttpDelete("{gymId:guid}")]
        public async Task<IActionResult> DeleteGym(Guid subscriptionId, Guid gymId)
        {

            var DeleteGym = new DeleteGymCommand(GymId: gymId, SubscriptionId: subscriptionId);
            var Result = await _mediator.Send(DeleteGym);

            return Result.Match(
            _ => NoContent(),
              Problem);


        }
    }
}