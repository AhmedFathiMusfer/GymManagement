
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Rooms;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
    {
        private readonly IGymRepository _gymRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionsRepository _subscriptionsRepository;

        public CreateRoomCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork, ISubscriptionsRepository subscriptionsRepository)
        {
            _unitOfWork = unitOfWork;
            _gymRepository = gymRepository;
            _subscriptionsRepository = subscriptionsRepository;
        }
        public async Task<ErrorOr<Room>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var gym = await _gymRepository.GetByIdAsync(request.GymId);
            if (gym == null)
            {
                return Error.NotFound(description: "The gym id not found");
            }
            var subscription = await _subscriptionsRepository.GetSubscription(gym.SubscriptionId);
            if (subscription is null)
            {
                return Error.Unexpected(description: "Subscription not found");
            }
            var room = new Room(
                name: request.RoomName,
                gymId: gym.Id,
                maxDailySessions: subscription.GetMaxDailySessions());

            var addGymResult = gym.AddRoom(room);
            if (addGymResult.IsError)
            {
                return addGymResult.Errors;
            }
            await _gymRepository.UpdateGymAsync(gym);
            await _unitOfWork.CommitChangesAsync();
            return room;

        }
    }
}