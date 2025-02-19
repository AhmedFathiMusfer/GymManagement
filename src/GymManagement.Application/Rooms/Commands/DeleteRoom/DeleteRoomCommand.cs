using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.DeleteRoom
{

    public record DeleteRoomCommand(Guid GymId, Guid RoomId)
        : IRequest<ErrorOr<Deleted>>;
}