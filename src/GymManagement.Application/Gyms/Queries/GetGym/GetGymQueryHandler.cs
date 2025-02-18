using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.GetGym
{
    public class GetGymQueryHandler : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
    {
        private readonly IGymRepository _gymRepository;
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GetGymQueryHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork, ISubscriptionsRepository subscriptionsRepository)
        {
            _gymRepository = gymRepository;
            _unitOfWork = unitOfWork;
            _subscriptionsRepository = subscriptionsRepository;
        }
        public async Task<ErrorOr<Gym>> Handle(GetGymQuery request, CancellationToken cancellationToken)
        {
            var gym = await _gymRepository.GetByIdAsync(request.gymId);
            if (gym == null)
            {
                return Error.NotFound(description: "The Gym is not found");
            }
            var subscription = await _subscriptionsRepository.GetSubscription(request.subscriptionId);
            if (subscription == null)
            {
                return Error.NotFound(description: "The subscribtion is not found");
            }
            if (!subscription.HasGym(request.gymId))
            {
                return Error.NotFound(description: "The Gym is not found");
            }
            return gym;
        }
    }
}