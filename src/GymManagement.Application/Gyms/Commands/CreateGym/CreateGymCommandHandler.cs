using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.CreateGym
{
    public class CreateGymCommandHandler : IRequestHandler<CreateGymCommand, ErrorOr<Gym>>
    {
        private readonly IGymRepository _gymRepository;
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGymCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork, ISubscriptionsRepository subscriptionsRepository)
        {
            _gymRepository = gymRepository;
            _unitOfWork = unitOfWork;
            _subscriptionsRepository = subscriptionsRepository;
        }

        public async Task<ErrorOr<Gym>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
        {

            var subscription = await _subscriptionsRepository.GetSubscription(request.SubscriptionId);
            if (subscription is null)
            {
                return Error.NotFound(description: "The subscription is not found ");

            }

            var gym = new Gym(name: request.Name, subscriptionId: request.SubscriptionId, maxRooms: 3);

            var AddGymResult = subscription.AddGym(gym);
            if (AddGymResult.IsError)
            {
                return AddGymResult.Errors;
            }

            await _subscriptionsRepository.UpdateSubscription(subscription);
            await _gymRepository.AddGymAsync(gym);
            await _unitOfWork.CommitChangesAsync();
            return gym;

        }
    }
}