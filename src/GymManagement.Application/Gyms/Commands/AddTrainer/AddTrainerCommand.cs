using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.AddTrainer
{
    public record AddTrainerCommand(Guid subscriptionId, Guid gymId,
    Guid trainerId
    ) : IRequest<ErrorOr<Success>>;

}