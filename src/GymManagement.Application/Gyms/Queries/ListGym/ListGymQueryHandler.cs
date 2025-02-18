using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.ListGym
{
    public class ListGymQueryHandler : IRequestHandler<ListGymQuery, ErrorOr<List<Gym>>>
    {

        private readonly IGymRepository _gymRepository;
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ListGymQueryHandler(IGymRepository gymRepository)
        {
            _gymRepository = gymRepository;

        }
        public async Task<ErrorOr<List<Gym>>> Handle(ListGymQuery request, CancellationToken cancellationToken)
        {
            var Gyms = await _gymRepository.ListBySubscriptionIdAsync(request.subscriptionId);
            return Gyms;
        }
    }
}