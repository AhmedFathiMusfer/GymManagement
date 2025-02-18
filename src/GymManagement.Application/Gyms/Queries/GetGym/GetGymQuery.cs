using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.GetGym
{
    public record GetGymQuery(Guid gymId, Guid subscriptionId) : IRequest<ErrorOr<Gym>>;

}