using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription
{
    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository, IUnitOfWork unitOfWork, IAdminRepository adminRepository)
        {
            _subscriptionsRepository = subscriptionsRepository;
            _unitOfWork = unitOfWork;
            _adminRepository = adminRepository;
        }

        public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var admin = await _adminRepository.GetByIdAsync(request.AdminId);
            if (admin is null)
            {
                return Error.NotFound(description: "The admin is not found");
            }
            var subscription = new Subscription(
                id: Guid.NewGuid(),
                subscriptionType: request.SubscriptionType,
                adminId: request.AdminId

            );
            if (admin.SubscriptionId is not null)
            {
                return Error.Conflict(description: "admin has already an active subscription");
            }

            admin.SetSubscription(subscription);
            await _subscriptionsRepository.CreateSubscription(subscription);
            await _adminRepository.UpdateAsync(admin);
            await _unitOfWork.CommitChangesAsync();
            return subscription;
        }
    }
}