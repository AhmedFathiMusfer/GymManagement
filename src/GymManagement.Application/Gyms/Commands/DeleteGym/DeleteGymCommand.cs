using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.DeleteGym
{
    public record DeleteGymCommand(Guid GymId, Guid SubscriptionId) : IRequest<ErrorOr<Deleted>>


}