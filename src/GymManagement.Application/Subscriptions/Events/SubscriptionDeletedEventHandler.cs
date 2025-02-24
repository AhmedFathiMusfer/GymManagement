using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins.Events;
using MediatR;

namespace GymManagement.Application.Subscriptions.Events
{
    public class SubscriptionDeletedEventHandler : INotificationHandler<SubscriptionDeletedEvent>
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionDeletedEventHandler(ISubscriptionsRepository subscriptionsRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionsRepository = subscriptionsRepository;
            _unitOfWork = unitOfWork;

        }
        public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionsRepository.GetSubscription(notification.sbscriptionId) ??
            throw new InvalidOperationException();
            await _subscriptionsRepository.RemoveSubscription(subscription);
            await _unitOfWork.CommitChangesAsync();
        }
    }
}