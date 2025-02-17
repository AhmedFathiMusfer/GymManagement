using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym
{
    public record CreatGymCommand(string Name, Guid SubscriptionId) : IRequest<ErrorOr<Gym>>;


}