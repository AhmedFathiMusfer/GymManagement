
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription
{
    public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, ErrorOr<Deleted>>
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IAdminRepository _adminRepository;

        private readonly IGymRepository _gymRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository, IUnitOfWork unitOfWork, IAdminRepository adminRepository, IGymRepository gymRepository)
        {
            _subscriptionsRepository = subscriptionsRepository;
            _unitOfWork = unitOfWork;
            _adminRepository = adminRepository;
            _gymRepository = gymRepository;
        }
        public async Task<ErrorOr<Deleted>> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionsRepository.GetSubscription(request.subscriptionId);
            if (subscription is null)
            {
                return Error.NotFound(description: "The subscription is not found");
            }
            var admin = await _adminRepository.GetByIdAsync(subscription.AdminId);
            if (admin is null)
            {
                return Error.NotFound(description: "the admin is not found");
            }
            admin.DeleteSubscription(subscription.Id);
            var GymsToDelete = await _gymRepository.ListBySubscriptionIdAsync(subscription.Id);
            await _adminRepository.UpdateAsync(admin);
            await _subscriptionsRepository.RemoveSubscription(subscription);
            await _gymRepository.RemoveRangeAsync(GymsToDelete);
            await _unitOfWork.CommitChangesAsync();


            return Result.Deleted;
        }
    }
}