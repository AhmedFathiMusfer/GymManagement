using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.DeleteGym
{
    public class DeleteGymCommandHandler : IRequestHandler<DeleteGymCommand, ErrorOr<Deleted>>
    {
        private readonly IGymRepository _gymRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionsRepository _subscriptionsRepository;

        public DeleteGymCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork, ISubscriptionsRepository subscriptionsRepository)
        {
            _unitOfWork = unitOfWork;
            _gymRepository = gymRepository;
            _subscriptionsRepository = subscriptionsRepository;
        }

        public async Task<ErrorOr<Deleted>> Handle(DeleteGymCommand request, CancellationToken cancellationToken)
        {
            var gym = await _gymRepository.GetByIdAsync(request.GymId);
            if (gym == null)
            {
                return Error.NotFound(description: "The Gym is not found");
            }
            var subscription = await _subscriptionsRepository.GetSubscription(request.SubscriptionId);
            if (subscription == null)
            {
                return Error.NotFound(description: "The subscribtion is not found");
            }
            if (!subscription.HasGym(request.GymId))
            {
                return Error.NotFound(description: "The Gym is not found");
            }
            subscription.DeleteGym(request.GymId);
            await _subscriptionsRepository.UpdateSubscription(subscription);
            await _gymRepository.RemoveGymAsync(gym);
            await _unitOfWork.CommitChangesAsync();

            return Result.Deleted;

        }
    }
}