using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins.Events;
using MediatR;

namespace GymManagement.Application.Gyms.Events
{
    public class SubscriptionDeletedEventHandler : INotificationHandler<SubscriptionDeletedEvent>
    {
        private readonly IGymRepository _gymRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionDeletedEventHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork)
        {
            _gymRepository = gymRepository;
            _unitOfWork = unitOfWork;

        }
        public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
        {
            var gyms = await _gymRepository.ListBySubscriptionIdAsync(notification.sbscriptionId);
            await _gymRepository.RemoveRangeAsync(gyms);
            await _unitOfWork.CommitChangesAsync();
        }
    }
}