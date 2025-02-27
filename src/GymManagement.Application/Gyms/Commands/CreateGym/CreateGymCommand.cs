using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Application.Common.Authorization;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym
{
    [Authorize(Permissions = "gyms:create")]
    public record CreateGymCommand(string Name, Guid SubscriptionId) : IRequest<ErrorOr<Gym>>;


}