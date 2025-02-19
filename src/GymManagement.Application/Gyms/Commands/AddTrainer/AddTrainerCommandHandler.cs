using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.AddTrainer
{
    public class AddTrainerCommandHandler : IRequestHandler<AddTrainerCommand, ErrorOr<Success>>
    {
        private readonly IGymRepository _gymRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddTrainerCommandHandler(IGymRepository gymRepository, IUnitOfWork unitOfWork)
        {
            _gymRepository = gymRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<Success>> Handle(AddTrainerCommand request, CancellationToken cancellationToken)
        {
            var gym = await _gymRepository.GetByIdAsync(request.gymId);

            if (gym == null)
            {
                return Error.NotFound(description: "the gym is not found");
            }

            var AddTrainerResult = gym.AddTrainer(trainerId: request.trainerId);
            if (AddTrainerResult.IsError)
            {
                return AddTrainerResult.Errors;
            }

            await _gymRepository.UpdateGymAsync(gym);
            await _unitOfWork.CommitChangesAsync();

            return Result.Success;

        }
    }
}