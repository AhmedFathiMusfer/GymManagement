
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.DeleteRoom
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, ErrorOr<Deleted>>
    {
        private readonly IGymRepository _gymRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionsRepository _subscriptionsRepository;

        public DeleteRoomCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork, ISubscriptionsRepository subscriptionsRepository)
        {
            _unitOfWork = unitOfWork;
            _gymRepository = gymRepository;
            _subscriptionsRepository = subscriptionsRepository;
        }
        public async Task<ErrorOr<Deleted>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var gym = await _gymRepository.GetByIdAsync(request.GymId);

            if (gym is null)
            {
                return Error.NotFound(description: "Gym not found");
            }


            if (!gym.HasRoom(request.RoomId))
            {
                return Error.NotFound(description: "Room not found");
            }

            gym.RemoveRoom(request.RoomId);

            await _gymRepository.UpdateGymAsync(gym);
            await _unitOfWork.CommitChangesAsync();

            return Result.Deleted;
        }
    }
}